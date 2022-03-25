using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion
{
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
