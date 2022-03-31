using System;
using System.Collections.Generic;

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
        public List<Facture> factures { get; set; }
        #endregion

        #region constructeurs
        public User(string Id, string Name, string Surname, string Mail, int Type, string Password, List<Facture> Factures)
        {
            id = Id;
            name = Name;
            surname = Surname;
            mail = Mail;
            type = Type;
            password = Password;
            factures = Factures;
        }
        #endregion
    }
}
