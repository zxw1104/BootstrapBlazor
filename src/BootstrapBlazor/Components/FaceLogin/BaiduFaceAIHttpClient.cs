using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BaiduFaceAIHttpClient
    {
        private HttpClient _client;
        private string _appKey;
        private string _secretKey;

        private static AccessTokenResult AccessToken { get; set; } = new AccessTokenResult();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="config"></param>
        public BaiduFaceAIHttpClient(HttpClient client, IConfiguration config)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://aip.baidubce.com/rest/2.0/face/v3/faceset/user/");
            _appKey = config["BaiduFace:AppKey"];
            _secretKey = config["BaiduFace:SecretKey"];
        }

        private async Task GetAccessToken()
        {
            string authHost = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={_appKey}&client_secret={_secretKey}";

            var resp = await _client.PostAsJsonAsync<string>(authHost, "");
            if (resp.IsSuccessStatusCode)
            {
                AccessToken = await resp.Content.ReadFromJsonAsync<AccessTokenResult>();
                AccessToken.Cache();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Register()
        {
            if (string.IsNullOrEmpty(AccessToken.Token)) await GetAccessToken();

            // register face
            var url = $"https://aip.baidubce.com/rest/2.0/face/v3/faceset/user/add?access_token={AccessToken.Token}";
        }

        private class AddFaceBody
        {
            [JsonPropertyName("image")]
            public string Image { get; set; } = "";

            [JsonPropertyName("image_type")]
            public string ImageType { get; set; } = "BASE64";

            [JsonPropertyName("group_id")]
            public string GroupId { get; set; } = "BB_TEST";

            [JsonPropertyName("user_id")]
            public string UserId { get; set; } = "BB_User_";
        }

        private class AccessTokenResult
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("access_token")]
            public string Token { get; set; } = "";

            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("expires_in")]
            public double Expires { get; set; }

            private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

            /// <summary>
            /// 
            /// </summary>
            public void Cache()
            {
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(Expires), _cancellationToken.Token);

                    if (!_cancellationToken.IsCancellationRequested)
                    {
                        Token = "";
                    }
                });
            }
        }
    }
}
