using System;
using NUnit.Framework;
using KAS.TheMovieDB.API;
using System.Threading.Tasks;

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
			}).Wait ();
		}
	}
	#endregion
}

