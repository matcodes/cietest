using System;

namespace KAS.TheMovieDB
{
	#region SwipeInfo
	/// <summary>
	/// Swipe info.
	/// </summary>

	public class SwipeInfo
	{
		public SwipeInfo ()
		{
		}

		/// <summary>
		/// Gets or sets the start x.
		/// </summary>
		/// <value>The start x.</value>

		public int StartX { get; set; }

		/// <summary>
		/// Gets or sets the start y.
		/// </summary>
		/// <value>The start y.</value>

		public int StartY { get; set; }

		/// <summary>
		/// Gets or sets the end x.
		/// </summary>
		/// <value>The end x.</value>

		public int EndX { get; set; }

		/// <summary>
		/// Gets or sets the end y.
		/// </summary>
		/// <value>The end y.</value>

		public int EndY { get; set; }

		/// <summary>
		/// Gets or sets the item position of list.
		/// </summary>
		/// <value>The position.</value>

		public int Position { get; set; }
	}
	#endregion
}

