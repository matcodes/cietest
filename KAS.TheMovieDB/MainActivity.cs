using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android.Content;
using Newtonsoft.Json;
using Android.Support.V4.Widget;

namespace KAS.TheMovieDB
{
	#region MainActivity
	/// <summary>
	/// Home application page. The list of new movies.
	/// </summary>

	[Activity (Label = "@string/mainPage", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, SwipeRefreshLayout.IOnRefreshListener
	{
		private SwipeRefreshLayout _refreshMovies = null;
		private MoviesListView _movieListView = null;
		private ImageButton _favorites = null;

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

			this.SetContentView (Resource.Layout.Main);

			this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

			AppHelper.Initialize (this);

			_refreshMovies = this.FindViewById<SwipeRefreshLayout> (Resource.Id.refreshMovies);
			_refreshMovies.SetOnRefreshListener (this);

			_movieListView = this.FindViewById<MoviesListView> (Resource.Id.movieList);
			_movieListView.ItemClick += (sender, args) => {
				_movieListView.Enabled = false;
				try {
					var movie = _movieListView.SelectedMovie;
					if (movie != null) {
						Intent intent = new Intent (this, typeof(MovieActivity));
						intent.PutExtra (Consts.EXTRA_MOVIE, movie.ID.ToString ());
						this.StartActivity (intent);
					}
				} finally {
					_movieListView.Enabled = true;
				}
			};
			_movieListView.DataLoading += (sender, args) => {
				_movieListView.Enabled = false;
				_refreshMovies.Refreshing = true;
			};
			_movieListView.DataLoaded += (sender, args) => {
				_movieListView.Enabled = true;
				_refreshMovies.Refreshing = false;
			};

			_favorites = this.FindViewById<ImageButton> (Resource.Id.favorites);
			var favoritesPar = (_favorites.LayoutParameters as RelativeLayout.LayoutParams);
			favoritesPar.Width = AppHelper.DisplayWidth / 8;
			favoritesPar.Height = favoritesPar.Width;
			_favorites.Click += (sender, args) => {
				_movieListView.Enabled = false;
				try {
					Intent intent = new Intent (this, typeof(FavoritesActivity));
					this.StartActivity (intent);
				} finally {
					_movieListView.Enabled = true;
				}
			};
		}

		/// <summary>
		/// Raises the start event.
		/// </summary>

		protected override void OnStart ()
		{
			base.OnStart ();

			if (!_isLoaded) {
				_isLoaded = true;
				_movieListView.LoadData ();
			}
		}

		#region SwipeRefreshLayout.IOnRefreshListener
		/// <summary>
		/// Raises the refresh event.
		/// </summary>

		public void OnRefresh ()
		{
			_movieListView.UpdateData ();
		}
		#endregion
	}
	#endregion
}


