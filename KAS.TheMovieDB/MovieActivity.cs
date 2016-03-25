
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
using Newtonsoft.Json;
using System.Threading.Tasks;
using Square.Picasso;
using KAS.TheMovieDB.API;

namespace KAS.TheMovieDB
{
	#region MovieActivity
	[Activity (Label = "@string/moviePage")]			
	public class MovieActivity : Activity
	{
		#region Static members
		private static string __posterBaseUri;
		#endregion

		private RelativeLayout _movieData = null;
		private ProgressBar _progressBar = null;
		private ImageView _poster = null;
		private TextView _title = null;
		private TextView _releaseDate = null;
		private VoteView _voteView = null;
		private TextView _voteCount = null;
		private Button _playVideo = null;
		private Button _saveToFavorites = null;
		private TextView _overview = null;
		private ImageView _firstMovie = null;
		private ImageView _secondMovie = null;
		private ImageView _thirdMovie = null; 

		private Movie _firstSimilarMovie = null;
		private Movie _secondSimilarMovie = null;
		private Movie _thirdSimilarMovie = null;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.MoviePage);

			this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

			var movieIDPar = this.Intent.GetStringExtra (Consts.EXTRA_MOVIE);

			int movieID = -1;
			int.TryParse (movieIDPar, out movieID);

			_movieData = this.FindViewById<RelativeLayout> (Resource.Id.movieData);
			_movieData.Visibility = ViewStates.Gone;
			_progressBar = this.FindViewById<ProgressBar> (Resource.Id.progressBar);
			this.ShowProgressBar ();

			_poster = this.FindViewById<ImageView> (Resource.Id.poster);
			_title = this.FindViewById<TextView> (Resource.Id.title);
			_releaseDate = this.FindViewById<TextView> (Resource.Id.releaseDate);
			_voteView = this.FindViewById<VoteView> (Resource.Id.vote);
			_voteCount = this.FindViewById<TextView> (Resource.Id.voteCount);
			_playVideo = this.FindViewById<Button> (Resource.Id.playVideo);
			_playVideo.Click += async (sender, args) => {
				await this.PlayVideo();
			};
			_saveToFavorites = this.FindViewById<Button> (Resource.Id.saveToFavorites);
			_saveToFavorites.Click += (sender, args) => {
				this.AddToFavorites();
			};
			_overview = this.FindViewById<TextView> (Resource.Id.overview);
			_firstMovie = this.FindViewById<ImageView> (Resource.Id.firstMovie);
			_firstMovie.Click += (sender, args) => {
				this.ShowMovie(_firstSimilarMovie);
			};
			_secondMovie = this.FindViewById<ImageView> (Resource.Id.secondMovie);
			_secondMovie.Click += (sender, args) => {
				this.ShowMovie(_secondSimilarMovie);
			};
			_thirdMovie = this.FindViewById<ImageView> (Resource.Id.thirdMovie);
			_thirdMovie.Click += (sender, args) => {
				this.ShowMovie(_thirdSimilarMovie);
			};

