using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotifyInsights
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		internal IEnumerable<SpotifyPlay> Plays { get; private set; }

		public MainWindow()
		{
			InitializeComponent();
		}

		private async void BrowseButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Microsoft.Win32.OpenFileDialog dialog = new()
				{
					Filter = "JSON|*.json",
					AddExtension = true
				};

				if (dialog.ShowDialog() == true)
				{
					string jsonPath = dialog.FileName;
					if (Plays == null)
					{
						Plays = [];
					}

					var newData = await SpotifyAnalyzer.LoadFromJsonAsync(jsonPath);
					if (newData == null)
					{
						string errorMessage = $"Error loading '{jsonPath}'";
						Debug.WriteLine(errorMessage);
						MessageBox.Show(errorMessage);
						return;
					}

					Plays = Plays.Concat(newData);
				}
			}
			catch (Exception ex) {
				string errorMessage = $"Error: {ex}";
				Debug.WriteLine(errorMessage);
				MessageBox.Show(errorMessage);
			}
		}
	}
}