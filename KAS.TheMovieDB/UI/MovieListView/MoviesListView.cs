using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using KAS.TheMovieDB.API;

namespace KAS.TheMovieDB
{
	#region MovieListView
	/// <summary>
	/// Сontrol to work with a list of movies.
	/// </summary>
	public class MoviesListView : ListView, AbsListView.IOnScrollListener
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.MoviesListView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>

		public MoviesListView (Context context)
			: this (context, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.MoviesListView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attr">Attr.</param>

		public MoviesListView(Context context, IAttributeSet attr) 
			: this (context, attr, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.MoviesListView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attr">Attr.</param>
		/// <param name="defStyleAttr">Def style attr.</param>

		public MoviesListView(Context context, IAttributeSet attr, int defStyleAttr)
			: base(context, attr, defStyleAttr)
		{
			this.Adapter = this.CreateAdapter ();

			this.SetOnScrollListener (this);

			this.ItemClick += (sender, args) => {
				this.SelectedMovie = (this.MovieAdapter.GetItem(args.Position) as Movie);
			};
		}

		/// <summary>
		/// The start x of swipe.
		/// </summary>

		private int _startX = int.MinValue;
		/// <summary>
		/// The start y of swipe.
		/// </summary>

		private int _startY = int.MinValue;

		/// <param name="e">The motion event.</param>
		/// <summary>
		/// Implement this method to handle touch screen motion events.
		/// </summary>
		/// <returns>To be added.</returns>
		/// <param name="args">Arguments.</param>

		public override bool OnTouchEvent (Android.Views.MotionEvent args)
		{
			var result = false;
			if (args.Action == Android.Views.MotionEventActions.Down) {
				_startX = (int)args.GetX ();
				_startY = (int)args.GetY ();
				var position = this.PointToPosition (_startX, _startY);
				var swipeInfo = new SwipeInfo { 
					StartX = _startX,
					StartY = _startY,
					EndX = _startX,
					EndY = _startY,
					Position = position
				};
				result = this.OnStartSwipe (swipeInfo);
			} else if (args.Action == Android.Views.MotionEventActions.Move) {
				var x = (int)args.GetX ();
				var y = (int)args.GetY ();
				var position = this.PointToPosition (x, y);
				var swipeInfo = new SwipeInfo { 
					StartX = _startX,
					StartY = _startY,
					EndX = x,
					EndY = y,
					Position = position
				};
				result = this.OnContinueSwipe (swipeInfo);
			} else if (args.Action == Android.Views.MotionEventActions.Up) {
				var x = (int)args.GetX ();
				var y = (int)args.GetY ();
				var position = this.PointToPosition (x, y);
				var swipeInfo = new SwipeInfo { 
					StartX = _startX,
					StartY = _startY,
					EndX = x,
					EndY = y,
					Position = position
				};
				result = this.OnEndSwipe (swipeInfo);
			}

			if (!result)
				result = base.OnTouchEvent (args);

			return result;
		}

		/// <summary>
		/// Raises the start swipe event.
		/// </summary>
		/// <param name="swipeInfo">Swipe info.</param>

		protected virtual bool OnStartSwipe(SwipeInfo swipeInfo)
		{
			System.Diagnostics.Debug.WriteLine ("Start swipe position: " + swipeInfo.Position.ToString ());
			return false;
		}

		/// <summary>
		/// Raises the continue swipe event.
		/// </summary>
		/// <param name="swipeInfo">Swipe info.</param>

		protected virtual bool OnContinueSwipe(SwipeInfo swipeInfo)
		{
			System.Diagnostics.Debug.WriteLine ("Continue swipe position: " + swipeInfo.Position.ToString ());
			return false;
		}

		/// <summary>
		/// Raises the end swipe event.
		/// </summary>
		/// <param name="swipeInfo">Swipe info.</param>

		protected virtual bool OnEndSwipe(SwipeInfo swipeInfo)
		{
			System.Diagnostics.Debug.WriteLine ("End swipe position: " + swipeInfo.Position.ToString ());
			return false;
		}

		/// <summary>
		/// Creates the adapter.
		/// </summary>
		/// <returns>The adapter.</returns>

		protected virtual IListAdapter CreateAdapter()
		{
			var adapter = new MoviesRowArrayAdapter (this.Context, new List<Movie> ());
			adapter.DataLoading += (sender, e) => {
				this.DoDataLoading ();
			};
			adapter.DataLoaded += (sender, e) => {
				this.DoDataLoaded ();
			};
			return adapter;
		}

		/// <summary>
		/// Updates the data.
		/// </summary>

		public void UpdateData()
		{
			if (this.MovieAdapter != null)
				this.MovieAdapter.UpdateData ();
		}

		/// <summary>
		/// Loads the data.
		/// </summary>

		public void LoadData()
		{
			if (this.MovieAdapter != null)
				this.MovieAdapter.LoadData ();	
		}

		#region AbsListView.IOnScrollListener
		/// <summary>
		/// Raises the scroll event.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="firstVisibleItem">First visible item.</param>
		/// <param name="visibleItemCount">Visible item count.</param>
		/// <param name="totalItemCount">Total item count.</param>

		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{
			if ((!this.MovieAdapter.IsUpdating) && (firstVisibleItem + visibleItemCount == totalItemCount) && (firstVisibleItem > 0))
				this.MovieAdapter.LoadData ();
		}

		/// <summary>
		/// Raises the scroll state changed event.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="scrollState">Scroll state.</param>

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
		}
		#endregion

		/// <summary>
		/// Dos the data loading.
		/// </summary>

		protected void DoDataLoading()
		{
			if (this.DataLoading != null)
				this.DataLoading (this, EventArgs.Empty);
		}

		/// <summary>
		/// Dos the data loaded.
		/// </summary>

		protected void DoDataLoaded()
		{
			if (this.DataLoaded != null)
				this.DataLoaded (this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets the selected movie.
		/// </summary>
		/// <value>The selected movie.</value>

		public Movie SelectedMovie { get; private set; }

		/// <summary>
		/// Gets the movie adapter.
		/// </summary>
		/// <value>The movie adapter.</value>

		private MoviesRowArrayAdapter MovieAdapter {
			get { return (this.Adapter as MoviesRowArrayAdapter); }
		}

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

