using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion
{
    class User
    {
        #region champs
        public string id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }    
        public string mail { get; set; }
        public int type { get; set; }
        public string password { get; set; }
        #endregion

        #region constructeurs
        public User(string i, string n, string s, string m, int t, string p)
        {
            id = i;
            name = n;
            surname = s;
            mail = m;
            type = t;
            password = p;
        }
        #endregion
    }

    class auth
    {
        #region champs
        private string _mail;
        private string _password;
        #endregion

        #region constructeurs
        public auth(string m, string p)
        {
            _mail = m;
            _password = p;
        }
        #endregion

        #region accesseurs/mutateurs
        public string mail
        {
            get { return _mail; }
            set { _mail = value; }
        }
        public string password
        {
            get { return _password; }
            set { _password = value; }
        }
        #endregion
    }
}
