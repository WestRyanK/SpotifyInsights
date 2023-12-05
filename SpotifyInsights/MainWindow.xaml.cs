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
			ViewModel.PlayCounts = SpotifyAnalyzer.CountSpotifyPlays(ViewModel.Plays);
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
					if (ViewModel.Plays == null)
					{
						ViewModel.Plays = [];
					}

					var newData = await SpotifyAnalyzer.LoadFromJsonAsync(jsonPath);
					if (newData == null)
					{
						string errorMessage = $"Error loading '{jsonPath}'";
						Debug.WriteLine(errorMessage);
						MessageBox.Show(errorMessage);
						return;
					}

					ViewModel.Plays = ViewModel.Plays.Concat(newData);
					AnalyzeData();
				}
			}
			catch (Exception ex) {
				string errorMessage = $"Error: {ex}";
				Debug.WriteLine(errorMessage);
				MessageBox.Show(errorMessage);
			}
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