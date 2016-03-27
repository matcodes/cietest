using System;
using System.Linq;
using Android.Widget;
using Android.Content;
using Android.Views;
using System.Threading.Tasks;
using Android.App;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using Square.Picasso;
using KAS.TheMovieDB.API;

namespace KAS.TheMovieDB
{
	#region MovieRowArrayAdapter
	/// <summary>
	/// Movies row array adapter.
	/// </summary>

	internal class MoviesRowArrayAdapter : ArrayAdapter<Movie>
	{
		#region Static members
		private static readonly int MOVIE_LINE_ID = Resource.Layout.MovieLine;
		#endregion

		/// <summary>
		/// The inflater.
		/// </summary>

		private LayoutInflater _inflater = null;

		/// <summary>
		/// The last loading page number.
		/// </summary>

		private int _lastLoadingPage = 0;

		/// <summary>
		/// The poster base URI.
		/// </summary>

		private string _posterBaseUri = "";

		/// <summary>
		/// Gets or sets the width of the image.
		/// </summary>
		/// <value>The width of the image.</value>

		protected int ImageWidth { get; set; }

		/// <summary>
		/// Gets or sets the height of the image.
		/// </summary>
		/// <value>The height of the image.</value>

		protected int ImageHeight { get; set; }

		/// <summary>
		/// Gets or sets the movies.
		/// </summary>
		/// <value>The movies.</value>

		protected List<Movie> Movies { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.MoviesRowArrayAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="movies">Movies.</param>

		public MoviesRowArrayAdapter (Context context, List<Movie> movies)
			: base(context, MOVIE_LINE_ID, movies)
		{
			this.Movies = movies;

			_inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);

			this.IsUpdating = false;

		}

		/// <param name="position">The position of the item within the adapter's data set of the item whose view
		///  we want.</param>
		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <returns>The view.</returns>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent.</param>

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView;

			if (position >= 0) {
				if (view == null)
					view = _inflater.Inflate (MOVIE_LINE_ID, null);

				var movie = (this.GetItem (position) as Movie);

				var poster = view.FindViewById<ImageView> (Resource.Id.poster);
				var posterPar = (poster.LayoutParameters as LinearLayout.LayoutParams);
				posterPar.Width = this.ImageWidth;
				posterPar.Height = this.ImageHeight;
				this.LoadMoviePoster (movie, poster);

				var info = view.FindViewById<RelativeLayout> (Resource.Id.info);
				var infoPar = (info.LayoutParameters as LinearLayout.LayoutParams);
				infoPar.Height = ImageHeight;

				var title = view.FindViewById<TextView> (Resource.Id.title);
				title.Text = (movie != null ? movie.OriginalTitle : "");

				var releaseDate = view.FindViewById<TextView> (Resource.Id.releaseDate);
				releaseDate.Text = (movie != null ? String.Format ("Release Date: {0}", movie.ReleaseDate.ToShortDateString ()) : "");

				var vote = view.FindViewById<VoteView> (Resource.Id.vote);
				var height = this.ImageHeight / 5;
				vote.ControlHeight = height;
				vote.ControlWidth = height * 5;
				vote.Value = movie.VoteAvarage;
			}

			return view;
		}

		/// <summary>
		/// Updates the data.
		/// </summary>

		public virtual void UpdateData()
		{
			_lastLoadingPage = 0;
			this.LoadData (true);
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		/// <param name="updated">If set to <c>true</c> updated.</param>

		public virtual void LoadData(bool updated = false)
		{
			Task.Run (async () => {
				this.DoDataLoading ();
				try {

					this.IsUpdating = true;
					_lastLoadingPage++;

					await this.BuildPosterBaseUri ();

					var apiClient = new TheMovieDBAPI (Consts.THEMOVIEDB_API_KEY);

					var result = await apiClient.GetNowPlayingMoviesAsync (_lastLoadingPage);

					this.AddMoviesToList (result.Results, updated);

				} catch (Exception exception) {
					this.ShowMessage (Consts.MOVIES_LOAD_DATA_ERROR_MESSAGE_TEXT);
					Debug.WriteLine (exception);
					_lastLoadingPage--;
					this.IsUpdating = false;
				} finally {
					this.DoDataLoaded ();
				}
			});
		}

		/// <summary>
		/// Adds the movies to list.
		/// </summary>
		/// <param name="movies">Movies.</param>
		/// <param name="updated">If set to <c>true</c> updated.</param>

		protected void AddMoviesToList(Movie[] movies, bool updated)
		{
			(this.Context as Activity).RunOnUiThread (() => {
				try {
					if (updated)
						this.Clear ();
					this.AddAll (movies);
				} finally {
					this.IsUpdating = false;
				}
			});
		}

		/// <summary>
		/// Builds the poster base URI.
		/// </summary>
		/// <returns>The poster base URI.</returns>

		protected async Task BuildPosterBaseUri ()
		{
			if (String.IsNullOrEmpty (_posterBaseUri)) {
				var configuration = await TheMovieDBAPI.GetConfiguration (Consts.THEMOVIEDB_API_KEY);
				var posterBaseUri = configuration.Images.SecureBaseUrl;

				var imageWidth = AppHelper.DisplayWidth / 5;

				var index = 0;
				var ip = "";
				while (index < configuration.Images.PosterSizes.Length) {
					try {
						var width = int.Parse (configuration.Images.PosterSizes [index].Replace ("w", ""));
						if (imageWidth > width)
							ip = configuration.Images.PosterSizes [index];
					} catch {
					}
					index++;
				}

				posterBaseUri += ip;

				_posterBaseUri = posterBaseUri;
				this.ImageWidth = imageWidth;
				this.ImageHeight = imageWidth / 2 * 3;
			}
		}

		/// <summary>
		/// Shows the toast message.
		/// </summary>
		/// <param name="message">Message.</param>

		protected void ShowMessage(string message)
		{
			(this.Context as Activity).RunOnUiThread (() => {
				AppHelper.ShowToast(this.Context, message);
			});
		}

		/// <summary>
		/// Loads the movie poster.
		/// </summary>
		/// <param name="movie">Movie.</param>
		/// <param name="poster">Poster.</param>

		private void LoadMoviePoster(Movie movie, ImageView poster)
		{
			Picasso.With (this.Context)
				.Load (this.GetPosterUri (movie))
				.Into (poster);
		}

		/// <summary>
		/// Gets the poster URI of movie.
		/// </summary>
		/// <returns>The poster URI.</returns>
		/// <param name="movie">Movie.</param>

		private string GetPosterUri(Movie movie)
		{
			return _posterBaseUri + movie.PosterPath;
		}

		/// <summary>
		/// Dos the data loaded.
		/// </summary>

		protected void DoDataLoaded()
		{
			if (this.DataLoaded != null)
				(this.Context as Activity).RunOnUiThread (() => {
					this.DataLoaded (this, EventArgs.Empty);
				});
		}

		/// <summary>
		/// Dos the data loading.
		/// </summary>

		protected void DoDataLoading()
		{
			if (this.DataLoading != null)
				(this.Context as Activity).RunOnUiThread (() => {
					this.DataLoading (this, EventArgs.Empty);
				});
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is updating.
		/// </summary>
		/// <value><c>true</c> if this instance is updating; otherwise, <c>false</c>.</value>

		public bool IsUpdating { get; protected set; }

		/// <summary>
		/// Occurs when data loading.
		/// </summary>

		public event EventHandler DataLoading;

		/// <summary>
		/// Occurs when data loaded.
		/// </summary>

		public event EventHandler DataLoaded;
	}
	#endregion
}

