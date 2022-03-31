﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion
{
    internal class UserService
    {
        public List<User> cache = new List<User>();
        private string url = "/api/v1/users";

        private Api client { get; set; }
        public UserService(Api api) { client = api; }


        public async Task<string> Get()
        {
            cache.Clear();
            dynamic users = await client.GetRequest(url);

            foreach (dynamic u in users)
            {
                List<Facture> factures = new List<Facture>();
                foreach (dynamic f in u.factures)
                {
                    List<LigneFacture> lignes = new List<LigneFacture>();
                    foreach (dynamic l in f.lignes)
                    {
                        lignes.Add(new LigneFacture(
                            Convert.ToString(l.id),
                            Convert.ToString(l.product),
                            Convert.ToInt16(l.quantity),
                            Convert.ToDouble(l.price),
                            Convert.ToString(l.factureId)
                        )); ;
                    }

                    factures.Add(new Facture(
                        Convert.ToString(f.id),
                        Convert.ToDateTime(f.date),
                        lignes,
                        Convert.ToString(f.clientId)
                    ));
                }

                cache.Add(new User(
                    Convert.ToString(u.id),
                    Convert.ToString(u.name),
                    Convert.ToString(u.surname),
                    Convert.ToString(u.mail),
                    Convert.ToInt16(u.type),
                    Convert.ToString(u.password),
                    factures
                ));
            }

            return "ok";
        }
        public async Task<string> Post(User u)
        {
            await client.PostRequest(url, u);
            return "ok";
        }
        public async Task<string> Put(User u)
        {
            await client.PutRequest(url + "/" + u.id, u);
            return "ok";
        }
        public async Task<string> Delete(string id)
        {
            await client.DeleteRequest(url + "/" + id);
            return "ok";
        }

        public int GetIntType(string name)
        {
            switch (name)
            {
                case "administrateur":
                    return 0;
                case "employé":
                    return 1;
                case "client":
                    return 2;
                case "prospect":
                    return 3;
                default:
                    return 3;
            }
        }
        public string GetStringType(int id)
        {
            switch (id)
            {
                case 0:
                    return "Administrateur";
                case 1:
                    return "Employé";
                case 2:
                    return "Client";
                case 3:
                    return "Prospect";
                default:
                    return "Prospect";
            }
        }
    }
}
