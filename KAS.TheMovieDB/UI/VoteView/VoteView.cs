using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Graphics;
using Android.App;

namespace KAS.TheMovieDB
{
	#region VoteView
	public class VoteView : RelativeLayout
	{
		#region Static members
		private static Bitmap __bitmap = null;
		#endregion

		private ImageView _emptyVote = null;
		private ImageView _fullVote = null;

		private double _minValue = 0;
		private double _maxValue = 10;
		private double _value = 10;

		private int _controlWidth = 0;
		private int _controlHeight = 0;

		public VoteView (Context context)
			: this(context, null)
		{
		}

		public VoteView(Context context, IAttributeSet attrs) 
			: this(context, attrs, 0)
		{
		}

		public VoteView(Context context, IAttributeSet attrs, int defStyleAttr) 
			: base (context, attrs, defStyleAttr)
		{
			this.CreateEmptyVote ();
			this.CreateFullVote ();
		}

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);

			if (this.ControlWidth != w)
				this.ControlWidth = w;

			if (this.ControlHeight != h)
				this.ControlHeight = h;

			this.ResetVote ();
		}

		private void SetMinValue(double minValue)
		{
			if (_minValue != minValue) {
				_minValue = minValue;
				this.ResetVote ();
			}
		}

		private void SetMaxValue(double maxValue)
		{
			if (_maxValue != maxValue) {
				_maxValue = maxValue;
				this.ResetVote ();
			}
		}

		private void SetValue(double value)
		{
			if (_value != value) {
				_value = value;
				this.ResetVote ();
			}
		}

		private void SetControlWidth(int controlWidth)
		{
			if (_controlWidth != controlWidth) {
				_controlWidth = controlWidth;
				var votePar = (this.LayoutParameters as RelativeLayout.LayoutParams);
				votePar.Width = controlWidth;
				this.ResetVote ();
			}
		}

		private void SetControlHeight(int controlHeight)
		{
			if (_controlHeight != controlHeight) {
				_controlHeight = controlHeight;
				var votePar = (this.LayoutParameters as RelativeLayout.LayoutParams);
				votePar.Height = controlHeight;
				this.ResetVote ();
			}
		}

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

		public double MinValue {
			get { return _minValue; }
			set { this.SetMinValue (value); }
		}

		public double MaxValue {
			get { return _maxValue; }
			set { this.SetValue (value); }
		}

		public double Value {
			get { return _value; }
			set { this.SetValue (value); }
		}

		public int ControlWidth {
			get { return _controlWidth; }
			set { this.SetControlWidth (value); }
		}

		public int ControlHeight {
			get { return _controlHeight; }
			set { this.SetControlHeight (value); }
		}
	}
	#endregion
}

