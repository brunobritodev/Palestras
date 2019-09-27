using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace BackgroundService.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "vssummit-client-credentials-acesso-total",
                ClientSecret = "super-secret",
                Scope = "vssummit-financeiro.leitura vssummit-financeiro.escrita"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine("Access Token");
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            await LerCotacao(tokenResponse);
            await ReiniciarCache(tokenResponse);
            await AtualizarCotacao(tokenResponse);

            Console.Read();
        }

        private static async Task LerCotacao(TokenResponse tokenResponse)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("http://localhost:5020/cotacao/cripto-moedas");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }

        private static async Task ReiniciarCache(TokenResponse tokenResponse)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.PostAsync("http://localhost:5020/admin/acesso-restrito", null);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JObject.Parse(content));
            }
        }

        private static async Task AtualizarCotacao(TokenResponse tokenResponse)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.PutAsync("http://localhost:5020/cotacao/atualizar-cripto-moeda", null);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JObject.Parse(content));
            }
        }
    }
}
