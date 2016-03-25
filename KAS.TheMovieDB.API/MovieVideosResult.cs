using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region MoviesVideosResult
	public class MovieVideosResult
	{
		[JsonProperty("id")]
		public int ID { get; set; }

		[JsonProperty("results")]
		public MovieVideo[] Results { get; set; }
	}
	#endregion
}

