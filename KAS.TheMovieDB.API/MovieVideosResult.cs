using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region MoviesVideosResult
	/// <summary>
	/// The class contains the query result to the list of videos of movies.
	/// </summary>

	public class MovieVideosResult
	{
		[JsonProperty("id")]
		public int ID { get; set; }

		[JsonProperty("results")]
		public MovieVideo[] Results { get; set; }
	}
	#endregion
}

