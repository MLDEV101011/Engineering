using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF
{
    /// <summary>
    /// Interaction logic for ImageViewWindow.xaml
    /// </summary>
    public partial class ImageViewWindow : Window
    {

        ImageSource _image;

        public ImageViewWindow()
        {
            InitializeComponent();
        }

        public ImageViewWindow(ImageSource image)
        {
            this._image = image;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImagePreview.ImageSource = _image;
        }
    }
}
