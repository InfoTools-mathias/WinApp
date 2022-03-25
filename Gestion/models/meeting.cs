using System;
using System.Collections.Generic;

namespace Gestion
{
    class Meeting
    {
        #region champs
        public string id { get; set; }
        public DateTime date { get; set; }
        public string zip { get; set; }
        public string adress { get; set; }
        public List<User> users { get; set; }
        #endregion

        #region constructeurs
        public Meeting(string Id, DateTime Date, string Zip, string Adress, List<User> Users)
        {
            id = Id;
            date = Date;
            zip = Zip;
            adress = Adress;
            users = Users;
        }
        #endregion
    }
}
