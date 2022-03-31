using System;
using System.Collections.Generic;

namespace Gestion
{
    class Facture
    {
        #region champs
        public string id { get; set; }
        public DateTime date { get; set; }
        public List<LigneFacture> lignes { get; set; }
        public string clientId { get; set; }
        #endregion

        #region constructeurs
        public Facture(string Id, DateTime Date, List<LigneFacture> Lignes, string ClientId)
        {
            id = Id;
            date = Date;
            lignes = Lignes;
            clientId = ClientId;
        }
        #endregion
    }

    class LigneFacture
    {
        #region champs
        public string id { get; set; }
        public string product { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public string factureId { get; set; }
        #endregion

        #region constructeurs
        public LigneFacture(string Id, string Product, int Quantity, double Price, string FactureId)
        {
            id = Id;
            product = Product;
            quantity = Quantity;
            price = Price;
            factureId = FactureId;
        }
        #endregion
    }
}
