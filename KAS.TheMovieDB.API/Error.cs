using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region Error
	/// <summary>
	/// The class describes an error in the request message data.
	/// </summary>

	public class Error
	{
		[JsonProperty("status_code")]
		public long Code { get; set; }

		[JsonProperty("status_message")]
		public string Message { get; set; }
	}
	#endregion
}

