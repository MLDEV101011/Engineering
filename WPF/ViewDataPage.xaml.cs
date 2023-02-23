using Microsoft.EntityFrameworkCore;
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
            _materialObject = _context.Materials.Include(x => x.Images).Include(x => x.Notes).Include(x => x.ApprovedMatches).FirstOrDefault(v => v.Id.Equals(materialObject.Id)) as MaterialObject;
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
                notes.Clear();
                foreach (var note in _materialObject.Notes.Distinct())
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
                approvedMatches.Clear();
                foreach (var item in _materialObject.ApprovedMatches.Distinct())
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
                images.Clear();
                foreach (var item in _materialObject.Images.Distinct())
                {
                    images.Add(item?.FilePath);
                }
            }
            SetImages();
        }

        void SetImages()
        {
            foreach (var img in _materialObject.Images)
            {
                if(img.Name == @"(MATCH POINT DETAIL VIEW)")
                {
                    MatchPointImage.ImageSource = new BitmapImage(new Uri(img.FilePath, UriKind.Relative));
                }
                if(img.Name == @"(V2 MATCH POINT DETAIL VIEW)")
                {
                    V2Image.ImageSource = new BitmapImage(new Uri(img.FilePath, UriKind.Relative));
                }                
                if(img.Name == @"(H2 MATCH POINT DETAIL VIEW)")
                {
                    H2Image.ImageSource = new BitmapImage(new Uri(img.FilePath, UriKind.Relative));
                }
                if(img.Name == @"(V2H2 MATCH POINT DETAIL VIEW)")
                {
                    V2H2Image.ImageSource = new BitmapImage(new Uri(img.FilePath, UriKind.Relative));
                }
                if(img.Name == @"(FULL REPEAT/MATCH POINT DETAIL VIEW)")
                {
                    FullRepeatImage.ImageSource = new BitmapImage(new Uri(img.FilePath, UriKind.Relative));
                }
            }            
        }

        
        void AllBlankImages()
        {
            MatchPointImage.ImageSource = noImageImg;
            V2Image.ImageSource = noImageImg;
            H2Image.ImageSource = noImageImg;
            V2H2Image.ImageSource = noImageImg;
            FullRepeatImage.ImageSource = noImageImg;
        }

        void CheckForBlankImage(ImageSource img)
        {
            if(img.ToString() == "pack://application:,,,/Verseteel%20Material%20Data%20Log;component/viewdatapage.xaml")
            {
                return;
            }
            else
            {
                ImageViewWindow imageViewWindow = new(img);
                imageViewWindow.ShowDialog();
            }
        }

        private void MatchPointGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CheckForBlankImage(MatchPointImage.ImageSource);
        }

        private void V2Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CheckForBlankImage(V2Image.ImageSource);

        }

        private void H2Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckForBlankImage(H2Image.ImageSource);
        }

        private void V2H2Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckForBlankImage(V2H2Image.ImageSource);
        }

        private void FullRepeatGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckForBlankImage(FullRepeatImage.ImageSource);
        }
    }
}
