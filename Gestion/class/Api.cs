using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Gestion
{
    class Api
    {
        private string host;
        private HttpClient client;
        private static string token;

        public Api()
        {
            host = "http://172.31.247.13:5000";
            client = new HttpClient();
        }

        #region auth
        public async Task<string> auth(auth tmpAuth)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try {
                //encodage mail et mot de passe
                string tmp = Convert.ToBase64String(Encoding.UTF8.GetBytes(tmpAuth.mail + ":" + tmpAuth.password));
                //création du header de la requête
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", tmp);

                //envoi de la requête api et récupération de la réponse
                var content = new StringContent(JsonConvert.SerializeObject(tmpAuth), Encoding.UTF8, "application/json");
                response = await client.PostAsync(host + "/api/v1/oauth/password", content);
                dynamic deserialize = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                //stockage du token
                token = deserialize.token;

                //envoi de la réponse api
                return response.StatusCode.ToString();
            } catch {
                return "error";
            }
        }
        public void killtoken()
        {
            token = "";
        }
        #endregion


        #region meeting
        public async Task<List<Meeting>> getMeetings()
        {
            HttpResponseMessage response = await client.GetAsync(host + "/api/v1/meetings");
            string parseResponse = await response.Content.ReadAsStringAsync();
            dynamic parsed = JsonConvert.DeserializeObject(parseResponse);

            List<Meeting> cMeeting = new List<Meeting>();
            foreach (dynamic i in parsed)
            {
                List<User> cUser = new List<User>();
                foreach (dynamic y in i.users)
                {
                    User user = new User(Convert.ToString(y.id), Convert.ToString(y.name), Convert.ToString(y.surname), Convert.ToString(y.mail), Convert.ToInt16(y.type), Convert.ToString(y.password));
                    cUser.Add(user);
                }
                Meeting meeting = new Meeting(Convert.ToString(i.id), Convert.ToDateTime(i.date), Convert.ToString(i.zip), Convert.ToString(i.adress), cUser);
                cMeeting.Add(meeting);
            }
            return cMeeting;
        }
        public async Task<HttpResponseMessage> postMeeting(Meeting tmpMeeting)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(tmpMeeting), Encoding.UTF8, "application/json");
            return await client.PostAsync(host + "/api/v1/meetings", content);
        }
        public async Task<HttpResponseMessage> putMeeting(Meeting tmpMeeting)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(tmpMeeting), Encoding.UTF8, "application/json");
            return await client.PutAsync(host + "/api/v1/meetings/" + tmpMeeting.id, content);
        }
        public async Task<HttpResponseMessage> deleteMeeting(string idMeeting)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await client.DeleteAsync(host + "/api/v1/meetings/" + idMeeting);
        }
        #endregion

        #region product
        public async Task<List<Product>> getProducts()
        {
            HttpResponseMessage response = await client.GetAsync(host + "/api/v1/products");
            string parseResponse = await response.Content.ReadAsStringAsync();
            dynamic parsed = JsonConvert.DeserializeObject(parseResponse);

            List<Product> cProduct = new List<Product>();
            foreach (dynamic i in parsed)
            {
                List<categorie> cType = new List<categorie>();
                foreach (dynamic y in i.categories)
                {
                    categorie thisType = new categorie(Convert.ToString(y.id), Convert.ToString(y.name));
                    cType.Add(thisType);
                }
                Product produit = new Product(Convert.ToString(i.id), Convert.ToString(i.name), Convert.ToDouble(i.price), Convert.ToInt16(i.quantity), Convert.ToString(i.description), cType);
                cProduct.Add(produit);
            }
            return cProduct;
        }
        public async Task<HttpResponseMessage> postProduct(Product tmpProduct)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(tmpProduct), Encoding.UTF8, "application/json");
            return await client.PostAsync(host + "/api/v1/products", content);
        }
        public async Task<HttpResponseMessage> putProduct(Product tmpProduct)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(tmpProduct), Encoding.UTF8, "application/json");
            return await client.PutAsync(host + "/api/v1/products/" + tmpProduct.id, content);
        }
        public async Task<HttpResponseMessage> deleteProduct(string idProduct)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await client.DeleteAsync(host + "/api/v1/products/" + idProduct);
        }
        #endregion

        #region user
        public async Task<List<User>> getUsers()
        {
            HttpResponseMessage response = await client.GetAsync(host + "/api/v1/users");
            string parseResponse = await response.Content.ReadAsStringAsync();
            dynamic parsed = JsonConvert.DeserializeObject(parseResponse);

            List<User> cUser = new List<User>();
            foreach (dynamic i in parsed)
            {
                User utilisateur = new User(Convert.ToString(i.id), Convert.ToString(i.name), Convert.ToString(i.surname), Convert.ToString(i.mail), Convert.ToInt16(i.type), Convert.ToString(i.password));
                cUser.Add(utilisateur);
            }
            return cUser;
        }
        public async Task<HttpResponseMessage> postUser(User tmpUser)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(tmpUser), Encoding.UTF8, "application/json");
            return await client.PostAsync(host + "/api/v1/users", content);
        }
        public async Task<HttpResponseMessage> putUser(User tmpUser)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(tmpUser), Encoding.UTF8, "application/json");
            return await client.PutAsync(host + "/api/v1/users/" + tmpUser.id, content);
        }
        public async Task<HttpResponseMessage> deleteUser(string idUser)
        {
            return await client.DeleteAsync(host + "/api/v1/users/" + idUser);
        }
        #endregion
    }
}
