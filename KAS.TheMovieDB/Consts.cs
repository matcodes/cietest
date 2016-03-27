using System;

namespace KAS.TheMovieDB
{
	#region Consts
	public static class Consts
	{
		public static readonly string THEMOVIEDB_API_KEY = "6f61777d326af5355399fc0681d8c621";

		public static readonly string VIDEO_SITE_YOUTUBE = "YouTube";

		public static readonly string YOUTUBE_VIDEO_URI = "http://www.youtube.com/watch?v={0}";

		public static readonly string FANDANGO_URI = "http://www.fandango.com/search/?q={0}";

		public static readonly string EXTRA_MOVIE = "Movie";


		public static readonly string FAVORITE_REMOVE_ERROR_MESSAGE_TEXT = "It is not possible to delete an item.";
		public static readonly string FAVORITES_NO_DATA_MESSAGE_TEXT = "No data to display.";
		public static readonly string FAVORITES_LOAD_DATA_ERROR_MESSAGE_TEXT = "Not possible to get data.";
		public static readonly string MOVIES_LOAD_DATA_ERROR_MESSAGE_TEXT = "Not possible to get data from the server.";
		public static readonly string MOVIE_LOAD_DATA_ERROR_MESSAGE_TEXT = "Movie info not found!";
		public static readonly string MOVIE_VIDEO_NOT_FOUND_MESSAGE_TEXT = "Video not found!";
		public static readonly string MOVIE_SAVE_TO_FAVORIT_ERROR_MESSAGE_TEXT = "Save the item error.";

		public static readonly string QUESTION_DELETE_ITEM_DIALOG_TITLE = "Delete item";
		public static readonly string QUESTION_YES_BUTTON_TEXT = "Yes";
		public static readonly string QUESTION_NO_BUTTON_TEXT = "No";

		public static readonly string DELETE_FAVORITE_MOVIE_QUESTION_TEXT = "Do you want to delete the item from your favorite list?";
	}
	#endregion
}

