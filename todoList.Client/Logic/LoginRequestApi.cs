using System;
using System.Net.Http;
using System.Threading.Tasks;
using todoList.Client.Model;
using Newtonsoft.Json;

namespace todoList.Client.Logic
{
    public class LoginRequestApi
    {

        public static async Task<string> Login(string login, string senha)
        {
            var client = new HttpClient { BaseAddress = new Uri(App.BaseUrl) };

            client
                .DefaultRequestHeaders
                .Add("Accept", "application/json");

            var content =
                string.Format("grant_type=password&username={0}&password={1}",
                    login, senha);

            var response =
                await client
                    .PostAsync(
                        "Token",
                        new StringContent(content)
                    );

            try
            {
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var tokenObj = JsonConvert.DeserializeObject<Token>(json);

                return tokenObj.access_token;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
