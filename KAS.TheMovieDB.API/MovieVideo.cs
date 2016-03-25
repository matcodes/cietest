using System;
using Newtonsoft.Json;

namespace KAS.TheMovieDB.API
{
	#region MovieVideo
	public class MovieVideo
	{
		[JsonProperty("id")]
		public string ID { get; set; }

		[JsonProperty("iso_639_1")]
		public string Language { get; set; }

		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("site")]
		public string Site { get; set; }

		[JsonProperty("size")]
		public int Size { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
	#endregion
}

