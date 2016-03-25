using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region MoviesResult
	/// <summary>
	/// The class contains the query result to the list of movies.
	/// </summary>

	public class MoviesResult
	{
		/// <summary>
		/// Gets or sets the period of release dates of movies.
		/// </summary>
		/// <value>The period.</value>

		[JsonProperty("dates")]
		public Period Period { get; set; }

		/// <summary>
		/// Gets or sets page number.
		/// </summary>
		/// <value>Page number.</value>

		[JsonProperty("page")]
		public int Page { get; set; }

		/// <summary>
		/// Gets or sets movies list.
		/// </summary>
		/// <value>Array of movies.</value>

		[JsonProperty("results")]
		public Movie[] Results { get; set; }

		/// <summary>
		/// Gets or sets the total pages.
		/// </summary>
		/// <value>The total pages count.</value>

		[JsonProperty("total_pages")]
		public int TotalPages { get; set; }

		/// <summary>
		/// Gets or sets the total movies count.
		/// </summary>
		/// <value>The total movies count.</value>

		[JsonProperty("total_results")]
		public int TotalResults { get; set; }
	}
	#endregion
}

