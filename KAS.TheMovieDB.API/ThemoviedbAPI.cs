using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;


namespace KAS.TheMovieDB.API
{
	#region ThemoviedbAPI
	/// <summary>
	/// The class implements methods to query TheMovieDB API.
	/// </summary>

	public class TheMovieDBAPI
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

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns>The configuration.</returns>
		/// <param name="apiKey">API key.</param>

		public static async Task<Configuration> GetConfiguration(string apiKey)
		{
			if (__configuration == null) {
				var api = new TheMovieDBAPI (apiKey);
				__configuration = await api.GetConfigurationAsync ();
			}
			return __configuration;
		}
		#endregion

		private string _key;

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.API.ThemoviedbAPI"/> class.
		/// </summary>
		/// <param name="key">API key.</param>

		public TheMovieDBAPI (string key)
		{
			_key = key;
		}

		/// <summary>
		/// Gets the configuration async.
		/// </summary>
		/// <returns>The configuration async.</returns>

		public async Task<Configuration> GetConfigurationAsync()
		{
			var request = new HttpRequestMessage {
				RequestUri = this.CreateRequestUri (CONFIGURATION_ENDPOINT),
				Method = HttpMethod.Get
			};
			var configuration = await this.ExecuteRequestAsync<Configuration> (request);
			return configuration;
		}

		/// <summary>
		/// Gets the now playing movies async.
		/// </summary>
		/// <returns>The now playing movies async.</returns>
		/// <param name="page">Page number.</param>

		public async Task<MoviesResult> GetNowPlayingMoviesAsync(int page = 1)
		{
			var request = new HttpRequestMessage {
				RequestUri = this.CreateRequestUri(NOW_PLAYING_MOVIES_ENDPOINT, page),
				Method = HttpMethod.Get
			};

			var moviesResult = await this.ExecuteRequestAsync<MoviesResult>(request);
			return moviesResult;
		}

		/// <summary>
		/// Gets the move by Id.
		/// </summary>
		/// <returns>The movie.</returns>
		/// <param name="id">Movie identifier.</param>

		public async Task<Movie> GetMovieByID(int id)
		{
			var request = new HttpRequestMessage { 
				RequestUri = this.CreateRequestUri(MOVIE_BY_ID_ENDPOINT, id),
				Method = HttpMethod.Get
			};

			var movie = await this.ExecuteRequestAsync<Movie> (request);
			return movie;
		}

		/// <summary>
		/// Gets the similar movies by movie Id.
		/// </summary>
		/// <returns>The similar movies.</returns>
		/// <param name="id">Movie identifier.</param>
		/// <param name="page">Page number.</param>

		public async Task<MoviesResult> GetSimilarMoviesByID(int id, int page = 1)
		{
			var request = new HttpRequestMessage {
				RequestUri = this.CreateRequestUri (SIMILAR_MOVIES_BY_ID_ENDPOINT, id, page),
				Method = HttpMethod.Get
			};

			var moviesResult = await this.ExecuteRequestAsync<MoviesResult> (request);
			return moviesResult;
		}

		/// <summary>
		/// Gets the movie videos by Id.
		/// </summary>
		/// <returns>The movie videos.</returns>
		/// <param name="id">Movie identifier.</param>

		public async Task<MovieVideosResult> GetMovieVideosByID(int id)
		{
			var request = new HttpRequestMessage { 
				RequestUri = this.CreateRequestUri (MOVIE_VIDEOS_BY_ID_ENDPOINT, id),
				Method = HttpMethod.Get
			};

			var movieVideosResult = await this.ExecuteRequestAsync<MovieVideosResult> (request);
			return movieVideosResult;
		}

		/// <summary>
		/// Executes the request async.
		/// </summary>
		/// <returns>The response data object.</returns>
		/// <param name="request">Request.</param>
		/// <typeparam name="T">The response data object type parameter.</typeparam>

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
						throw new TheMovieDBException (error.Message, response.StatusCode, error.Code, null);
					}
				}
				Debug.WriteLine (content);
			}
			return result;
		}

		/// <summary>
		/// Creates the request URI.
		/// </summary>
		/// <returns>The request URI.</returns>
		/// <param name="resource">Request URI resource string.</param>
		/// <param name="args">Request URI arguments.</param>

		private Uri CreateRequestUri(string resource, params Object[] args)
		{
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

