using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion
{
    class ProductService
    {
        public List<Product> cache = new List<Product>();
        private string url = "/api/v1/products";

        private Api client { get; set; }
        public ProductService(Api api) { client = api; }


        public async Task<string> Get()
        {
            cache.Clear();
            dynamic products = await client.GetRequest(url);

            foreach (dynamic p in products)
            {
                List<Categorie> categories = new List<Categorie>();
                foreach (dynamic c in p.categories)
                {
                    categories.Add(new Categorie(
                        Convert.ToString(c.id),
                        Convert.ToString(c.name)
                    ));
                }

                cache.Add(new Product(
                    Convert.ToString(p.id),
                    Convert.ToString(p.name),
                    Convert.ToDouble(p.price),
                    Convert.ToInt16(p.quantity),
                    Convert.ToString(p.description),
                    categories
                ));
            }

            return "ok";
        }
        public async Task<string> Post(Product p)
        {
            await client.PostRequest(url, p);
            return "ok";
        }
        public async Task<string> Put(Product p)
        {
            await client.PutRequest(url + "/" + p.id, p);
            return "ok";
        }
        public async Task<string> Delete(string id)
        {
            await client.DeleteRequest(url + "/" + id);
            return "ok";
        }
    }
}
