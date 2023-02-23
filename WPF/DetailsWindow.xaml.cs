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
        Border DetailsBorder = new();
        ScrollViewer scroll = new();

        public DetailsWindow(MaterialObject materialObject)
        {
            _materialObject = materialObject;
            InitializeComponent();
            ContentFrame.Content = new ViewDataPage(_materialObject);
            DetailsBorder = ViewDataPage.border;
            scroll = ViewDataPage.scrollViewer;
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
            ScrollViewer sv;

            if(printDlg.ShowDialog() == true)
            {
                sv = scroll;
                PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / sv.ActualWidth, capabilities.PageImageableArea.ExtentHeight / sv.ActualHeight);
                sv.LayoutTransform = new ScaleTransform(scale, scale);
                Size size = new(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                sv.Measure(size);
                sv.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), size));
                printDlg.PrintVisual(sv, "Details");
            }
            ContentFrame.Content = new ViewDataPage(_materialObject);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            
            ContentFrame.Content = new ViewDataPage(_materialObject);            
        }

        
    }
}
