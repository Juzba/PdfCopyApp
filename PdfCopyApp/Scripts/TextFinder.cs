using System.Text.RegularExpressions;

namespace PdfCopyApp.Scripts
{
    internal class TextFinder
    {
        public static List<Seznam> SeznamsList = new List<Seznam>();
        public static List<int> PositionsList = new List<int>();
        public static List<int> PositionListSTK = new List<int>();
        public static string[] _slovaPDF = [];                          // Pole slov z PDF

        public static void Main(string pdfText)
        {
            SeznamsList.Clear();         // Vyčištění seznamu
            //PositionsList.Clear();   
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

            SeznamGenerator();
        }


        private static void SeznamGenerator()
        {
            for (int i = 0; i < PositionListSTK.Count; i++)
            {
                SeznamsList.Add( new Seznam(i + 1,
                    _slovaPDF[PositionsList[i]],
                    _slovaPDF[PositionListSTK[i] + 6],
                    _slovaPDF[PositionListSTK[i] + 2],
                    _slovaPDF[PositionListSTK[i] + 4],
                    _slovaPDF[PositionListSTK[i] + 7]));
            }
        }


        public static string PrintText()
        {
            string str = "";


            for (int i = 0; i < PositionListSTK.Count; i++)
            {
                if (i != 0) str += "\n";
                str += $"{i + 1}) {_slovaPDF[PositionsList[i]]}    || {_slovaPDF[PositionListSTK[i] + 6]}  {_slovaPDF[PositionListSTK[i] + 2]}   || {_slovaPDF[PositionListSTK[i] + 4]}  {_slovaPDF[PositionListSTK[i] + 7]} ";
                str += "\n";
            }


            return str;
        }













    }
}
