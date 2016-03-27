
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;

namespace KAS.TheMovieDB
{
	#region FavoritesActivity

	/// <summary>
	/// Page list of favorite movies.
	/// </summary>

	[Activity (Label = "@string/favoritesPage")]			
	public class FavoritesActivity : Activity, SwipeRefreshLayout.IOnRefreshListener
	{
		private SwipeRefreshLayout _refreshFavorites = null;
		private FavoritesListView _favoritesList = null;

		/// <summary>
		/// Page list of favorite movies.
		/// </summary>

		private bool _isLoaded = false;

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.FavoritesPage);

			this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

			var movieID = this.Intent.GetStringExtra (Consts.EXTRA_MOVIE);

			_refreshFavorites = this.FindViewById<SwipeRefreshLayout> (Resource.Id.refreshFavorites);
			_refreshFavorites.SetOnRefreshListener (this);

			_favoritesList = this.FindViewById<FavoritesListView> (Resource.Id.favoritesList);
			_favoritesList.ItemClick += (sender, args) => {
				_favoritesList.Enabled = false;
				try {
					var movie = _favoritesList.SelectedMovie;
					if (movie != null) {
						Intent intent = new Intent (this, typeof(MovieActivity));
						intent.PutExtra (Consts.EXTRA_MOVIE, movie.ID.ToString ());
						this.StartActivity (intent);
					}
				} finally {
					_favoritesList.Enabled = true;
				}
			};
			_favoritesList.DataLoading += (sender, args) => {
				_favoritesList.Enabled = false;
				_refreshFavorites.Refreshing = true;
			};
			_favoritesList.DataLoaded += (sender, args) => {
				_favoritesList.Enabled = true;
				_refreshFavorites.Refreshing = false;
			};

			if (!String.IsNullOrEmpty (movieID)) {
				int selectionID = -1;
				int.TryParse (movieID, out selectionID);
				_favoritesList.SelectionID = selectionID;
			}
		}

		/// <summary>
		/// Raises the start event.
		/// </summary>

		protected override void OnStart ()
		{
			base.OnStart ();

			if (!_isLoaded) {
				_isLoaded = false;
				_favoritesList.LoadData ();
			}
		}

		#region SwipeRefreshLayout.IOnRefreshListener
		/// <summary>
		/// Raises the refresh event.
		/// </summary>

		public void OnRefresh ()
		{
			_favoritesList.UpdateData ();
		}
		#endregion
	}
	#endregion
}

