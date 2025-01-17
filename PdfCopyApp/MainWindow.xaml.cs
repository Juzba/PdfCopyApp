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
    private int _pageIndex = 0;
    private string _HlavniPDF;
    private bool IsMainEnd = false;


    public MainWindow()
    {
        InitializeComponent();
    }

    private void Main()
    {

        PdfReader reader = new PdfReader(_filePath);
        PdfDocument pdfDoc = new PdfDocument(reader);
        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
        _pageCount = pdfDoc.GetNumberOfPages();

        for (int i = 1; i < _pageCount; i++)
        {
            _HlavniPDF = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);
        }



        TextFinder.Main(_HlavniPDF);




        TextBoxInfo.Text = "Count H: "
            + TextFinder.PositionsList.Count
            + " STK: "
            + TextFinder.PositionListSTK.Count
            + " Pages: "
            + _pageCount;


        DataGrid1.ItemsSource = TextFinder.SeznamsList;

    }





    private void Button_Start(object sender, RoutedEventArgs e)
    {
        //TextFinder.PositionsList.Clear();
        //TextFinder.PositionListSTK.Clear();
        //TextFinder.SeznamsList.Clear();
        //if (_isFileOpen) Main();
    }



    private void Button_OpenFile(object sender, RoutedEventArgs e)
    {
        IsMainEnd = false ;

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



    private int c;
    private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (IsMainEnd)
        {

            int rowIndex = DataGrid1.SelectedIndex;
            var row = (DataGridRow)DataGrid1.ItemContainerGenerator.ContainerFromIndex(rowIndex);

            int columnIndex = DataGrid1.CurrentColumn.DisplayIndex;
            var cell = DataGrid1.Columns[columnIndex].GetCellContent(row).Parent as DataGridCell;

            string cellValue = (cell.Content as TextBlock).Text;



            TextBoxInfo.Clear();
            TextBoxInfo.Text = $"X: {rowIndex}  Y: {columnIndex} \nCopy: {cellValue}";

            Clipboard.SetText(cellValue);

        }
    }



    private void Button_Top_click(object sender, RoutedEventArgs e)
    {


        if (this.Topmost)
        {
            this.Topmost = false;
            Button_Top.Background = new SolidColorBrush(Colors.AliceBlue);
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