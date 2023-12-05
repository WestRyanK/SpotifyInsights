using System.Diagnostics;
using System.Windows;

namespace SpotifyInsights
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainViewModel ViewModel
		{
			get => (MainViewModel)base.DataContext;
			set => base.DataContext = value;
		}

		public MainWindow()
		{
			InitializeComponent();

			ViewModel = new();
		}

		private void AnalyzeData()
		{
			ViewModel.PlayCounts = SpotifyAnalyzer.CountSpotifyPlays(ViewModel.Plays, new DateTime(2023, 1, 1), new DateTime(2024, 1, 1));
		}

		private async void BrowseButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Microsoft.Win32.OpenFileDialog dialog = new()
				{
					Filter = "JSON|*.json",
					AddExtension = true,
					Multiselect = true,
				};

				if (dialog.ShowDialog() == true)
				{
					ViewModel.Plays = await LoadData(dialog.FileNames);
					AnalyzeData();
				}
			}
			catch (Exception ex)
			{
				string errorMessage = $"Error: {ex}";
				Debug.WriteLine(errorMessage);
				MessageBox.Show(errorMessage);
			}
		}

		private async Task<IEnumerable<SpotifyPlay>> LoadData(string[] jsonPaths)
		{
			IEnumerable<SpotifyPlay> plays = [];
			foreach (var jsonPath in jsonPaths)
			{
				var newData = await SpotifyAnalyzer.LoadFromJsonAsync(jsonPath);
				if (newData == null)
				{
					string errorMessage = $"Error loading '{jsonPath}'";
					Debug.WriteLine(errorMessage);
					MessageBox.Show(errorMessage);
					break;
				}

				plays = plays.Concat(newData);
			}
			return plays;
		}
	}

	internal class MainViewModel : ViewModelBase
	{
		private IEnumerable<SpotifyPlayCount> _playCounts = [];
		public IEnumerable<SpotifyPlayCount> PlayCounts
		{
			get => _playCounts;
			set => SetProperty(ref _playCounts, value);
		}

		private IEnumerable<SpotifyPlay> _plays = [];
		public IEnumerable<SpotifyPlay> Plays
		{
			get => _plays;
			set => SetProperty(ref _plays, value);
		}

	}
}