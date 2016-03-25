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

namespace KAS.TheMovieDB
{
	#region FavoritesRowArrayAdapter
	internal class FavoritesRowArrayAdapter : MoviesRowArrayAdapter
	{
		private bool _isLoading = false;

		private Movie[] _movies = new Movie[]{};

		public FavoritesRowArrayAdapter (Context context, List<Movie> movies)
			: base(context, movies)
		{
		}

		public void RemoveMovie(int position)
		{
			var movie = (this.GetItem (position) as Movie);
			if (movie != null) {
				(this.Context as Activity).RunOnUiThread (() => {
					this.Remove(movie);
					try {
						LocalStore.RemoveMovie(movie);
					} catch (Exception exception) {
						System.Diagnostics.Debug.WriteLine(exception);
						this.Insert(movie, position);
						this.ShowMessage(Consts.FAVORITE_REMOVE_ERROR_MESSAGE_TEXT);
					}
				});
			}
		}

		public override void UpdateData ()
		{
			_isLoading = false;

			base.UpdateData ();
		}

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

		public int GetMoviePositionByID(int id)
		{
			var position = -1;
			var movie = _movies.FirstOrDefault (m => m.ID == id);
			if (movie != null)
				position = (_movies as IList).IndexOf (movie);
			return position;
		}
	}
	#endregion
}

