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
	internal class MoviesRowArrayAdapter : ArrayAdapter<Movie>
	{
		#region Static members
		private static readonly int MOVIE_LINE_ID = Resource.Layout.MovieLine;
		#endregion

		private LayoutInflater _inflater = null;

		private int _lastLoadingPage = 0;

		private string _posterBaseUri = "";

		protected int ImageWidth { get; set; }

		protected int ImageHeight { get; set; }

		protected List<Movie> Movies { get; set; }

		public MoviesRowArrayAdapter (Context context, List<Movie> movies)
			: base(context, MOVIE_LINE_ID, movies)
		{
			this.Movies = movies;

			_inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);

			this.IsUpdating = false;

		}

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

		public virtual void UpdateData()
		{
			_lastLoadingPage = 0;
			this.LoadData (true);
		}

		public virtual void LoadData(bool updated = false)
		{
			Task.Run (async () => {
				this.DoDataLoading ();
				try {

					this.IsUpdating = true;
					_lastLoadingPage++;

					await this.BuildPosterBaseUri ();

					var apiClient = new ThemoviedbAPI (Consts.THEMOVIEDB_API_KEY);

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

		protected async Task BuildPosterBaseUri ()
		{
			if (String.IsNullOrEmpty (_posterBaseUri)) {
				var configuration = await ThemoviedbAPI.GetConfiguration (Consts.THEMOVIEDB_API_KEY);
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

		protected void ShowMessage(string message)
		{
			(this.Context as Activity).RunOnUiThread (() => {
				AppHelper.ShowToast(this.Context, message);
			});
		}

		private void LoadMoviePoster(Movie movie, ImageView poster)
		{
			Picasso.With (this.Context)
				.Load (this.GetPosterUri (movie))
				.Into (poster);
		}

		private string GetPosterUri(Movie movie)
		{
			return _posterBaseUri + movie.PosterPath;
		}

		protected void DoDataLoaded()
		{
			if (this.DataLoaded != null)
				(this.Context as Activity).RunOnUiThread (() => {
					this.DataLoaded (this, EventArgs.Empty);
				});
		}

		protected void DoDataLoading()
		{
			if (this.DataLoading != null)
				(this.Context as Activity).RunOnUiThread (() => {
					this.DataLoading (this, EventArgs.Empty);
				});
		}

		public bool IsUpdating { get; protected set; }

		public event EventHandler DataLoading;

		public event EventHandler DataLoaded;
	}
	#endregion
}

