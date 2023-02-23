using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.Data;
using WPF.Models;

namespace WPF
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {

        MaterialObject? _materialObject = new();
        FlowDocument _flowDoc = new();
        StackPanel _panel = new();

        public DetailsWindow(MaterialObject materialObject)
        {
            _materialObject = materialObject;
            InitializeComponent();
            ContentFrame.Content = new ViewDataPage(_materialObject);
            _flowDoc = ViewDataPage.flowDoc;
            _panel = ViewDataPage.stackPanel;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new EditDataPage(_materialObject);
            PrintBorder.Visibility = Visibility.Collapsed;
            EditBorder.Visibility = Visibility.Collapsed;
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new();
            FlowDocument fd = new();

            

            if(printDlg.ShowDialog() == true)
            {
                fd = _flowDoc;
                _panel.Visibility = Visibility.Collapsed;
                IDocumentPaginatorSource source = fd;
              
                fd.PageWidth = printDlg.PrintableAreaWidth;
                fd.PageHeight = printDlg.PrintableAreaHeight;
                fd.ColumnWidth = printDlg.PrintableAreaWidth;
                fd.PagePadding = new Thickness(30, 50, 20, 30);

                printDlg.PrintDocument(source.DocumentPaginator, "Details");
            }
            ContentFrame.Content = new ViewDataPage(_materialObject);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            
            ContentFrame.Content = new ViewDataPage(_materialObject);            
        }

        
    }
}
