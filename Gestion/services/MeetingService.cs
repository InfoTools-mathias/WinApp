using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion
{
    class MeetingService
    {
        public List<Meeting> cache = new List<Meeting>();
        private string url = "/api/v1/meetings";

        private Api client { get; set; }
        public MeetingService(Api api) { client = api; }


        public async Task<string> Get()
        {
            cache.Clear();
            dynamic meetings = await client.GetRequest(url);

            foreach (dynamic m in meetings)
            {
                List<User> users = new List<User>();
                foreach (dynamic u in m.users)
                {
                    users.Add(new User(
                        Convert.ToString(u.id),
                        Convert.ToString(u.name),
                        Convert.ToString(u.surname),
                        Convert.ToString(u.mail),
                        Convert.ToInt16(u.type),
                        Convert.ToString(u.password)
                    ));
                }
                cache.Add(new Meeting(
                    Convert.ToString(m.id),
                    Convert.ToDateTime(m.date),
                    Convert.ToString(m.zip),
                    Convert.ToString(m.adress),
                    users
                ));
            }

            return "ok";
        }
        public async Task<string> Post(Meeting m)
        {
            await client.PostRequest(url, m);
            return "ok";
        }
        public async Task<string> Put(Meeting m)
        {
            await client.PutRequest(url + "/" + m.id, m);
            return "ok";
        }
        public async Task<string> Delete(string id)
        {
            await client.DeleteRequest(url + "/" + id);
            return "ok";
        }
    }
}
