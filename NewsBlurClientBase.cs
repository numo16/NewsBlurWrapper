using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ayls.NewsBlur.Results;

namespace Ayls.NewsBlur
{
    public abstract class NewsBlurClientBase
    {
        protected abstract string BaseUrl { get; }
        protected abstract string UserAgent { get; }

        private HttpClient _client;
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

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            client.DefaultRequestHeaders.Add("user-agent", UserAgent);

            return client;
        }

        protected T HandleException<T>(Exception e, Func<string, ApiCallStatus, T> resultDelegate)
        {
            T result;

            var innerWebException = e.InnerException as WebException;
            if (innerWebException != null && innerWebException.Status == WebExceptionStatus.RequestCanceled)
            {
                result = resultDelegate.Invoke("Request cancelled.", ApiCallStatus.Cancelled);
            }
            else if (innerWebException != null)
            {
                result = resultDelegate.Invoke(string.Format("{0}{1}", e.Message, innerWebException.Message), ApiCallStatus.CommunicationError);
            }
            else
            {
                result = resultDelegate.Invoke(e.Message, ApiCallStatus.CommunicationError);
            }

            return result;
        }

        protected async Task<T> ApiMethodRunner<T>(Func<Task<T>> task, Func<Task<LoginResult>> loginTask)
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

            var loginResult = await loginTask.Invoke();
            if (loginResult.Status == ApiCallStatus.Ok && loginResult.IsAuthenticated)
            {
                return await task.Invoke();
            }
            
            throw new HttpRequestException(errorMessage);
        }
    }
}