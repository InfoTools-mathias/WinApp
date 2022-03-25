using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion
{
    internal class CategorieService
    {
        public List<Categorie> cache = new List<Categorie>();
        private string url = "/api/v1/categories";

        private Api client { get; set; }
        public CategorieService(Api api) { client = api; }


        public async Task<string> Get()
        {
            cache.Clear();
            dynamic categories = await client.GetRequest(url);

            foreach (dynamic c in categories)
            {
                cache.Add(new Categorie(
                    Convert.ToString(c.id),
                    Convert.ToString(c.name)
                ));
            }

            return "ok";
        }
        public async Task<string> Post(Categorie c)
        {
            await client.PostRequest(url, c);
            return "ok";
        }
        public async Task<string> Put(Categorie c)
        {
            await client.PutRequest(url + "/" + c.id, c);
            return "ok";
        }
        public async Task<string> Delete(string id)
        {
            await client.DeleteRequest(url + "/" + id);
            return "ok";
        }
    }
}
