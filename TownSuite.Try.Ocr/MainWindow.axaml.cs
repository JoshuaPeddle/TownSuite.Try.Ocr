using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Tesseract;

namespace TownSuite.Try.Ocr
{
    public partial class MainWindow : Window
    {
        private TextBlock _ocrOutput;
        private TextBox _imagePathTextBox;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _ocrOutput = this.FindControl<TextBlock>("OcrOutput");
            _imagePathTextBox = this.FindControl<TextBox>("ImagePathTextBox");
        }
        
        private async void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filters.Add(new FileDialogFilter
                { Name = "Images", Extensions = { "png", "jpg", "jpeg", "bmp", "pdf" } });

            var selectedFiles = await openFileDialog.ShowAsync(this);

            if (selectedFiles != null && selectedFiles.Length > 0)
            {
                _imagePathTextBox.Text = selectedFiles[0];
            }
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = _imagePathTextBox.Text;

            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                await ProcessImage(imagePath);
            }
        }

        private async Task ProcessImage(string imagePath)
        {
            using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        var ocrText = page.GetText();
                        _ocrOutput.Text = ocrText;
                    }
                }
            }
        }
    }
}