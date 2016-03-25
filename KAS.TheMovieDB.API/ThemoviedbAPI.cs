using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;


namespace KAS.TheMovieDB.API
{
	#region ThemoviedbAPI
	public class ThemoviedbAPI
	{
		#region Static members
		private static readonly string BASE_URI = "https://api.themoviedb.org";
		private static readonly string VERSION = "/3";

		private static readonly string CONFIGURATION_ENDPOINT = "/configuration";
		private static readonly string NOW_PLAYING_MOVIES_ENDPOINT = "/movie/now_playing?page={0}";
		private static readonly string MOVIE_BY_ID_ENDPOINT = "/movie/{0}";
		private static readonly string SIMILAR_MOVIES_BY_ID_ENDPOINT = "/movie/{0}/similar?page={1}";
		private static readonly string MOVIE_VIDEOS_BY_ID_ENDPOINT = "/movie/{0}/videos";

		private static Configuration __configuration = null;

		public static async Task<Configuration> GetConfiguration(string apiKey)
		{
			if (__configuration == null) {
				var api = new ThemoviedbAPI (apiKey);
				__configuration = await api.GetConfigurationAsync ();
			}
			return __configuration;
		}
		#endregion

		private string _key;

		public ThemoviedbAPI (string key)
		{
			_key = key;
		}


		public async Task<Configuration> GetConfigurationAsync()
		{
			var request = new HttpRequestMessage {
				RequestUri = this.CreateRequestUri (CONFIGURATION_ENDPOINT),
				Method = HttpMethod.Get
			};
			var configuration = await this.ExecuteRequestAsync<Configuration> (request);
			return configuration;
		}

		public async Task<MoviesResult> GetNowPlayingMoviesAsync(int page = 1)
		{
			var request = new HttpRequestMessage {
				RequestUri = this.CreateRequestUri(NOW_PLAYING_MOVIES_ENDPOINT, page),
				Method = HttpMethod.Get
			};

			var moviesResult = await this.ExecuteRequestAsync<MoviesResult>(request);
			return moviesResult;
		}

		public async Task<Movie> GetMoveByID(int id)
		{
			var request = new HttpRequestMessage { 
				RequestUri = this.CreateRequestUri(MOVIE_BY_ID_ENDPOINT, id),
				Method = HttpMethod.Get
			};

			var movie = await this.ExecuteRequestAsync<Movie> (request);
			return movie;
		}

		public async Task<MoviesResult> GetSimilarMoviesByID(int id, int page = 1)
		{
			var request = new HttpRequestMessage {
				RequestUri = this.CreateRequestUri (SIMILAR_MOVIES_BY_ID_ENDPOINT, id, page),
				Method = HttpMethod.Get
			};

			var moviesResult = await this.ExecuteRequestAsync<MoviesResult> (request);
			return moviesResult;
		}

		public async Task<MovieVideosResult> GetMovieVideosByID(int id)
		{
			var request = new HttpRequestMessage { 
				RequestUri = this.CreateRequestUri (MOVIE_VIDEOS_BY_ID_ENDPOINT, id),
				Method = HttpMethod.Get
			};

			var movieVideosResult = await this.ExecuteRequestAsync<MovieVideosResult> (request);
			return movieVideosResult;
		}

		private async Task<T> ExecuteRequestAsync<T>(HttpRequestMessage request)
		{
			T result = default(T);
			using (var httpClient = new HttpClient ()) {
				var response = await httpClient.SendAsync (request);

				var content = await response.Content.ReadAsStringAsync ();

				if (content != null) {
					if (response.IsSuccessStatusCode) 
						result = JsonConvert.DeserializeObject<T> (content);
					else {
						var error = JsonConvert.DeserializeObject<Error> (content);
						throw new ThemoviedbException (error.Message, response.StatusCode, error.Code, null);
					}
				}
				Debug.WriteLine (content);
			}
			return result;
		}

		private Uri CreateRequestUri(string resource, params Object[] args)
		{
//			resource.AssertNotNull("resource");

			StringBuilder builder = new StringBuilder();
			builder.Append(BASE_URI);
			builder.Append(VERSION);

			if (args != null) {
				builder.AppendFormat(resource, args);
			} else {
				builder.Append(resource);
			}

			if (resource.Contains("?")) {
				builder.AppendFormat("&api_key={0}", _key);
			} else {
				builder.AppendFormat("?api_key={0}", _key);
			}

			string url = builder.ToString();
			return new Uri(url);
		}
	}
	#endregion
}

