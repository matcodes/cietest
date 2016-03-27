using System;
using Android.App;
using Android.Util;
using System.Threading.Tasks;
using Android.Graphics;
using System.Net;
using Android.Widget;
using Android.Content;

namespace KAS.TheMovieDB
{
	#region AppHelper
	/// <summary>
	/// Helper class for applications.
	/// </summary>

	public static class AppHelper
	{
		/// <summary>
		/// Initializes a helper class.
		/// </summary>
		/// <param name="activity">Activity.</param>
		public static void Initialize(Activity activity)
		{
			DisplayMetrics displayMetrics = new DisplayMetrics();
			activity.WindowManager.DefaultDisplay.GetMetrics (displayMetrics);
			DisplayHeight = displayMetrics.HeightPixels;
			DisplayWidth = displayMetrics.WidthPixels;			
		}

		/// <summary>
		/// Gets the image bitmap from URL.
		/// </summary>
		/// <returns>The image bitmap from URL.</returns>
		/// <param name="url">URL.</param>

		public static Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			try {
				using (var webClient = new WebClient ()) {
					var imageBytes = webClient.DownloadData (url);
					if (imageBytes != null && imageBytes.Length > 0)
						imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
				}
			} catch {
			}

			return imageBitmap;
		}

		/// <summary>
		/// Shows the toast.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="message">Message.</param>

		public static void ShowToast(Context context, string message)
		{
			Toast.MakeText (context, message, ToastLength.Long).Show ();
		}

		/// <summary>
		/// Gets the display width.
		/// </summary>
		/// <value>The display width.</value>

		public static int DisplayWidth { get; private set; }

		/// <summary>
		/// Gets the display height.
		/// </summary>
		/// <value>The display height.</value>

		public static int DisplayHeight { get; private set; }
	}
	#endregion
}

