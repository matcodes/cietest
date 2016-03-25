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
	public class MoviesListView : ListView, AbsListView.IOnScrollListener
	{
		public MoviesListView (Context context)
			: this (context, null)
		{
		}

		public MoviesListView(Context context, IAttributeSet attr) 
			: this (context, attr, 0)
		{
		}

		public MoviesListView(Context context, IAttributeSet attr, int defStyleAttr)
			: base(context, attr, defStyleAttr)
		{
			this.Adapter = this.CreateAdapter ();

			this.SetOnScrollListener (this);

			this.ItemClick += (sender, args) => {
				this.SelectedMovie = (this.MovieAdapter.GetItem(args.Position) as Movie);
			};
		}

		private int _startX = int.MinValue;
		private int _startY = int.MinValue;

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

		protected virtual bool OnStartSwipe(SwipeInfo swipeInfo)
		{
			System.Diagnostics.Debug.WriteLine ("Start swipe position: " + swipeInfo.Position.ToString ());
			return false;
		}

		protected virtual bool OnContinueSwipe(SwipeInfo swipeInfo)
		{
			System.Diagnostics.Debug.WriteLine ("Continue swipe position: " + swipeInfo.Position.ToString ());
			return false;
		}

		protected virtual bool OnEndSwipe(SwipeInfo swipeInfo)
		{
			System.Diagnostics.Debug.WriteLine ("End swipe position: " + swipeInfo.Position.ToString ());
			return false;
		}

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

		public void UpdateData()
		{
			if (this.MovieAdapter != null)
				this.MovieAdapter.UpdateData ();
		}

		public void LoadData()
		{
			if (this.MovieAdapter != null)
				this.MovieAdapter.LoadData ();	
		}

		#region AbsListView.IOnScrollListener
		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{
			if ((!this.MovieAdapter.IsUpdating) && (firstVisibleItem + visibleItemCount == totalItemCount) && (firstVisibleItem > 0))
				this.MovieAdapter.LoadData ();
		}

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
		}
		#endregion

		protected void DoDataLoading()
		{
			if (this.DataLoading != null)
				this.DataLoading (this, EventArgs.Empty);
		}

		protected void DoDataLoaded()
		{
			if (this.DataLoaded != null)
				this.DataLoaded (this, EventArgs.Empty);
		}

		public Movie SelectedMovie { get; private set; }

		private MoviesRowArrayAdapter MovieAdapter {
			get { return (this.Adapter as MoviesRowArrayAdapter); }
		}

		public event EventHandler DataLoading;

		public event EventHandler DataLoaded;
	}
	#endregion
}

