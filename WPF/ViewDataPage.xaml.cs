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
using WPF.Data;

namespace WPF
{
    /// <summary>
    /// Interaction logic for ViewDataPage.xaml
    /// </summary>
    public partial class ViewDataPage : Page
    {
        readonly DataContext _context = new();
        MaterialObject? _materialObject = new();
        List<string> approvedMatches = new();
        List<string> notes = new();
        List<string> images = new();

        public static Border border;

        BitmapImage noImageImg = new(new Uri("no_image.png", UriKind.Relative));
        public ViewDataPage(MaterialObject materialObject)
        {
            _materialObject = materialObject;
            InitializeComponent();
            border = DetailsView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TitleLable.Content = "Details for " + _materialObject?.Vendor;
            VendorTextbox.Text = _materialObject?.Vendor;
            PatternTextbox.Text = _materialObject?.Pattern;
            ColorTextbox.Text = _materialObject?.Color;
            ProdNumTextbox.Text = _materialObject?.ProductNum;
            VertRepeatTextbox.Text = _materialObject?.VertRepeat.ToString();
            HorzRepeatTextbox.Text = _materialObject?.HorzRepeat.ToString();
            RailroadedTextbox.Text = _materialObject?.Railroaded;
            DurabiltyTextbox.Text = _materialObject?.Durability.ToString();
            WidthTextbox.Text = _materialObject?.Width.ToString();
            WeightTextbox.Text = _materialObject?.Weight.ToString();
            RollWidthTextbox.Text = _materialObject?.RollWidth.ToString();
            UsableWidthTextbox.Text = _materialObject?.UsableWidth.ToString();
            StretchAcrossTextbox.Text = _materialObject?.StretchAcrossWidth.ToString();
            StretchDownTextbox.Text = _materialObject?.StretchDownLength.ToString();
            RepeatAcrossTextbox.Text = _materialObject?.RepeatAcrossWidth.ToString();
            SalvageToMatchTextbox.Text = _materialObject?.SalvageToMatchOffset.ToString();
            RepeatDownTextbox.Text = _materialObject?.RepeatDownLength.ToString();
            EndToMatchTextbox.Text = _materialObject?.EndToMatchOffset.ToString();

            // Handle notes list for material object
            if (_materialObject?.Notes?.Count != null)
            {
                foreach (var note in _materialObject.Notes)
                {
                    notes.Add(note.Content + " - " + note.CreatedDate);
                }
                NotesTextbox.Text = string.Join(" // ", notes);
            }
            else
            {
                NotesTextbox.Text = string.Empty;
            }

            // Handle approved matches for material object
            if (_materialObject?.ApprovedMatches?.Count != null)
            {
                foreach (var item in _materialObject.ApprovedMatches)
                {
                    approvedMatches.Add(item?.MatchType);
                }
                ApprovedMatchingTextbox.Text = string.Join(", ", approvedMatches);
            }
            else
            {
                ApprovedMatchingTextbox.Text = string.Empty;
            }

            // Handle Images for material object
            if (_materialObject?.Images?.Count > 0)
            {
                foreach (var item in _materialObject.Images)
                {
                    images.Add(item?.FilePath);
                }
            }
            SetImages();
        }

        void SetImages()
        {
            int imageCount = images.Count;

            if (imageCount == 1)
            {
                GetImage(MatchPointImage, images[0]);
                OneBlankImage(H2Image);
                OneBlankImage(V2Image);
                OneBlankImage(V2H2Image);
                OneBlankImage(FullRepeatImage);
            }
            else if (imageCount == 2)
            {
                GetImage(MatchPointImage, images[0]);
                GetImage(V2Image, images[1]);
                OneBlankImage(V2Image);
                OneBlankImage(V2H2Image);
                OneBlankImage(FullRepeatImage);
            }
            else if (imageCount == 3)
            {
                GetImage(MatchPointImage, images[0]);
                GetImage(V2Image, images[1]);
                GetImage(H2Image, images[2]);
                OneBlankImage(V2H2Image);
                OneBlankImage(FullRepeatImage);
            }
            else if (imageCount == 4)
            {
                GetImage(MatchPointImage, images[0]);
                GetImage(V2Image, images[1]);
                GetImage(H2Image, images[2]);
                GetImage(V2H2Image, images[3]);
                OneBlankImage(FullRepeatImage);
            }
            else if (imageCount == 5)
            {
                GetImage(MatchPointImage, images[0]);
                GetImage(V2Image, images[1]);
                GetImage(H2Image, images[2]);
                GetImage(V2H2Image, images[3]);
                GetImage(FullRepeatImage, images[4]);
            }
            else
            {
                AllBlankImages();
            }


        }
        void GetImage(ImageBrush img, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                img.ImageSource = noImageImg;
            }
            else
            {
                img.ImageSource = new BitmapImage(new Uri(path));
            }
        }

        void OneBlankImage(ImageBrush img)
        {
            img.ImageSource = noImageImg;
        }

        void AllBlankImages()
        {
            MatchPointImage.ImageSource = noImageImg;
            V2Image.ImageSource = noImageImg;
            H2Image.ImageSource = noImageImg;
            V2H2Image.ImageSource = noImageImg;
            FullRepeatImage.ImageSource = noImageImg;
        }

        private void MatchPointGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImageViewWindow imageViewWindow = new(new BitmapImage(new Uri(images[0])));
            imageViewWindow.Show();
        }

        private void V2Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImageViewWindow imageViewWindow = new(new BitmapImage(new Uri(images[1])));
            imageViewWindow.Show();
        }

        private void H2Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageViewWindow imageViewWindow = new(new BitmapImage(new Uri(images[2])));
            imageViewWindow.Show();
        }

        private void V2H2Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageViewWindow imageViewWindow = new(new BitmapImage(new Uri(images[3])));
            imageViewWindow.Show();
        }

        private void FullRepeatGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageViewWindow imageViewWindow = new(new BitmapImage(new Uri(images[4])));
            imageViewWindow.Show();
        }
    }
}
