using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Gestion.managers
{
    class UserManager
    {
        private Api _api { get; set; }
        public List<User> cache { get; set; } = new List<User>();

        public UserManager(Api api) { _api = api; }

        private User ParseUser(dynamic data)
        {
            return new User(Convert.ToString(data.id), Convert.ToString(data.name), Convert.ToString(data.surname), Convert.ToString(data.mail), Convert.ToInt16(data.type), Convert.ToString(data?.password));
        }


        public async Task<List<User>> GetUsers()
        {
            HttpResponseMessage httpResponse = await this._api.client.GetAsync(this._api.host + "/api/v1/users");
            string parseResponse = await httpResponse.Content.ReadAsStringAsync();
            dynamic parsed = JsonConvert.DeserializeObject<dynamic>(parseResponse);

            foreach(dynamic u in parsed)
            {
                this.cache.Add(this.ParseUser(u));
            }
            return this.cache;
        }

        public async Task<User> PostUser(User user)
        {
            var Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await this._api.client.PostAsync(this._api.host + "/api/v1/users", Content);
            string parseResponse = await httpResponse.Content.ReadAsStringAsync();
            dynamic parsed = JsonConvert.DeserializeObject<dynamic>(parseResponse);

            this.cache.Add(this.ParseUser(parsed));
            return this.ParseUser(parsed);
        }

        public async Task<User> PutUser(User user)
        {
            var Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await this._api.client.PutAsync(this._api.host + "/api/v1/users/" + user.id, Content);
            string parseResponse = await httpResponse.Content.ReadAsStringAsync();
            dynamic parsed = JsonConvert.DeserializeObject<dynamic>(parseResponse);

            User editUser = this.ParseUser(parsed);
            foreach(User u in this.cache)
            {
                if(u.id == editUser.id)
                {
                    this.cache.Remove(u);
                    break;
                }
            }
            this.cache.Add(editUser);
            return editUser;
        }
    }
}
