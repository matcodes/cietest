using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region Configuration
	/// <summary>
	/// This class currently holds the data relevant to building image URLs as well as the change key map.
	/// </summary>
	public class Configuration
	{
		[JsonProperty("images")]
		public Images Images { get; set; }

		[JsonProperty("change_keys")]
		public string[] ChangeKeys { get; set; }
	}
	#endregion
}

