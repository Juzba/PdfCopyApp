namespace PdfCopyApp.Scripts
{
    internal class Seznam
    {
        public int Number { get; set; }
        public string Id { get; set; }
        public string Menge { get; set; }
        public string Termin { get; set; }
        public string EPrice { get; set; }
        public string Part { get; set; }

        public Seznam(int number, string id, string menge, string date, string preis, string part)
        {
            this.Number = number;
            this.Id = id;
            this.Menge = menge;
            this.Termin = date;
            this.EPrice = preis;
            this.Part = part;
        }
    }
}
