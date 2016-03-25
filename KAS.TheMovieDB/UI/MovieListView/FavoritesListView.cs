using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.App;
using Java.Lang;
using System.Collections.Generic;
using KAS.TheMovieDB.API;

namespace KAS.TheMovieDB
{
	#region FavoritesListView
	public class FavoritesListView : MoviesListView
	{
		private View _view = null;
		private int _position = -1;

		public FavoritesListView (Context context)
			: this (context, null)
		{
		}

		public FavoritesListView(Context context, IAttributeSet attr) 
			: this (context, attr, 0)
		{
		}

		public FavoritesListView(Context context, IAttributeSet attr, int defStyleAttr)
			: base(context, attr, defStyleAttr)
		{
		}

		public void ScrollToItem(int id)
		{
			var position = (this.Adapter as FavoritesRowArrayAdapter).GetMoviePositionByID (id);
			if (position >= 0)
				this.SetSelection (position);
		}

		protected override Android.Widget.IListAdapter CreateAdapter ()
		{
			var adapter = new FavoritesRowArrayAdapter (this.Context, new List<Movie> ());

			adapter.DataLoading += (sender, args) => {
				this.DoDataLoading ();
			};
			adapter.DataLoaded += (sender, args) => {
				if (SelectionID >= 0) {
					this.ScrollToItem (SelectionID);
					SelectionID = -1;
				}

				this.DoDataLoaded ();
			};

			return adapter;
		}

		protected override bool OnStartSwipe (SwipeInfo swipeInfo)
		{
			_position = swipeInfo.Position;
			_view = this.GetViewByPosition (swipeInfo.Position);
			return false;
		}

		protected override bool OnContinueSwipe (SwipeInfo swipeInfo)
		{
			if (_position != swipeInfo.Position) {
				_view = null;
				_position = -1;
			}
			return false;
		}

		protected override bool OnEndSwipe (SwipeInfo swipeInfo)
		{
			var result = false;
			if ((_view != null) && (_position == swipeInfo.Position) && (System.Math.Abs(swipeInfo.StartX - swipeInfo.EndX) > _view.Width / 3)) {
				var view = _view;
				this.Enabled = false;
				view.Animate ()
					.SetDuration (500)
					.Alpha (0)
					.WithEndAction (new Runnable (() => {
						(this.Adapter as FavoritesRowArrayAdapter).RemoveMovie (swipeInfo.Position);
						view.Alpha = 1;
						this.Enabled = true;
				}));
				_view = null;
				_position = -1;
				result = true;
			}
			return result;
		}

		private View GetViewByPosition(int position) 
		{
			View view = null;

			var firstListItemPosition = this.FirstVisiblePosition;
			var lastListItemPosition = firstListItemPosition + this.ChildCount - 1;

			if ((position < firstListItemPosition) || (position > lastListItemPosition )) 
				view = this.Adapter.GetView(position, null, this);
			else {
				var childIndex = position - firstListItemPosition;
				view = this.GetChildAt(childIndex);
			}

			return view;
		}

		public int SelectionID { get; set;}
	}
	#endregion
}

