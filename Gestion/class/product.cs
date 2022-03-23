using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion
{
    class Product
    {
        #region champs
        private string _id;
        private string _name;
        private double _price;
        private int _quantity;
        private string _description;
        private List<categorie> _categories;
        #endregion

        #region constructeurs
        public Product(string id, string n, double p, int q, string d, List<categorie> t)
        {
            _id = id;
            _name = n;
            _price = p;
            _quantity = q;
            _description = d;
            _categories = t;
        }
        #endregion

        #region accesseurs/mutateurs
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public double price
        {
            get { return _price; }
            set { _price = value; }
        }

        public int quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string description
        {
            get { return _description; }
            set { _description = value; }
        }

        public List<categorie> categories
        {
            get { return _categories; }
            set { _categories = value; }
        }
        #endregion
    }

    class categorie
    {
        #region
        private string _id;
        private string _name;
        #endregion

        #region constructeurs
        public categorie(string i, string n)
        {
            _id = i;
            _name = n;
        }
        #endregion

        #region accesseurs/mutateurs
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion
    }
}
