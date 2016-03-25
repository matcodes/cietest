using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region Error
	public class Error
	{
		[JsonProperty("status_code")]
		public long Code { get; set; }

		[JsonProperty("status_message")]
		public string Message { get; set; }
	}
	#endregion
}

