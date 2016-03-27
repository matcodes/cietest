using System;
using System.Linq;
using Android.Content;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using System.Collections;
using KAS.TheMovieDB.API;
using System.Threading;

namespace KAS.TheMovieDB
{
	#region FavoritesRowArrayAdapter
	/// <summary>
	/// Favorites row array adapter.
	/// </summary>

	internal class FavoritesRowArrayAdapter : MoviesRowArrayAdapter
	{
		/// <summary>
		/// The is loading flag.
		/// </summary>
		private bool _isLoading = false;

		/// <summary>
		/// The movies array.
		/// </summary>
		private Movie[] _movies = new Movie[] { };

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.FavoritesRowArrayAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="movies">Movies.</param>

		public FavoritesRowArrayAdapter (Context context, List<Movie> movies)
			: base(context, movies)
		{
		}

		/// <summary>
		/// Removes the movie.
		/// </summary>
		/// <param name="position">Position.</param>

		public void RemoveMovie(int position)
		{
			var movie = (this.GetItem (position) as Movie);
			if (movie != null) {
				(this.Context as Activity).RunOnUiThread (() => {
					this.Remove (movie);
					this.RemoveMovieQuestion (movie, position);
				});
			}
		}

		/// <summary>
		/// Updates the data.
		/// </summary>

		public override void UpdateData ()
		{
			_isLoading = false;

			base.UpdateData ();
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		/// <param name="updated">If set to <c>true</c> updated.</param>

		public override void LoadData (bool updated = false)
		{
			if (!_isLoading) {
				_isLoading = true;
				Task.Run (async () => {
					this.DoDataLoading ();
					try {
						this.IsUpdating = true;

						await this.BuildPosterBaseUri ();

						_movies = LocalStore.GetMovies (0, 1000);

						this.AddMoviesToList (_movies, updated);

						if (_movies.Length == 0)
							this.ShowMessage (Consts.FAVORITES_NO_DATA_MESSAGE_TEXT);

					} catch (Exception exception) {
						this.ShowMessage (Consts.FAVORITES_LOAD_DATA_ERROR_MESSAGE_TEXT);
						Debug.WriteLine (exception);
						this.IsUpdating = false;
					} finally {
						this.DoDataLoaded ();
					}
				});
			}
		}

		/// <summary>
		/// Gets the movie position by Id.
		/// </summary>
		/// <returns>The movie position by Id.</returns>
		/// <param name="id">Identifier.</param>

		public int GetMoviePositionByID(int id)
		{
			var position = -1;
			var movie = _movies.FirstOrDefault (m => m.ID == id);
			if (movie != null)
				position = (_movies as IList).IndexOf (movie);
			return position;
		}

		/// <summary>
		/// Removes the movie question.
		/// </summary>
		/// <param name="movie">Movie.</param>
		/// <param name="position">Position.</param>

		private void RemoveMovieQuestion(Movie movie, int position)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this.Context);
			builder
				.SetTitle (Consts.QUESTION_DELETE_ITEM_DIALOG_TITLE)
				.SetMessage (Consts.DELETE_FAVORITE_MOVIE_QUESTION_TEXT)
				.SetPositiveButton (Consts.QUESTION_YES_BUTTON_TEXT, delegate {
					this.ApplyRemoveItem (movie, position);
				})
				.SetNegativeButton (Consts.QUESTION_NO_BUTTON_TEXT, delegate {
					this.CancelRemoveItem (movie, position);
				})
				.Show ();
		}

		/// <summary>
		/// Applies the remove item.
		/// </summary>
		/// <param name="movie">Movie.</param>
		/// <param name="position">Position.</param>

		private void ApplyRemoveItem(Movie movie, int position)
		{
			try {
				LocalStore.RemoveMovie (movie);
			} catch (Exception exception) {
				System.Diagnostics.Debug.WriteLine (exception);
				this.Insert (movie, position);
				this.ShowMessage (Consts.FAVORITE_REMOVE_ERROR_MESSAGE_TEXT);
			}
		}

		/// <summary>
		/// Determines whether this instance cancel remove item the specified movie position.
		/// </summary>
		/// <returns><c>true</c> if this instance cancel remove item the specified movie position; otherwise, <c>false</c>.</returns>
		/// <param name="movie">Movie.</param>
		/// <param name="position">Position.</param>

		private void CancelRemoveItem(Movie movie, int position)
		{
			this.Insert (movie, position);
		}
	}
	#endregion
}

