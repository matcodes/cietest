using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region Configuration
	public class Configuration
	{
		[JsonProperty("images")]
		public Images Images { get; set; }

		[JsonProperty("change_keys")]
		public string[] ChangeKeys { get; set; }
	}
	#endregion
}

