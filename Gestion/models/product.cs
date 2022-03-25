using System.Collections.Generic;

namespace Gestion
{
    class Product
    {
        #region champs
        public string id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
        public List<Categorie> categories { get; set; }
        #endregion

        #region constructeurs
        public Product(string Id, string Name, double Price, int Quantity, string Description, List<Categorie> Categories)
        {
            id = Id;
            name = Name;
            price = Price;
            quantity = Quantity;
            description = Description;
            categories = Categories;
        }
        #endregion
    }

    class Categorie
    {
        #region
        public string id { get; set; }
        public string name { get; set; }
        #endregion

        #region constructeurs
        public Categorie(string Id, string Name)
        {
            id = Id;
            name = Name;
        }
        #endregion
    }
}
