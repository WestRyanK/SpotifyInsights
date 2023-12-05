using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SpotifyInsights
{
	internal record struct SpotifyPlay
	{
		public string EndTime { get; set; }
		public string ArtistName { get; set; }
		public string TrackName { get; set; }
		public long MsPlayed { get; set; }
	}

	internal class SpotifyAnalyzer
	{
		public static async Task<List<SpotifyPlay>?> LoadFromJsonAsync(string path)
		{
			using FileStream stream = File.OpenRead(path);
			return await JsonSerializer.DeserializeAsync<List<SpotifyPlay>>(stream, new JsonSerializerOptions()
			{
				 PropertyNameCaseInsensitive = true,
			});
		}
	}
}
