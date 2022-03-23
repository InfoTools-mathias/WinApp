using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion
{
    class user
    {
        #region champs
        private string _id;
        private string _name;
        private string _surname;
        private string _mail;
        private int _type;
        private string _password;
        #endregion

        #region constructeurs
        public user(string i, string n, string s, string m, int t, string p)
        {
            _id = i;
            _name = n;
            _surname = s;
            _mail = m;
            _type = t;
            _password = p;
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

        public string surname
        {
            get { return _surname; }
            set { _surname = value; }
        }

        public string mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        public int type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string password
        {
            get { return _password; }
            set { _password = value; }
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
