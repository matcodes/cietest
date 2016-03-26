using System;
using NUnit.Framework;
using KAS.TheMovieDB.API;
using System.Threading.Tasks;
using System.Net;

namespace KAS.TheMovieDB.Test
{
	#region ThemoviedbAPITest
	[TestFixture]
	public class ThemoviedbAPITest
	{
		#region Static members
		public static readonly string THEMOVIEDB_API_KEY = "6f61777d326af5355399fc0681d8c621";

		public static readonly int MOVIE_ID = 209112;   // Batman v Superman: Dawn of Justice
		#endregion

		[SetUp]
		public void Setup ()
		{
		}

		
		[TearDown]
		public void Tear ()
		{
		}

		[Test]
		public void TestGetConfigurationAsync ()
		{
			Task.Run (async() => {
				var api = new TheMovieDBAPI (THEMOVIEDB_API_KEY);

				var configuration = await api.GetConfigurationAsync ();

				Assert.NotNull (configuration.ChangeKeys);
				Assert.IsTrue (configuration.ChangeKeys.Length > 0);
				Assert.NotNull (configuration.Images);
			});
		}

		[Test]
		public void TestGetNowPlayingMoviesAsync()
		{
			Task.Run (async () => {
				var api = new TheMovieDBAPI(THEMOVIEDB_API_KEY);

				var result = await api.GetNowPlayingMoviesAsync(1);

				Assert.NotNull(result.Results);
				Assert.True(result.Results.Length > 0);

				foreach (var movie in result.Results) {
					Console.WriteLine(movie.Title);
				}
			});
		}

		[Test]
		public void TestGetMovieByIDAsync()
		{
			Task.Run (async() => {
				var api = new TheMovieDBAPI (THEMOVIEDB_API_KEY);

				var result = await api.GetMovieByIDAsync (MOVIE_ID);

				Assert.NotNull (result);
				Assert.Equals (result.ID, MOVIE_ID);

				TheMovieDBException exception = null;
				try {
					result = await api.GetMovieByIDAsync (int.MaxValue);
				} catch (TheMovieDBException ex) {
					exception = ex;
				}

				Assert.NotNull (exception);
				Assert.Equals (exception.HttpStatus, HttpStatusCode.NotFound);
				Assert.Equals (exception.StatusCode, 6);
			});
		}

		[Test]
		public void TestGetSimilarMoviesByIDAsync()
		{
			Task.Run (async() => {
				var api = new TheMovieDBAPI(THEMOVIEDB_API_KEY);

				var result = await api.GetSimilarMoviesByIDAsync(MOVIE_ID, 1);

				Assert.NotNull(result.Results);
				Assert.True(result.Results.Length > 0);

				foreach (var movie in result.Results) {
					Console.WriteLine(movie.Title);
				}
			});
		}

		[Test]
		public void TestGetMovieVideosByIDAsync()
		{
			Task.Run (async() => {
				var api = new TheMovieDBAPI (THEMOVIEDB_API_KEY);

				var result = await api.GetMovieVideosByIDAsync (MOVIE_ID);

				Assert.NotNull (result.Results);
				Assert.True (result.Results.Length > 0);
			});
		}
	}
	#endregion
}

