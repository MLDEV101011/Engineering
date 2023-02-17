﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public DetailsWindow(MaterialObject materialObject)
        {
            _materialObject = materialObject;
            InitializeComponent();
            ContentFrame.Content = new ViewDataPage(_materialObject);
            DetailsBorder = ViewDataPage.border;
            CancelBorder.Visibility = Visibility.Hidden;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new EditDataPage(_materialObject);
            CancelBorder.Visibility = Visibility.Visible;
            PrintBorder.Visibility = Visibility.Hidden;
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new();
            Border frame;

            if(printDlg.ShowDialog() == true)
            {
                frame = DetailsBorder;
                frame.BorderThickness = new Thickness(0);
                Size pageSize = new(printDlg.PrintableAreaWidth - 30, printDlg.PrintableAreaHeight - 30);
                frame.Measure(pageSize);
                frame.Arrange(new Rect(15,15,pageSize.Width,pageSize.Height));
                printDlg.PrintVisual(frame, "Details");
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            
            ContentFrame.Content = new ViewDataPage(_materialObject);
            CancelBorder.Visibility = Visibility.Hidden;
            PrintBorder.Visibility = Visibility.Visible;
        }
    }
}
