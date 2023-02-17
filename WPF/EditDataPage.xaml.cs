using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for EditDataPage.xaml
    /// </summary>
    public partial class EditDataPage : Page
    {
        MaterialObject? _materialObject = new();

        public EditDataPage()
        {
            InitializeComponent();
        }

        public EditDataPage(MaterialObject material)
        {
            _materialObject = material;
        }
    }
}
