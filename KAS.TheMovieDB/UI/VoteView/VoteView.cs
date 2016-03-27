using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Graphics;
using Android.App;

namespace KAS.TheMovieDB
{
	#region VoteView
	/// <summary>
	/// Visual control for display on a five-point rating scale.
	/// </summary>

	public class VoteView : RelativeLayout
	{
		#region Static members
		/// <summary>
		/// Stores the base image to display votes.
		/// </summary>

		private static Bitmap __bitmap = null;
		#endregion

		private ImageView _emptyVote = null;
		private ImageView _fullVote = null;

		private double _minValue = 0;
		private double _maxValue = 10;
		private double _value = 10;

		private int _controlWidth = 0;
		private int _controlHeight = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.VoteView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>

		public VoteView (Context context)
			: this(context, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.VoteView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attrs">Attrs.</param>

		public VoteView(Context context, IAttributeSet attrs) 
			: this(context, attrs, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.VoteView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attrs">Attrs.</param>
		/// <param name="defStyleAttr">Def style attr.</param>

		public VoteView(Context context, IAttributeSet attrs, int defStyleAttr) 
			: base (context, attrs, defStyleAttr)
		{
			this.CreateEmptyVote ();
			this.CreateFullVote ();
		}

		/// <param name="w">Current width of this view.</param>
		/// <param name="h">Current height of this view.</param>
		/// <param name="oldw">Old width of this view.</param>
		/// <param name="oldh">Old height of this view.</param>
		/// <summary>
		/// This is called during layout when the size of this view has changed.
		/// </summary>

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);

			if (this.ControlWidth != w)
				this.ControlWidth = w;

			if (this.ControlHeight != h)
				this.ControlHeight = h;

			this.ResetVote ();
		}

		/// <summary>
		/// It sets the minimum value for the vote.
		/// </summary>
		/// <param name="minValue">Minimum value.</param>

		private void SetMinValue(double minValue)
		{
			if (_minValue != minValue) {
				_minValue = minValue;
				this.ResetVote ();
			}
		}

		/// <summary>
		/// It sets the minimum value for the vote.
		/// </summary>
		/// <param name="maxValue">Max value.</param>

		private void SetMaxValue(double maxValue)
		{
			if (_maxValue != maxValue) {
				_maxValue = maxValue;
				this.ResetVote ();
			}
		}

		/// <summary>
		/// It sets the minimum value for the vote.
		/// </summary>
		/// <param name="value">Value.</param>

		private void SetValue(double value)
		{
			if (_value != value) {
				_value = value;
				this.ResetVote ();
			}
		}

		/// <summary>
		/// Sets the width of the control.
		/// </summary>
		/// <param name="controlWidth">Control width.</param>

		private void SetControlWidth(int controlWidth)
		{
			if (_controlWidth != controlWidth) {
				_controlWidth = controlWidth;
				var votePar = (this.LayoutParameters as RelativeLayout.LayoutParams);
				votePar.Width = controlWidth;
				this.ResetVote ();
			}
		}

		/// <summary>
		/// Sets the height of the control.
		/// </summary>
		/// <param name="controlHeight">Control height.</param>

		private void SetControlHeight(int controlHeight)
		{
			if (_controlHeight != controlHeight) {
				_controlHeight = controlHeight;
				var votePar = (this.LayoutParameters as RelativeLayout.LayoutParams);
				votePar.Height = controlHeight;
				this.ResetVote ();
			}
		}

		/// <summary>
		/// It updates the current state of the vote control.
		/// </summary>

		private void ResetVote()
		{
			if (__bitmap == null)
				__bitmap = BitmapFactory.DecodeResource (this.Context.Resources, Resource.Drawable.FullVote);
			
			var newBitmapWidth = (int)(__bitmap.Width / this.MaxValue * this.Value);

			Bitmap bitmap = null;
			if (newBitmapWidth > 0) {
				bitmap = Bitmap.CreateBitmap (__bitmap, 0, 0, newBitmapWidth, __bitmap.Height);
			}
			_fullVote.SetImageBitmap (bitmap);

			var viewWidth = (int)(this.ControlWidth / this.MaxValue * this.Value);

			var par = (_fullVote.LayoutParameters as RelativeLayout.LayoutParams);
			par.Width = viewWidth;
		}

		/// <summary>
		/// Creates the empty vote image view.
		/// </summary>

		private void CreateEmptyVote()
		{
			_emptyVote = new ImageView (this.Context);
			var par = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
			par.AddRule (LayoutRules.AlignParentLeft);
			par.AddRule (LayoutRules.AlignParentTop);
			_emptyVote.LayoutParameters = par;
			var bitmap = BitmapFactory.DecodeResource (this.Context.Resources, Resource.Drawable.EmptyVote);
			_emptyVote.SetImageBitmap (bitmap);
			this.AddView (_emptyVote);
		}

		/// <summary>
		/// Creates the full vote image view.
		/// </summary>

		private void CreateFullVote()
		{
			_fullVote = new ImageView (this.Context);
			var par = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
			par.AddRule (LayoutRules.AlignParentLeft);
			par.AddRule (LayoutRules.AlignParentTop);
			_fullVote.LayoutParameters = par;
			var bitmap = BitmapFactory.DecodeResource (this.Context.Resources, Resource.Drawable.FullVote);
			_fullVote.SetImageBitmap (bitmap);
			this.AddView (_fullVote);
		}

		/// <summary>
		/// Gets or sets the minimum value of the vote.
		/// </summary>
		/// <value>The minimum value.</value>

		public double MinValue {
			get { return _minValue; }
			set { this.SetMinValue (value); }
		}

		/// <summary>
		/// Gets or sets the max value of the vote.
		/// </summary>
		/// <value>The max value.</value>

		public double MaxValue {
			get { return _maxValue; }
			set { this.SetValue (value); }
		}

		/// <summary>
		/// Gets or sets the current value of the vote.
		/// </summary>
		/// <value>The value.</value>

		public double Value {
			get { return _value; }
			set { this.SetValue (value); }
		}

		/// <summary>
		/// Gets or sets the width of the vote control.
		/// </summary>
		/// <value>The width of the control.</value>

		public int ControlWidth {
			get { return _controlWidth; }
			set { this.SetControlWidth (value); }
		}

		/// <summary>
		/// Gets or sets the height of the vote control.
		/// </summary>
		/// <value>The height of the control.</value>

		public int ControlHeight {
			get { return _controlHeight; }
			set { this.SetControlHeight (value); }
		}
	}
	#endregion
}