			this.GetMovieByID (movieID);
		}

		private async Task PlayVideo()
		{
			if (this.Movie != null) {
				this.DisableCommands ();
				this.ShowProgressBar ();
				try {
					var api = new TheMovieDBAPI (Consts.THEMOVIEDB_API_KEY);
					var movieVideosResult = await api.GetMovieVideosByID (this.Movie.ID);
					var movieVideo = movieVideosResult.Results.FirstOrDefault (mv => mv.Site == Consts.VIDEO_SITE_YOUTUBE);
					if (movieVideo != null) {
						var uri = String.Format (Consts.YOUTUBE_VIDEO_URI, movieVideo.Key);
						var intent = new Intent (Intent.ActionView, Android.Net.Uri.Parse (uri));
						this.StartActivity (intent);
					}
					else
						this.ShowMessage(Consts.MOVIE_VIDEO_NOT_FOUND_MESSAGE_TEXT);
				} finally {
					this.HideProgressBar ();
					this.EnableCommands ();
				}
			}
		}

		private void DisableCommands()
		{
			this.RunOnUiThread (() => {
				_playVideo.Clickable = false;
				_saveToFavorites.Clickable = false;
				_firstMovie.Clickable = false;
				_secondMovie.Clickable = false;
				_thirdMovie.Clickable = false;
			});
		}

		private void EnableCommands()
		{
			this.RunOnUiThread (() => {
				_playVideo.Clickable = true;
				_saveToFavorites.Clickable = true;
				_firstMovie.Clickable = true;
				_secondMovie.Clickable = true;
				_thirdMovie.Clickable = true;
			});
		}

		private void ShowProgressBar()
		{
			this.RunOnUiThread (() => {
				_progressBar.Visibility = ViewStates.Visible;
			});	
		}

		private void HideProgressBar()
		{
			this.RunOnUiThread (() => {
				_progressBar.Visibility = ViewStates.Gone;
			});
		}

		private void AddToFavorites()
		{
			if (this.Movie != null) {
				Task.Run (() => {
					this.DisableCommands ();
					this.ShowProgressBar();
					try {
						LocalStore.SaveMovie(this.Movie);
						this.ShowFavorites();
					} catch (Exception exception) {
						System.Diagnostics.Debug.WriteLine (exception);
						this.ShowMessage(Consts.MOVIE_SAVE_TO_FAVORIT_ERROR_MESSAGE_TEXT);
					} finally {
						this.HideProgressBar();
						this.EnableCommands ();
					}
				});
			}
		}

		private void ShowFavorites()
		{
			this.RunOnUiThread (() => {
				Intent intent = new Intent(this, typeof(FavoritesActivity));
				intent.PutExtra(Consts.EXTRA_MOVIE, this.Movie.ID.ToString());
				this.StartActivity(intent);
			});
		}

		private void ShowMovie(Movie movie)
		{
			if (movie != null) {
				Intent intent = new Intent(this, typeof(MovieActivity));
				intent.PutExtra(Consts.EXTRA_MOVIE, movie.ID.ToString());
				this.StartActivity(intent);
			}
		}

		private void GetMovieByID(int movieID)
		{
			Task.Run (async () => {
				Movie movie = null;
				try {
					var api = new TheMovieDBAPI (Consts.THEMOVIEDB_API_KEY);
					movie = await api.GetMovieByID (movieID);
				} catch (Exception exception) {
					System.Diagnostics.Debug.WriteLine (exception);
				}

				if (movie != null)
					this.ShowMovieData (movie);
				else {
					this.RunOnUiThread (() => {
						this.ShowMessage(Consts.MOVIE_LOAD_DATA_ERROR_MESSAGE_TEXT);
						this.Finish ();
					});
				}
			});
		}

		private void ShowMovieData(Movie movie)
		{
			this.RunOnUiThread (() => {
				this.Movie = movie;
				_title.Text = (this.Movie != null ? this.Movie.Title : "");
				_releaseDate.Text = (this.Movie != null ? String.Format (this.Resources.GetString (Resource.String.releaseDate), this.Movie.ReleaseDate.ToShortDateString ()) : "");
				_voteView.Value = (this.Movie != null ? this.Movie.VoteAvarage : 0);
				_voteCount.Text = (this.Movie != null ? String.Format (this.Resources.GetString (Resource.String.voteCount), this.Movie.VoteCount) : "");
				_overview.Text = (this.Movie != null ? this.Movie.Overview : "");

				_movieData.Visibility = ViewStates.Visible;
				this.HideProgressBar();

				this.LoadPoster(this.Movie, _poster);
				this.LoadSimularMovies();
			});
		}

		private void LoadPoster(Movie movie, ImageView imageView)
		{
			Task.Run (async () => {
				try {
					if (String.IsNullOrEmpty (__posterBaseUri))
						__posterBaseUri = await this.GetPosterBaseUri (AppHelper.DisplayWidth / 2 * 5);

					var posterUri = __posterBaseUri + movie.PosterPath;

					this.RunOnUiThread(()=>{
						Picasso.With (this)
							.Load (posterUri)
							.Into (imageView);
					});
				} catch (Exception exception) {
					System.Diagnostics.Debug.WriteLine (exception);
				}
			});
		}

		private void ShowMessage(string message)
		{
			this.RunOnUiThread (() => {
				AppHelper.ShowToast(this, message);
			});
		}

		private void LoadSimularMovies()
		{
			Task.Run (async () => {
				try {
					var api = new TheMovieDBAPI (Consts.THEMOVIEDB_API_KEY);
					var similarMovies = await api.GetSimilarMoviesByID (this.Movie.ID);
					if (similarMovies.Results.Length > 0) {
						_firstSimilarMovie = similarMovies.Results [0];
						this.LoadPoster (_firstSimilarMovie, _firstMovie);
					}
					if (similarMovies.Results.Length > 1) {
						_secondSimilarMovie = similarMovies.Results [1];
						this.LoadPoster (_secondSimilarMovie, _secondMovie);
					}
					if (similarMovies.Results.Length > 2) {
						_thirdSimilarMovie = similarMovies.Results [2];
						this.LoadPoster (_thirdSimilarMovie, _thirdMovie);
					}
				} catch (Exception exception) {
					System.Diagnostics.Debug.WriteLine (exception);
				}
			});
		}

		private async Task<string> GetPosterBaseUri (int imageWidth)
		{
			var configuration = await TheMovieDBAPI.GetConfiguration (Consts.THEMOVIEDB_API_KEY);
			var posterBaseUri = configuration.Images.SecureBaseUrl;

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

			return posterBaseUri;
		}

		private Movie Movie { get; set; }
	}
	#endregion
}

