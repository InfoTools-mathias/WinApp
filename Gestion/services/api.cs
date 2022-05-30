﻿using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Gestion
{
    class Api
    {
        private User me;

        //public string host = "http://172.31.247.13:5000";
        public string host = "http://localhost:5000";
        public HttpClient client = new HttpClient();

        public CategorieService categories { get; set; }
        public MeetingService meetings { get; set; }
        public ProductService products { get; set; }
        public UserService users { get; set; }
        public FactureService factures { get; set; }
        public LigneFactureService lignes { get; set; }
        public Api()
        {
            categories = new CategorieService(this);
            meetings = new MeetingService(this);
            products = new ProductService(this);
            users = new UserService(this);
            factures = new FactureService(this);
            lignes = new LigneFactureService(this);
        }

        public async Task<string> Start()
        {
            await categories.Get();
            await meetings.Get();
            await products.Get();
            await users.Get();
            await factures.Get();
            return "ok";
        }

        public async Task<dynamic> GetRequest(string url)
        {
            try
            {
                //Console.WriteLine(client.DefaultRequestHeaders);
                HttpResponseMessage httpResponse = await client.GetAsync(host + url);
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
                    Console.WriteLine("ce que j'envoie :");
                    Console.WriteLine(content.ReadAsStringAsync().Result);
                HttpResponseMessage response = await client.PostAsync(host + url, content);
                    Console.WriteLine("retour de l'api :");
                    Console.WriteLine(response);
                return response;
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
                    Console.WriteLine("ce que j'envoie :");
                    Console.WriteLine(content.ReadAsStringAsync().Result);
                HttpResponseMessage response = await client.PutAsync(host + url, content);
                    Console.WriteLine("retour de l'api :");
                    Console.WriteLine(response);
                return response;
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
                string parseMessage = await response.Content.ReadAsStringAsync();
                dynamic deserialize = JsonConvert.DeserializeObject(parseMessage);

                //stockage du token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Convert.ToString(deserialize.token));


                dynamic res = await GetRequest("/api/v1/oauth/@me");
                me = new User(
                    Convert.ToString(res.id),
                    Convert.ToString(res.name),
                    Convert.ToString(res.surname),
                    Convert.ToString(res.mail),
                    Convert.ToInt16(res.type),
                    Convert.ToString(res.password)
                );

                //envoi de la réponse api
                return response.StatusCode.ToString();
            }
            catch (JsonException ex)
            {
                Console.WriteLine("error : " + ex.Message);
                return "error";
            }
        }
        public void killtoken()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
        }
        #endregion
    }
}
