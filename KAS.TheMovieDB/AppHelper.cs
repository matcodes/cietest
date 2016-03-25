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
	public static class AppHelper
	{
		public static void Initialize(Activity activity)
		{
			DisplayMetrics displayMetrics = new DisplayMetrics();
			activity.WindowManager.DefaultDisplay.GetMetrics (displayMetrics);
			DisplayHeight = displayMetrics.HeightPixels;
			DisplayWidth = displayMetrics.WidthPixels;			
		}

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

		public static void ShowToast(Context context, string message)
		{
			Toast.MakeText (context, message, ToastLength.Long).Show ();
		}

		public static int DisplayWidth { get; private set; }

		public static int DisplayHeight { get; private set; }
	}
	#endregion
}

