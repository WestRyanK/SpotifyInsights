﻿using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SpotifyInsights;

internal record struct SpotifyPlay
{
	[JsonConverter(typeof(DateTimeJsonConverter))]
	public DateTime EndTime { get; set; }
	public string ArtistName { get; set; }
	public string TrackName { get; set; }
	public long MsPlayed { get; set; }
}

internal record struct SpotifyPlayCount
{
	public string ArtistName { get; set; }
	public string TrackName { get; set; }
	public int Plays { get; set; }
	public int Rank { get; set; }
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

	public static IEnumerable<SpotifyPlayCount> CountSpotifyPlays(IEnumerable<SpotifyPlay> plays, DateTime? rangeStart, DateTime? rangeEnd) =>
		plays
		.Where(p => (!rangeStart.HasValue || p.EndTime > rangeStart.Value) && (!rangeEnd.HasValue || p.EndTime < rangeEnd.Value))
			.GroupBy(p => (p.ArtistName, p.TrackName))
			.Select(t => new SpotifyPlayCount
			{
				ArtistName = t.First().ArtistName,
				TrackName = t.First().TrackName,
				Plays = t.Count()
			})
			.OrderByDescending(p => p.Plays)
			.Select((p, i) =>
			{
				p.Rank = i + 1;
				return p;
			});
}
