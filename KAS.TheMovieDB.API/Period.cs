using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region Period
	public class Period
	{
		[JsonProperty("minimum")]
		public DateTime Minimum { get; set; }

		[JsonProperty("maximum")]
		public DateTime Maximum { get; set; }
	}
	#endregion
}

