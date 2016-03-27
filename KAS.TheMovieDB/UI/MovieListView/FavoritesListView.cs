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
	/// <summary>
	/// Сontrol to work with a list of favorites movies.
	/// </summary>

	public class FavoritesListView : MoviesListView
	{
		private View _view = null;
		private int _position = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.FavoritesListView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>

		public FavoritesListView (Context context)
			: this (context, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.FavoritesListView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attr">Attr.</param>

		public FavoritesListView(Context context, IAttributeSet attr) 
			: this (context, attr, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.FavoritesListView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attr">Attr.</param>
		/// <param name="defStyleAttr">Def style attr.</param>

		public FavoritesListView(Context context, IAttributeSet attr, int defStyleAttr)
			: base(context, attr, defStyleAttr)
		{
		}

		/// <summary>
		/// Scrolls to item.
		/// </summary>
		/// <param name="id">Identifier.</param>

		public void ScrollToItem(int id)
		{
			var position = (this.Adapter as FavoritesRowArrayAdapter).GetMoviePositionByID (id);
			if (position >= 0)
				this.SetSelection (position);
		}

		/// <summary>
		/// Creates the adapter of the list.
		/// </summary>
		/// <returns>The adapter.</returns>

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

		/// <summary>
		/// Raises the start swipe event.
		/// </summary>
		/// <param name="swipeInfo">Swipe info.</param>

		protected override bool OnStartSwipe (SwipeInfo swipeInfo)
		{
			_position = swipeInfo.Position;
			_view = this.GetViewByPosition (swipeInfo.Position);
			return false;
		}

		/// <summary>
		/// Raises the continue swipe event.
		/// </summary>
		/// <param name="swipeInfo">Swipe info.</param>

		protected override bool OnContinueSwipe (SwipeInfo swipeInfo)
		{
			if (_position != swipeInfo.Position) {
				_view = null;
				_position = -1;
			}
			return false;
		}

		/// <summary>
		/// Raises the end swipe event.
		/// </summary>
		/// <param name="swipeInfo">Swipe info.</param>

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

		/// <summary>
		/// Gets the view by position.
		/// </summary>
		/// <returns>The view by position.</returns>
		/// <param name="position">Position.</param>

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

		/// <summary>
		/// Gets or sets the selection Id.
		/// </summary>
		/// <value>The selection Id.</value>

		public int SelectionID { get; set;}
	}
	#endregion
}

