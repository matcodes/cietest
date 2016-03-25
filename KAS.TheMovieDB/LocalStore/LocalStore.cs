using System;
using SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using KAS.TheMovieDB.API;

namespace KAS.TheMovieDB
{
	#region LocalStore
	/// <summary>
	/// Local store.
	/// </summary>
	public static class LocalStore
	{
		/// <summary>
		/// Initializes the <see cref="KAS.TheMovieDB.LocalStore"/> class.
		/// </summary>
		static LocalStore()
		{
			try {
				string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				Connection = new SQLiteConnection (System.IO.Path.Combine (folder, "TheMovieDB.db"));
				Connection.CreateTable<MovieEx> ();
			} catch (Exception exception) {
				System.Diagnostics.Debug.WriteLine (exception);
				throw exception;
			}
		}

		/// <summary>
		/// Saves the movie.
		/// </summary>
		/// <param name="movie">Movie.</param>
		public static void SaveMovie(Movie movie)
		{
			try {
				var movieEx = new MovieEx(movie);
				if (MovieExist(movieEx.ID))
					Connection.Update (movieEx);
				else
					Connection.Insert (movieEx);
			} catch (Exception exception) {
				System.Diagnostics.Debug.WriteLine (exception);
				throw exception;
			}
		}

		/// <summary>
		/// Removes the movie.
		/// </summary>
		/// <param name="movie">Movie.</param>
		public static void RemoveMovie(Movie movie)
		{
			try {
				var movieEx = new MovieEx (movie);
				Connection.Delete (movieEx);
			} catch (Exception exception) {
				System.Diagnostics.Debug.WriteLine (exception);
				throw exception;
			}
		}

		/// <summary>
		/// Gets the movie by I.
		/// </summary>
		/// <returns>The movie by I.</returns>
		/// <param name="id">Identifier.</param>
		public static Movie GetMovieByID(int id)
		{
			Movie movie = null;
			try {
				var movieEx = Connection.Table<MovieEx> ().Where (m => m.ID == id).FirstOrDefault ();
				if (movieEx != null)
					movie = movieEx.GetMovie ();
			} catch (Exception exception) {
				System.Diagnostics.Debug.WriteLine (exception);
				throw exception;
			}
			return movie;
		}

		/// <summary>
		/// Gets the movies.
		/// </summary>
		/// <returns>The movies.</returns>
		/// <param name="skip">Skip.</param>
		/// <param name="count">Count.</param>
		public static Movie[] GetMovies(int skip, int count)
		{
			var movies = new List<Movie> ();
			try {
				var query = Connection.Table<MovieEx> ().OrderByDescending (m => m.ReleaseDate).Skip (skip).Take (count); 

				foreach (var movie in query)
					movies.Add (movie.GetMovie());
			} catch (Exception exception) {
				System.Diagnostics.Debug.WriteLine (exception);
				throw exception;
			}
			return movies.ToArray ();
		}

		/// <summary>
		/// Movies the exist.
		/// </summary>
		/// <returns><c>true</c>, if exist was movied, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		private static bool MovieExist(int id)
		{
			return (Connection.Table<MovieEx> ().Where (m => m.ID == id).Count () > 0);
		}

		/// <summary>
		/// Gets or sets the connection.
		/// </summary>
		/// <value>The connection.</value>
		private static SQLiteConnection Connection { get; set; }
	}
	#endregion
}

