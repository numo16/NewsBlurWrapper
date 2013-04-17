using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ayls.NewsBlur
{
    public class NewsBlurClient
    {
        private string _baseUrl = "https://www.newsblur.com";
        private HttpClient _client;
        private string _username;
        private string _password;

        protected HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = InitializeClient();
                }

                return _client;
            }
        }

        protected HttpClient InitializeClient()
        {
            var cookieJar = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieJar,
                UseCookies = true,
                UseDefaultCredentials = false
            };

            return new HttpClient(handler)
                {
                    BaseAddress = new Uri(_baseUrl)
                };
        }

        public async Task<bool> Login(string username, string password)
        {
            _username = username;
            _password = password;

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", _username), 
                    new KeyValuePair<string, string>("password", _password)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/api/login")
                {
                    Content = content
                };
            var response = await Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var converter = new JsonSerializer();
                var loginResponse =
                    converter.Deserialize<LoginResponse>(
                        new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));

                return loginResponse.IsAuthenticated;
            }
            else
            {
                throw new HttpRequestException(string.Format("Server returned {0}", response.StatusCode));
            }


            return false;
        }

        public async Task<SignupResult> Signup(string username, string password)
        {
            _username = username;
            _password = password;

            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", _username), 
                    new KeyValuePair<string, string>("password", _password)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/api/signup")
            {
                Content = content
            };
            var response = await Client.SendAsync(request);
            var result = new SignupResult() { Errors = new List<string>() };
            if (response.IsSuccessStatusCode)
            {
                var converter = new JsonSerializer();
                var signupResponse = converter.Deserialize<SignupResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));
                result.IsAuthenticated = signupResponse.IsAuthenticated;
                if (!result.IsAuthenticated)
                {
                    foreach (var errorToken in signupResponse.Errors)
                    {
                        result.Errors.AddRange(JsonConvert.DeserializeObject<string[]>(errorToken.Value.ToString()));
                    }
                }
            }
            else
            {
                throw new HttpRequestException(string.Format("Server returned {0}", response.StatusCode));
            }

            return result;
        }

        public async Task<bool> Logout()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/api/logout");

            var response = await Client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        private async Task<T> ApiMethodRunner<T>(Func<Task<T>> task)
        {
            string errorMessage = null;
            try
            {
                return await task.Invoke();
            }
            catch (HttpRequestException e)
            {
                errorMessage = e.Message;
                if (!e.Message.Contains("403"))
                {
                    throw;
                }
            }

            if (await Login(_username, _password))
            {
                return await task.Invoke();
            }

            throw new HttpRequestException(errorMessage);
        }

        public async Task<IEnumerable<FeedResult>> GetFeeds()
        {
            var result = new Collection<FeedResult>();

            var response = await ApiMethodRunner<Stream>(async () => await Client.GetStreamAsync(_baseUrl + "/reader/feeds"));

            var converter = new JsonSerializer();
            var feedList = converter.Deserialize<FeedsResponse>(new JsonTextReader(new StreamReader(response)));
            foreach (var feedToken in feedList.Feeds)
            {
                var feedResult = JsonConvert.DeserializeObject<FeedResult>(feedToken.Value.ToString());
                if (feedResult.Active)
                {
                    result.Add(feedResult);
                }
            }

            foreach (var groupToken in feedList.Groups)
            {
                ProcessGroups(result, groupToken, 0, null);
            }

            return result;
        }

        private void ProcessGroups(IEnumerable<FeedResult> feeds, JToken groupToken, int level, string currentGroupName)
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

        public async Task<IEnumerable<FeedUnreadCountResult>> GetFeedUnreadCount()
        {
            var result = new Collection<FeedUnreadCountResult>();

            var response = await ApiMethodRunner<Stream>(async () => await Client.GetStreamAsync(_baseUrl + "/reader/refresh_feeds"));

            var converter = new JsonSerializer();
            var feedsUnreadCountResponse = converter.Deserialize<FeedsUnreadCountResponse>(new JsonTextReader(new StreamReader(response)));
            foreach (var feedToken in feedsUnreadCountResponse.Feeds)
            {
                var feedUnreadCount = JsonConvert.DeserializeObject<FeedUnreadCountResult>(feedToken.Value.ToString());
                result.Add(feedUnreadCount);
            }

            return result;
        }

        public async Task<IEnumerable<StorySummaryResult>> GetStories(string feedId, int page)
        {
            var response = await ApiMethodRunner<Stream>(async () => await Client.GetStreamAsync(_baseUrl + "/reader/feed/" + feedId + "?include_story_content=false&page=" + page));

            var converter = new JsonSerializer();
            var storiesResponse = converter.Deserialize<StoriesResponse>(new JsonTextReader(new StreamReader(response)));
          
            return storiesResponse.Stories;
        }

        public async Task<bool> MarkStoryAsRead(string feedId, string storyId)
        {
            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("story_id", storyId), 
                    new KeyValuePair<string, string>("feed_id", feedId)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/reader/mark_story_as_read")
            {
                Content = content
            };
            var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request));

            return response.IsSuccessStatusCode;
        }

        public async Task<MarkStoryAsUnreadResult> MarkStoryAsUnread(string feedId, string storyId)
        {
            var content = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("story_id", storyId), 
                    new KeyValuePair<string, string>("feed_id", feedId)
                });

            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/reader/mark_story_as_unread")
            {
                Content = content
            };
            var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request));

            var result = new MarkStoryAsUnreadResult();
            if (response.IsSuccessStatusCode)
            {
                var converter = new JsonSerializer();
                var addFeedResponse = converter.Deserialize<MarkStoryAsUnreadResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));
                if (!addFeedResponse.IsSuccess)
                {
                    result.Error = addFeedResponse.Error;
                }
            }
            else
            {
                throw new HttpRequestException(string.Format("Server returned {0}", response.StatusCode));
            }

            return result;
        }

        public async Task<AddFeedResult> AddFeed(string link, string folder)
        {
            var values = new Collection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("url", link), 
                };

            if (!string.IsNullOrEmpty(folder))
            {
                values.Add(new KeyValuePair<string, string>("folder", folder));
            }

            var content = new FormUrlEncodedContent(values);

            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/reader/add_url")
            {
                Content = content
            };
            var response = await ApiMethodRunner<HttpResponseMessage>(async () => await Client.SendAsync(request));

            var result = new AddFeedResult();
            if(response.IsSuccessStatusCode)
            {
                var converter = new JsonSerializer();
                var addFeedResponse = converter.Deserialize<AddFeedResponse>(new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())));
                result.Feed = addFeedResponse.Feed;
                result.Feed.Group = folder;
                if (!result.IsSuccess)
                {
                    result.Error = addFeedResponse.Error;
                }
            }
            else
            {
                throw new HttpRequestException(string.Format("Server returned {0}", response.StatusCode));
            }

            return result;
        }
    }
}
