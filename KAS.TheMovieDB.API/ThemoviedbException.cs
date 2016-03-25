using System;
using System.Net;

namespace KAS.TheMovieDB.API
{
	#region TheMovieDBException

	/// <summary>
	/// TheMovieDB request exception.
	/// </summary>
	/// 
	public class TheMovieDBException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.API.ThemoviedbException"/> class.
		/// </summary>
		 
		public TheMovieDBException ()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.API.ThemoviedbException"/> class.
		/// </summary>
		/// <param name="message">Request error message.</param>
		 
		public TheMovieDBException(string message) 
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.API.ThemoviedbException"/> class.
		/// </summary>
		/// <param name="message">Request error message.</param>
		/// <param name="inner">Inner exception.</param>
		 
		public TheMovieDBException(string message, Exception inner)
			: base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KAS.TheMovieDB.API.ThemoviedbException"/> class.
		/// </summary>
		/// <param name="message">Request error message.</param>
		/// <param name="httpStatus">Response http status.</param>
		/// <param name="statusCode">Response status code.</param>
		/// <param name="inner">Inner exception.</param>
		 
		public TheMovieDBException(string message, HttpStatusCode httpStatus, long statusCode, Exception inner)
			: base(message, inner)
		{
			this.HttpStatus = httpStatus;
			this.StatusCode = statusCode;
		}

		/// <summary>
		/// Gets the response http status.
		/// </summary>
		/// <value>The http status.</value>
		 
		public HttpStatusCode HttpStatus { get; private set; }

		/// <summary>
		/// Gets the response status code.
		/// </summary>
		/// <value>The status code.</value>
		 
		public long StatusCode { get; private set; }
	}
	#endregion
}

