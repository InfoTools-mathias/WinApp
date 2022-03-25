using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Gestion
{
    class Api
    {
        private string token { get; set; }

        //public string host = "http://172.31.247.13:5000";
        public string host = "http://localhost:5000";
        public HttpClient client = new HttpClient();

        public MeetingService meetings { get; set; }
        public ProductService products { get; set; }
        public UserService users { get; set; }
        public Api()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            meetings = new MeetingService(this);
            products = new ProductService(this);
            users = new UserService(this);
        }

        public async Task<string> Start()
        {
            await meetings.Get();
            await products.Get();
            await users.Get();
            return "ok";
        }

        public async Task<dynamic> GetRequest(string url)
        {
            try
            {
                HttpResponseMessage httpResponse = await this.client.GetAsync(this.host + url);
                string parseMessage = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(parseMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : " + ex.Message);
                return null;
            }
        }
        public async Task<HttpResponseMessage> PostRequest(string url, dynamic data)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                //Console.WriteLine("retour de l'api : " + await client.PostAsync(host + url, content));
                return await client.PostAsync(host + url, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : " + ex.Message);
                return null;
            }
        }
        public async Task<HttpResponseMessage> PutRequest(string url, dynamic data)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                //Console.WriteLine("retour de l'api : " + await client.PutAsync(host + url, content));
                return await client.PutAsync(host + url, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : " + ex.Message);
                return null;
            }
        }
        public async Task<HttpResponseMessage> DeleteRequest(string url)
        {
            try
            {
                return await client.DeleteAsync(host + url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : " + ex.Message);
                return null;
            }
        }


        #region auth
        public async Task<string> auth(User tmpUser)
        {
            try
            {
                //encodage mail et mot de passe
                string tmp = Convert.ToBase64String(Encoding.UTF8.GetBytes(tmpUser.mail + ":" + tmpUser.password));
                //création du header de la requête
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", tmp);

                //envoi de la requête api et récupération de la réponse
                StringContent content = new StringContent(JsonConvert.SerializeObject(tmpUser), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(host + "/api/v1/oauth/password", content);
                dynamic deserialize = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                //stockage du token
                token = deserialize.token;

                //envoi de la réponse api
                return response.StatusCode.ToString();
            }
            catch (JsonException ex)
            {
                Console.WriteLine("error : " + ex.Message);
                return null;
            }
        }
        public void killtoken()
        {
            token = "";
        }
        #endregion
    }
}
