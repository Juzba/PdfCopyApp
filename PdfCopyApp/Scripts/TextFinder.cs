using System.Text.RegularExpressions;

namespace PdfCopyApp.Scripts
{
    internal class TextFinder
    {
        public static List<Seznam> SeznamsList = new List<Seznam>();
        public static List<int> PositionsList = new List<int>();
        public static List<int> PositionListSTK = new List<int>();
        private static List<string> PartList = new List<string>();
        public static string[] _slovaPDF = [];                          // Pole slov z PDF

        public static void Main(string pdfText)
        {
            SeznamsList.Clear();         // Vyčištění seznamu
            PartList.Clear();
            _slovaPDF = [];              // Vyčištění pole slov z PDF
            string pattern = @"^[A-Za-z]{1}\d{7,}";



            _slovaPDF = Regex.Split(pdfText, @"[^\w/,\.]+");       // Rozdělení textu na slova


            int count = 0;
            for (int i = 0; i < _slovaPDF.Length; i++)
            {
                if (Regex.IsMatch(_slovaPDF[i], pattern)) PositionsList.Add(i);
                if (_slovaPDF[i] == "STK" && count != 0) PositionListSTK.Add(i);
                else if (_slovaPDF[i] == "STK") count++;
            }



            foreach (int i in PositionListSTK)
            {
                string s = "";
                bool IsIndexFound = false;
                for (int j = 0; j < 5; j++)
                {
                    if (_slovaPDF[i + j + 7] == "Index" || _slovaPDF[i + j + 7] == "Behandlung") { IsIndexFound = true; break; }
                    else s += " " + _slovaPDF[i + j + 7];
                }
                if( IsIndexFound ) {PartList.Add(s); }
                else {PartList.Add(" Chyba"); }
            }

            SeznamGenerator();
        }


        private static void SeznamGenerator()
        {
            for (int i = 0; i < PositionListSTK.Count; i++)
            {
                SeznamsList.Add(new Seznam(i + 1,
                    _slovaPDF[PositionsList[i]],
                    _slovaPDF[PositionListSTK[i] + 6],
                    _slovaPDF[PositionListSTK[i] + 2],
                    _slovaPDF[PositionListSTK[i] + 4],
                    PartList[i]));
            }
        }
    }
}
