using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion
{
    class LigneFactureService
    {
        public List<LigneFacture> cache = new List<LigneFacture>();
        private string url = "/api/v1/factures/";

        private Api client { get; set; }
        public LigneFactureService(Api api) { client = api; }


        public async Task<string> Post(LigneFacture lf)
        {
            Console.WriteLine(url + lf.factureId + "/lignes", lf);
            await client.PostRequest(url + lf.factureId + "/lignes", lf);
            return "ok";
        }
        //public async Task<string> Put(LigneFacture lf)
        //{
        //    await client.PutRequest(url + lf.factureId + "/lignes" + lf.id, lf);
        //    return "ok";
        //}
        public async Task<string> Delete(string idF, string idLigneF)
        {
            await client.DeleteRequest(url + idF + "/lignes/" + idLigneF);
            return "ok";
        }
    }
}
