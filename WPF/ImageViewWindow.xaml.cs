using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF
{
    /// <summary>
    /// Interaction logic for ImageViewWindow.xaml
    /// </summary>
    public partial class ImageViewWindow : Window
    {

        BitmapImage _image;

        public ImageViewWindow()
        {
            InitializeComponent();
        }

        public ImageViewWindow(BitmapImage image)
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
