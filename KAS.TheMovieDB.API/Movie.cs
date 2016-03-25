using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region Movie
	/// <summary>
	/// The class contains information about the movie.
	/// </summary>

	public class Movie
	{
		[JsonProperty("adult")]
		public bool IsAdult { get; set; }

		[JsonProperty("backdrop_path")]
		public string BackdropPath { get; set; }

		[JsonProperty("genre_ids")]
		public int[] GenreIDs { get; set; }

		[JsonProperty("id")]
		public int ID { get; set; }

		[JsonProperty("original_language")]
		public string OriginalLanguage { get; set; }

		[JsonProperty("original_title")]
		public string OriginalTitle { get; set; }

		[JsonProperty("overview")]
		public string Overview { get; set; }

		[JsonProperty("release_date")]
		public DateTime ReleaseDate { get; set; }

		[JsonProperty("poster_path")]
		public string PosterPath { get; set; }

		[JsonProperty("popularity")]
		public double Popularity { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("video")]
		public bool IsVideo { get; set; }

		[JsonProperty("vote_average")]
		public double VoteAvarage { get; set; }

		[JsonProperty("vote_count")]
		public int VoteCount { get; set; }
	}
	#endregion
}

