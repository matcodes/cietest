package md5c1aac12613abab58eda1f84b13147bd3;


public class MovieActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("KAS.TheMovieDB.MovieActivity, KAS.Themoviedb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MovieActivity.class, __md_methods);
	}


	public MovieActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MovieActivity.class)
			mono.android.TypeManager.Activate ("KAS.TheMovieDB.MovieActivity, KAS.Themoviedb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
