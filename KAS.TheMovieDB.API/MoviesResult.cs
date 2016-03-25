using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region MoviesResult
	public class MoviesResult
	{
		[JsonProperty("dates")]
		public Period Period { get; set; }

		[JsonProperty("page")]
		public int Page { get; set; }

		[JsonProperty("results")]
		public Movie[] Results { get; set; }

		[JsonProperty("total_pages")]
		public int TotalPages { get; set; }

		[JsonProperty("total_results")]
		public int TotalResults { get; set; }
	}
	#endregion
}

