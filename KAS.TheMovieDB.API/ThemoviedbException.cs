using System;
using System.Net;

namespace KAS.TheMovieDB.API
{
	#region ThemoviedbException
	public class ThemoviedbException : Exception
	{
		public ThemoviedbException ()
			: base()
		{
		}

		public ThemoviedbException(string message) 
			: base(message)
		{
		}

		public ThemoviedbException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public ThemoviedbException(string message, HttpStatusCode httpStatus, long statusCode, Exception inner)
			: base(message, inner)
		{
			this.HttpStatus = httpStatus;
			this.StatusCode = statusCode;
		}

		public HttpStatusCode HttpStatus { get; private set; }

		public long StatusCode { get; private set; }
	}
	#endregion
}

