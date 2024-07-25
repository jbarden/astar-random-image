using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AStar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AStar.Random.Image;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string selectedImage;

    public IEnumerable<string> Files { get; set; } = [];

    public MainWindow()
    {
        try
        {
            InitializeComponent();
            CancellationTokenSource = new();
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick!);
            DispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            selectedImage = string.Empty;
            var appSettings = File.ReadAllText(@"appsettings.json");
            FileDetail.Content = appSettings;
            var appSettings1 = System.Text.Json.JsonSerializer.Deserialize<AStar.Random.Image.Config.AppSettings>(appSettings, (JsonSerializerOptions)new(JsonSerializerDefaults.Web));
            FileDetail.Content = string.IsNullOrWhiteSpace(appSettings1.ConnectionStrings.SqlServer) ? "Opps" : appSettings1.ConnectionStrings.SqlServer;
            var contextOptions = new DbContextOptionsBuilder<FilesContext>()
                .UseSqlServer(appSettings1.ConnectionStrings.SqlServer)
                .Options;
            var filesContext = new FilesContext(new (){Value=appSettings1.ConnectionStrings.SqlServer}, new());
            Files = filesContext.Files.Include(f=>f.FileAccessDetail)
                                    .Where(f=> !f.FileAccessDetail.SoftDeleted && !f.FileAccessDetail.HardDeletePending && !f.FileAccessDetail.SoftDeletePending)
                                    .Select(f => Path.Combine(f.DirectoryName, f.FileName));   
        }
        catch (System.Exception ex)
        {
            File.AppendAllText(@"c:\logs\astar-random.image.log.txt", $"UTC: {DateTime.UtcNow} - {ex.Message} (MainWindow){Environment.NewLine}");
        }
    }

    public DispatcherTimer DispatcherTimer { get; }

    private CancellationTokenSource CancellationTokenSource { get; set; }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CancellationTokenSource = new();
            StartSearch.IsEnabled = false;
            CancelSearch.IsEnabled = true;
            SelectImageForImageSource();
            DispatcherTimer.Start();   
        }
        catch (System.Exception ex)
        {
            File.AppendAllText(@"c:\logs\astar-random.image.log.txt", $"UTC: {DateTime.UtcNow} - {ex.Message} (Button_Click){Environment.NewLine}");
        }
    }

    private void SelectImageForImageSource()
    {
        try
        {
            FileDetail.Content = $"Starting search at {DateTime.Now}...";
            if (!Files.Any())
            {
                // Files = Directory.EnumerateFiles(StartingFolder.Text, "*.*", SearchOption.AllDirectories);
            }

            FileDetail.Content = $"Got files at {DateTime.Now}...";
            var filesToDelete = File.ReadAllLines(@"c:\temp\delete.json");
            var files = Files.Where(file => file.StartsWith(StartingFolder.Text)).ToList().Except(filesToDelete);
            File.AppendAllText(@"c:\temp\fileCount.txt", $"UTC: {DateTime.UtcNow}{Environment.NewLine}File Count after selection: {files.Count()}{Environment.NewLine}");

            var random = new System.Random(Guid.NewGuid().GetHashCode());
            var imageIndex = random.Next(1, files.Count());
            selectedImage = files.Skip(imageIndex).Take(1).First();
            FileDetail.Content = $"Got {selectedImage} at {DateTime.Now}...";
            ImageDisplay.Source = new BitmapImage(new Uri(selectedImage));
        }
        catch (Exception ex)
        {
            //Think about how to handle / what message and where
            FileDetail.Content = ex.GetBaseException().Message;
            AddToDeleteFile();
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

    private void Button_Click_2(object sender, RoutedEventArgs e) => AddToDeleteFile();

    private void AddToDeleteFile()
    {
        Files = Files.Where(file => file != selectedImage);
        File.AppendAllText(@"c:\temp\delete.json", $"UTC: {DateTime.UtcNow}{Environment.NewLine}{selectedImage}{Environment.NewLine}");
        File.AppendAllText(@"c:\temp\fileCount.txt", $"UTC: {DateTime.UtcNow}{Environment.NewLine}{Files.Count()}{Environment.NewLine}");
        selectedImage = string.Empty;
        SelectImageForImageSource();
    }
}
