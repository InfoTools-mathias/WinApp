namespace Gestion
{
    class Categorie
    {
        #region champs
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
