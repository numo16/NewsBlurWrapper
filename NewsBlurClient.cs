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

            _username = username;
            _password = password;

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", _username), 
                    new KeyValuePair<string, string>("password", _password)
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
                    result = new LoginResult(loginResponse.IsAuthenticated);
                }
                else
                {
                    result = new LoginResult(string.Format("Server returned {0}", response.StatusCode), ApiCallStatus.Failed);
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

            _username = username;
            _password = password;

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", _username), 
                    new KeyValuePair<string, string>("password", _password)
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
                        if (signupResponse.Errors is JObject)
                        {
                            foreach (var errorToken in signupResponse.Errors)
                            {
                                result.Errors.AddRange(JsonConvert.DeserializeObject<string[]>(errorToken.ToString()));
                            }                      
                        }
                        else if (signupResponse.Errors != null)
                        {
                            result.Errors.Add(JsonConvert.DeserializeObject<string>(signupResponse.Errors.ToString()));
                        }
                    }
                }
                else
                {
                    result = new SignupResult(string.Format("Server returned {0}", response.StatusCode), ApiCallStatus.Failed);
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
                    result = new LogoutResult(string.Format("Server returned {0}", response.StatusCode), ApiCallStatus.Failed);
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
                foreach (var feedToken in feedResponse.Feeds)
                {
                    var feedSummaryResult = JsonConvert.DeserializeObject<FeedSummaryResult>(feedToken.Value.ToString());
                    if (feedSummaryResult.Active)
                    {
                        feedSummaryResults.Add(feedSummaryResult);
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
                    var feedUnreadCountSummaryResult = JsonConvert.DeserializeObject<FeedUnreadCountSummaryResult>(feedToken.Value.ToString());
                    feedUnreadCountSummaryResults.Add(feedUnreadCountSummaryResult);
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
                    result = new AddFeedResult(addFeedResponse.Feed, addFeedResponse.IsFeedAdded);
                    result.Feed.Group = folder;
                    if (!result.IsFeedAdded)
                    {
                        result.Errors.Add(addFeedResponse.Error);
                    }
                }
                else
                {
                    result = new AddFeedResult(string.Format("Server returned {0}", response.StatusCode), ApiCallStatus.Failed);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new AddFeedResult(m, s));
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
                    result = new MarkStoryAsReadResult();
                }
                else
                {
                    result = new MarkStoryAsReadResult(string.Format("Server returned {0}", response.StatusCode), ApiCallStatus.Failed);
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
                    var addFeedResponse = converter.Deserialize<MarkStoryAsUnreadResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));

                    result = new MarkStoryAsUnreadResult(addFeedResponse.IsMarkedAsUnread);
                    if (!addFeedResponse.IsMarkedAsUnread)
                    {
                        result.Errors.Add(addFeedResponse.Error);
                    }
                }
                else
                {
                    result = new MarkStoryAsUnreadResult(string.Format("Server returned {0}", response.StatusCode), ApiCallStatus.Failed);
                }
            }
            catch (Exception e)
            {
                result = HandleException(e, (m, s) => new MarkStoryAsUnreadResult(m, s));
            }

            return result;
        }
    }
}