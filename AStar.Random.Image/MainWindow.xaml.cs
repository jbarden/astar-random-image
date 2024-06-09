using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AStar.Random.Image.Data;
using AStar.Web.Domain;
using Microsoft.EntityFrameworkCore;

namespace AStar.Random.Image;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public IEnumerable<string> Files { get; set; } = [];

    public MainWindow()
    {
        InitializeComponent();
        CancellationTokenSource = new();
        DispatcherTimer = new DispatcherTimer();
        DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick!);
        DispatcherTimer.Interval = new TimeSpan(0, 0, 5);
        Application.Current.MainWindow.WindowState = WindowState.Maximized;
    }

    public DispatcherTimer DispatcherTimer { get; }

    private CancellationTokenSource CancellationTokenSource { get; set; }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        CancellationTokenSource = new();
        StartSearch.IsEnabled = false;
        CancelSearch.IsEnabled = true;
        SelectImageForImageSource();
        DispatcherTimer.Start();
    }

    private void SelectImageForImageSource()
    {
        try
        {
            if (!Files.Any())
            {
                Files = Directory.EnumerateFiles(StartingFolder.Text);
            }

            var random = new System.Random(Guid.NewGuid().GetHashCode());
            var imageIndex = random.Next(1, Files.Count());
            var selectedImage = Files.Skip(imageIndex).Take(1).First();
            FileDetail.Content = selectedImage;
            ImageDisplay.Source = new BitmapImage(new Uri(selectedImage));
        }
        catch(Exception ex)
        {
            //Think about how to handle / what message and where
            StartingFolder.Text = ex.GetBaseException().Message;
        }
    }

    private void DispatcherTimer_Tick(object sender, EventArgs e) => SelectImageForImageSource();

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        DispatcherTimer.Stop();
        CancellationTokenSource.Cancel();
        StartSearch.IsEnabled = true;
        CancelSearch.IsEnabled = false;
    }
}
