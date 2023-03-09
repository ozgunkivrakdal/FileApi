using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using FileApiContract.Model;
using FileOperationEngineContract.Model.Base;

namespace FileApiContract.Proxy
{
    public class BaseService
    {
        private readonly string PWD = "c28pMc*2Wgab5rWP";
        private string endPointUrl;
        private string controller;

        public BaseService(string endPointUrl, string controller)
        {
            this.endPointUrl = endPointUrl;
            this.controller = controller;
        }

        protected T Post<T>(BaseRequestModel request, string serviceName)
        {
            T response = default(T);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{endPointUrl}/{controller}/");
                    client.Timeout = request.timeout;

                    HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(request));
                    requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    requestContent.Headers.Add("UUID", request.uuid);

                    HttpResponseMessage httpResponse = Task.Run(async () => await (client.PostAsync(serviceName, requestContent))).Result;
                    string responseContent = Task.Run(async () => await httpResponse.Content.ReadAsStringAsync()).Result;
                    if (httpResponse.StatusCode != HttpStatusCode.OK || responseContent.Length == 0)
                    {
                        BaseResponseModel errObj = new BaseResponseModel();
                        errObj.success = false;
                        errObj.code = $"{(int)httpResponse.StatusCode}_{httpResponse.StatusCode}";
                        response = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(errObj));
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<T>(responseContent);
                    }
                }
            }
            catch (Exception e)
            {
                BaseResponseModel errObj = new BaseResponseModel();
                errObj.uuid = request.uuid;
                // errObj.set(EComResult.EXCEPTION, e.ToStr(100));
                ModifyError(ref errObj, request.timeout);
                response = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(errObj));
            }
            return response;
        }

        private void SetAuthorizationHeader(HttpClient client, SessionModel session)
        {
            if (client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Remove("Authorization");
            }
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Token(session)}");
        }

        private string Token(SessionModel session)
        {
            string plainToken = $"{session.userId}__{session.timezoneId}:{PWD}";
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainToken));
            return encodedToken;
        }

        private void ModifyError(ref BaseResponseModel response, TimeSpan timeout)
        {
            if (response.code.Contains("TaskCanceledException"))
            {
                response.code = "TIMEOUT";
                response.result = $"uri: {endPointUrl}/{controller}/  {timeout.TotalSeconds} SECONDS";
            }
            else if (response.code.Contains("AggregateException"))
            {
                int start = response.code.IndexOf("AggregateException:");
                int end = response.code.IndexOf("--->");
                response.code = "EXCEPTION";
                response.result = $"uri: {endPointUrl}/{controller}/  err:{response.code.Substring(start, end)}";
            }
        }
    }
}
