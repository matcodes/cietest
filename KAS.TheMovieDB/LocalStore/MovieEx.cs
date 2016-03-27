using System;
using SQLite;
using KAS.TheMovieDB.API;

namespace KAS.TheMovieDB
{
	#region MovieEx
	/// <summary>
	/// Helper class storage of the movie in a local data store.
	/// </summary>

	public class MovieEx
	{
		public MovieEx()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.MovieEx"/> class.
		/// </summary>
		/// <param name="movie">Movie from TheMoveDBAPI.</param>

		public MovieEx(Movie movie)
		{
			this.ID = movie.ID;
			this.IsAdult = movie.IsAdult;
			this.BackdropPath = movie.BackdropPath;
			this.OriginalLanguage = movie.OriginalLanguage;
			this.OriginalTitle = movie.OriginalTitle;
			this.Overview = movie.Overview;
			this.ReleaseDate = movie.ReleaseDate;
			this.PosterPath = movie.PosterPath;
			this.Popularity = movie.Popularity;
			this.Title = movie.Title;
			this.IsVideo = movie.IsVideo;
			this.VoteAvarage = movie.VoteAvarage;
			this.VoteCount = movie.VoteCount;
		}

		/// <summary>
		/// Initialization of TheMovieDBApi Movie based on the local storage of data.
		/// </summary>
		/// <returns>The movie from TheMovieDBAPI.</returns>

		public Movie GetMovie()
		{
			var movie = new Movie {
				ID = this.ID,
				IsAdult = this.IsAdult,
				BackdropPath = this.BackdropPath,
				OriginalLanguage = this.OriginalLanguage,
				OriginalTitle = this.OriginalTitle,
				Overview = this.Overview,
				ReleaseDate = this.ReleaseDate,
				PosterPath = this.PosterPath,
				Popularity = this.Popularity,
				Title = this.Title,
				IsVideo = this.IsVideo,
				VoteAvarage = this.VoteAvarage,
				VoteCount = this.VoteCount
			};
			return movie;
		}

		public bool IsAdult { get; set; }

		public string BackdropPath { get; set; }

		[PrimaryKey]
		public int ID { get; set; }

		public string OriginalLanguage { get; set; }

		public string OriginalTitle { get; set; }

		public string Overview { get; set; }

		public DateTime ReleaseDate { get; set; }

		public string PosterPath { get; set; }

		public double Popularity { get; set; }

		public string Title { get; set; }

		public bool IsVideo { get; set; }

		public double VoteAvarage { get; set; }

		public int VoteCount { get; set; }	}
	#endregion
}

