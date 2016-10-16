using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace todoList.Client.Logic
{
    public class NovoUsuarioRequestApi
    {
        public static async Task<bool> Cadastrar(
            string email, string senha, string confirmacao)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(App.BaseUrl)
            };

            client
                .DefaultRequestHeaders
                .Add("Accept", "application/json");

            var novoUsuario = new
            {
                Email = email,
                Password = senha,
                ConfirmPassword = confirmacao
            };

            var json = JsonConvert.SerializeObject(novoUsuario);

            var dados = new StringContent(
                json, Encoding.UTF8, "application/json"
                );

            var response = await client
                            .PostAsync(
                                "api/Account/Register",
                                dados
                            );

            try
            {
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
