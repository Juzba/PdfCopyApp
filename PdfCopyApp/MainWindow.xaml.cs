using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Win32;
using PdfCopyApp.Scripts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PdfCopyApp;




public partial class MainWindow : Window
{

    private string _filePath;
    private bool _isFileOpen;
    private int _pageCount = 1;
    private string _HlavniPDF;
    private bool IsMainEnd = false;




    public MainWindow()
    {

        InitializeComponent();

        button_test.Visibility = Visibility.Hidden;  // testovaci tlacitko
    }

    private void Main()
    {

        PdfReader reader = new PdfReader(_filePath);
        PdfDocument pdfDoc = new PdfDocument(reader);
        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
        _pageCount = pdfDoc.GetNumberOfPages();

        for (int i = 1; i < _pageCount; i++)
            _HlavniPDF = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);



        TextFinder.MainTF(_HlavniPDF);



        TextBox_Nadpis.Text = TextFinder.Nadpis;
        TextBoxInfo.Text = "Count H: "
            + TextFinder.PositionsList.Count
            + " STK: "
            + TextFinder.PositionListSTK.Count
            + " Pages: "
            + _pageCount;


        DataGrid1.ItemsSource = null;
        DataGrid1.ItemsSource = TextFinder.SeznamsList;


        IsMainEnd = true;
    }




    // Testovaci okno
    private void Button_Test(object sender, RoutedEventArgs e)
    {
        WindowTest windowTest = new WindowTest();
        windowTest.Show();


        windowTest.TextBoxTesting.Clear();

        for (int i = 0; i < TextFinder._slovaPDF.Count() - 1; i++)
        {
            windowTest.TextBoxTesting.AppendText($"{i}) {TextFinder._slovaPDF[i]}\n");
        }
    }


    // Otevre soubor PDF
    private void Button_OpenFile(object sender, RoutedEventArgs e)
    {
        IsMainEnd = false;


        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";

        if (openFileDialog.ShowDialog() == true)
        {
            _filePath = openFileDialog.FileName;
            TextBoxFilePath.Text = _filePath;
            _isFileOpen = true;
        }



        TextFinder.PositionsList.Clear();
        TextFinder.PositionListSTK.Clear();
        TextFinder.SeznamsList.Clear();
        if (_isFileOpen) Main();
    }



    private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {

        if (IsMainEnd && DataGrid1.SelectedCells.Count > 0)
        {
            var selectedCell = DataGrid1.SelectedCells[0];


            if (selectedCell.Column.GetCellContent(selectedCell.Item) != null)
            {
                var cellContent = (TextBlock)selectedCell.Column.GetCellContent(selectedCell.Item);

                string? cellValue = cellContent?.Text;
                TextBoxInfo.Clear();
                TextBoxInfo.Text = $"Copy: {cellValue}";

                Clipboard.SetText(cellValue);  // Copiruje item do pameti pc
            }
        }
    }



    private void Button_Top_click(object sender, RoutedEventArgs e)
    {


        if (this.Topmost)
        {
            this.Topmost = false;
            Button_Top.Background = new SolidColorBrush(Colors.LightBlue);
            Button_Top.Content = "Unlocked";
        }

        else
        {
            this.Topmost = true;
            Button_Top.Background = new SolidColorBrush(Colors.OrangeRed);
            Button_Top.Content = "Locked";
        }
    }

 
}