using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion
{
    class Meeting
    {
        #region champs
        private string _id;
        private DateTime _date;
        private string _zip;
        private string _adress;
        private List<User> _users;
        #endregion

        #region constructeurs
        public Meeting(string id, DateTime date, string zip, string adress, List<User> users)
        {
            _id = id;
            _date = date;
            _zip = zip;
            _adress = adress;
            _users = users;
        }
        #endregion

        #region accesseurs/mutateurs
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        public DateTime date
        {
            get { return _date; }
            set { _date = value; }
        }
        public string zip
        {
            get { return _zip; }
            set { _zip = value; }
        }
        public string adress
        {
            get { return _adress; }
            set { _adress = value; }
        }
        public List<User> users
        {
            get { return _users; }
            set { _users = value; }
        }
        #endregion
    }
}
