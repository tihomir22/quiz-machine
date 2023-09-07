using UnityEngine;
using System.Collections;

public class facebookshare : MonoBehaviour {


	// Your app’s unique identifier.
	string AppID = "1469549399961687";

	// The link attached to this post.
	public string Link;

	// The URL of a picture attached to this post. The picture must be at least 200px by 200px.
	public string Picture;

	// The name of the link attachment.
	public string Name;

	// The caption of the link (appears beneath the link name).
	public string Caption;

	// The description of the link (appears beneath the link caption).
	public string Description;

	void Start(){



	}


	public void ShareScoreOnFB(){
		Application.OpenURL("https://www.facebook.com/dialog/feed?"+ "app_id="+AppID+ "&link="+
			Link+ "&picture="+Picture+ "&name="+ReplaceSpace(Name)+ "&caption="+
			ReplaceSpace(Caption)+ "&description="+ReplaceSpace(Description)+
			"&redirect_uri=https://facebook.com/");
	}

	string ReplaceSpace (string val) {
		return val.Replace(" ", "%20");
	}
}