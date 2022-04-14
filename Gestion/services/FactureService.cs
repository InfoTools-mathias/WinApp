using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion
{
    class FactureService
    {
        public List<Facture> cache = new List<Facture>();
        private string url = "/api/v1/factures";

        private Api client { get; set; }
        public FactureService(Api api) { client = api; }


        public async Task<string> Get()
        {
            cache.Clear();
            dynamic factures = await client.GetRequest(url);

            foreach(dynamic f in factures)
            {
                List<LigneFacture> lignes = new List<LigneFacture>();
                foreach(dynamic l in f.lignes)
                {
                    lignes.Add(new LigneFacture(
                        Convert.ToString(l.id),
                        Convert.ToString(l.product),
                        Convert.ToInt16(l.quantity),
                        Convert.ToDouble(l.Price),
                        Convert.ToString(l.factureId)
                    ));
                }
                cache.Add(new Facture(
                    Convert.ToString(f.id),
                    Convert.ToDateTime(f.date),
                    lignes,
                    Convert.ToString(f.clientId)
                ));
            }

            return "ok";
        }
        public async Task<string> Post(Facture f)
        {
            await client.PostRequest(url, f);
            return "ok";
        }
        public async Task<string> Put(Facture f)
        {
            await client.PutRequest(url + "/" + f.id, f);
            return "ok";
        }
        public async Task<string> Delete(string id)
        {
            await client.DeleteRequest(url + "/" + id);
            return "ok";
        }
    }
}
