using Ayls.NewsBlur.Responses;
using Ayls.NewsBlur.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ayls.NewsBlur
{
    public class NewsBlurClient : NewsBlurClientBase
    {
        protected override string BaseUrl
        {
            get
            {
                return "https://www.newsblur.com";
            }
        }

        private string _username;
        private string _password;

        public async Task<LoginResult> Login(string username, string password)
        {
            LoginResult result; 

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", username), 
                    new KeyValuePair<string, string>("password", password)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/api/login")
                {
                    Content = content
                };

            try
            {
                var response = await Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
                    if (loginResponse.IsAuthenticated)
                    {
                        _username = username;
                        _password = password;
                        result = new LoginResult(loginResponse.IsAuthenticated);
                    }
                    else
                    {
                        result = new LoginResult("Failed to authenticate.", ApiCallStatus.Failed);
                    }
                }
                else
                {
                    result = new LoginResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new LoginResult(m, s));
            }

            return result;
        }

        public async Task<SignupResult> Signup(string username, string password)
        {
            SignupResult result;

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", username), 
                    new KeyValuePair<string, string>("password", password)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/api/signup")
            {
                Content = content
            };

            try
            {
                var response = await Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var signupResponse = JsonConvert.DeserializeObject<SignupResponse>(await response.Content.ReadAsStringAsync());
                    result = new SignupResult(signupResponse.IsAuthenticated);
                    if (!signupResponse.IsAuthenticated)
                    {
                        var errorList = new List<string>();
                        if (signupResponse.Errors is JObject)
                        {
                            var errors = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(signupResponse.Errors.ToString());
                            foreach (var msg in errors.Values)
                            {
                                errorList.AddRange(JsonConvert.DeserializeObject<string[]>(msg.ToString()));
                            }                      
                        }
                        else if (signupResponse.Errors != null)
                        {
                            errorList.Add(JsonConvert.DeserializeObject<string>(signupResponse.Errors.ToString()));
                        }
                        result = new SignupResult(errorList, ApiCallStatus.Failed);
                    }
                    else
                    {
                        _username = username;
                        _password = password;
                    }
                }
                else
                {
                    result = new SignupResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new SignupResult(m, s));
            }

            return result;
        }

        public async Task<LogoutResult> Logout()
        {
            LogoutResult result;

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/api/logout");

            try
            {
                var response = await Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    result = new LogoutResult();
                }
                else
                {
                    result = new LogoutResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new LogoutResult(m, s));
            }

            return result;
        }

        public async Task<GetGroupedFeedsResult> GetGroupedFeeds()
        {
            GetGroupedFeedsResult result;

            try
            {
                var response = await ApiMethodRunner<Stream>(async () => await Client.GetStreamAsync(BaseUrl + "/reader/feeds"),
                    async () => await Login(_username, _password));

                var converter = new JsonSerializer();
                var feedResponse = converter.Deserialize<FeedsResponse>(new JsonTextReader(new StreamReader(response)));

                var feedSummaryResults = new Collection<FeedSummaryResult>();
                if (feedResponse.Feeds is JObject)
                {
                    var feedSummaryResponse = JsonConvert.DeserializeObject<Dictionary<string, FeedSummaryResponse>>(feedResponse.Feeds.ToString());
                    foreach (var feedSummary in feedSummaryResponse.Values)
                    {
                        if (feedSummary.Active)
                        {
                            feedSummaryResults.Add(new FeedSummaryResult(feedSummary));
                        }
                    }
                }

                foreach (var groupToken in feedResponse.Groups)
                {
                    ProcessGroups(feedSummaryResults, groupToken, 0, null);
                }

                result = new GetGroupedFeedsResult(feedSummaryResults);
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new GetGroupedFeedsResult(m, s));
            }

            return result;
        }

        private void ProcessGroups(IEnumerable<FeedSummaryResult> feeds, JToken groupToken, int level, string currentGroupName)
        {
            if (groupToken is JObject)
            {
                var groupTokenKeyValue =
                    JsonConvert.DeserializeObject<Dictionary<string, JToken>>((groupToken.ToString())).First();

                foreach (var subGroupToken in groupTokenKeyValue.Value)
                {
                    var groupName = currentGroupName;
                    if (level < 1)
                    {
                        groupName = groupTokenKeyValue.Key;
                    }
                    ProcessGroups(feeds, subGroupToken, level + 1, groupName);
                }
            }
            else
            {
                var feedId = JsonConvert.DeserializeObject<string>((groupToken.ToString()));
                var feed = feeds.FirstOrDefault(x => x.Id == feedId);
                if (feed != null)
                {
                    feed.Group = currentGroupName;
                }         
            }
        }

        public async Task<GetFeedsUnreadCountResult> GetFeedsUnreadCount()
        {
            GetFeedsUnreadCountResult result;

            try
            {
                var response = await ApiMethodRunner<Stream>(async () => await Client.GetStreamAsync(BaseUrl + "/reader/refresh_feeds"),
                    async () => await Login(_username, _password));

                var converter = new JsonSerializer();
                var feedsUnreadCountResponse = converter.Deserialize<FeedsUnreadCountResponse>(new JsonTextReader(new StreamReader(response)));

                var feedUnreadCountSummaryResults = new Collection<FeedUnreadCountSummaryResult>();
                foreach (var feedToken in feedsUnreadCountResponse.Feeds)
                {
                    var unreadCountSummaryResponse = JsonConvert.DeserializeObject<FeedUnreadCountSummaryResponse>(feedToken.Value.ToString());
                    feedUnreadCountSummaryResults.Add(new FeedUnreadCountSummaryResult(unreadCountSummaryResponse));
                }

                result = new GetFeedsUnreadCountResult(feedUnreadCountSummaryResults);
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new GetFeedsUnreadCountResult(m, s));
            }

            return result;
        }

        public async Task<AddFeedResult> AddFeed(string link, string folder)
        {
            AddFeedResult result;

            var values = new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("url", link), 
                };

            if (!string.IsNullOrEmpty(folder))
            {
                values.Add(new KeyValuePair<string, string>("folder", folder));
            }

            var content = new FormUrlEncodedContent(values);

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/reader/add_url")
            {
                Content = content
            };

            try
            {
                var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request),
                    async () => await Login(_username, _password));

                if (response.IsSuccessStatusCode)
                {
                    var addFeedResponse = JsonConvert.DeserializeObject<AddFeedResponse>(await response.Content.ReadAsStringAsync());
                    if (addFeedResponse.IsFeedAdded)
                    {
                        result = new AddFeedResult(addFeedResponse.Feed, folder);
                    }
                    else
                    {
                        result = new AddFeedResult(addFeedResponse.Error, ApiCallStatus.Failed);
                    }
                }
                else
                {
                    result = new AddFeedResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new AddFeedResult(m, s));
            }

            return result;
        }

        public async Task<DeleteFeedResult> DeleteFeed(string feedId, string folder)
        {
            DeleteFeedResult result;

            var values = new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("feed_id", feedId)
                };

            if (!string.IsNullOrEmpty(folder))
            {
                values.Add(new KeyValuePair<string, string>("in_folder", folder));
            }

            var content = new FormUrlEncodedContent(values);

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/reader/delete_feed")
            {
                Content = content
            };

            try
            {
                var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request),
                    async () => await Login(_username, _password));

                if (response.IsSuccessStatusCode)
                {
                    var converter = new JsonSerializer();
                    var deleteFeedResponse = converter.Deserialize<DeleteFeedResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));
                    if (deleteFeedResponse.IsDeleted)
                    {
                        result = new DeleteFeedResult();
                    }
                    else
                    {
                        result = new DeleteFeedResult("Failed to delete feed.", ApiCallStatus.Failed);
                    }
                }
                else
                {
                    result = new DeleteFeedResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new DeleteFeedResult(m, s));
            }

            return result;
        }

        public async Task<MarkFeedAsReadResult> MarkFeedAsRead(string feedId)
        {
            MarkFeedAsReadResult result;

            var values = new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("feed_id", feedId)
                };

            var content = new FormUrlEncodedContent(values);

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/reader/mark_feed_as_read")
            {
                Content = content
            };

            try
            {
                var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request),
                    async () => await Login(_username, _password));

                if (response.IsSuccessStatusCode)
                {
                    var converter = new JsonSerializer();
                    var markFeedAsReadResponse = converter.Deserialize<MarkFeedAsReadResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));
                    if (markFeedAsReadResponse.IsMarkedAsRead)
                    {
                        result = new MarkFeedAsReadResult();
                    }
                    else
                    {
                        result = new MarkFeedAsReadResult("Failed to mark feed as read.", ApiCallStatus.Failed);
                    }
                }
                else
                {
                    result = new MarkFeedAsReadResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new MarkFeedAsReadResult(m, s));
            }

            return result;
        }

        public async Task<GetStoriesResult> GetStories(string feedId, int page)
        {
            GetStoriesResult result;

            try
            {
                var response = await ApiMethodRunner<Stream>(async () => await Client.GetStreamAsync(BaseUrl + "/reader/feed/" + feedId + "?include_story_content=false&page=" + page),
                    async () => await Login(_username, _password));

                var converter = new JsonSerializer();
                var storiesResponse = converter.Deserialize<StoriesResponse>(new JsonTextReader(new StreamReader(response)));

                result = new GetStoriesResult(storiesResponse.Stories);
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new GetStoriesResult(m, s));
            }
          
            return result;
        }

        public async Task<MarkStoryAsReadResult> MarkStoryAsRead(string feedId, string storyId)
        {
            MarkStoryAsReadResult result;

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("story_id", storyId), 
                    new KeyValuePair<string, string>("feed_id", feedId)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/reader/mark_story_as_read")
            {
                Content = content
            };

            try
            {
                var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request),
                    async () => await Login(_username, _password));

                if (response.IsSuccessStatusCode)
                {
                    var converter = new JsonSerializer();
                    var markStoryAsReadResponse = converter.Deserialize<MarkStoryAsReadResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));
                    if (markStoryAsReadResponse.IsMarkedAsRead)
                    {
                        result = new MarkStoryAsReadResult();
                    }
                    else
                    {
                        result = new MarkStoryAsReadResult(markStoryAsReadResponse.Errors, ApiCallStatus.Failed);           
                    }
                }
                else
                {
                    result = new MarkStoryAsReadResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new MarkStoryAsReadResult(m, s));
            }

            return result;
        }

        public async Task<MarkStoryAsUnreadResult> MarkStoryAsUnread(string feedId, string storyId)
        {
            MarkStoryAsUnreadResult result;

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("story_id", storyId), 
                    new KeyValuePair<string, string>("feed_id", feedId)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/reader/mark_story_as_unread")
            {
                Content = content
            };

            try
            {
                var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request),
                    async () => await Login(_username, _password));

                if (response.IsSuccessStatusCode)
                {
                    var converter = new JsonSerializer();
                    var markStoryAsUnreadResponse = converter.Deserialize<MarkStoryAsUnreadResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));
                    if (markStoryAsUnreadResponse.IsMarkedAsUnread)
                    {
                        result = new MarkStoryAsUnreadResult();
                    }
                    else
                    {
                        result = new MarkStoryAsUnreadResult(markStoryAsUnreadResponse.Error, ApiCallStatus.Failed);
                    }
                }
                else
                {
                    result = new MarkStoryAsUnreadResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new MarkStoryAsUnreadResult(m, s));
            }

            return result;
        }

        public async Task<MarkStoryAsStarredResult> MarkStoryAsStarred(string feedId, string storyId)
        {
            MarkStoryAsStarredResult result;

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("story_id", storyId), 
                    new KeyValuePair<string, string>("feed_id", feedId)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/reader/mark_story_as_starred")
            {
                Content = content
            };

            try
            {
                var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request),
                    async () => await Login(_username, _password));

                if (response.IsSuccessStatusCode)
                {
                    var converter = new JsonSerializer();
                    var markStoryAsStarredResponse = converter.Deserialize<MarkStoryAsStarredResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));
                    if (markStoryAsStarredResponse.IsMarkedAsStarred)
                    {
                        result = new MarkStoryAsStarredResult();
                    }
                    else
                    {
                        result = new MarkStoryAsStarredResult("Failed to mark story as starred.", ApiCallStatus.Failed);
                    }
                }
                else
                {
                    result = new MarkStoryAsStarredResult(string.Format("Server returned {0}.", response.StatusCode), ApiCallStatus.CommunicationError);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new MarkStoryAsStarredResult(m, s));
            }

            return result;
        }
    }
}