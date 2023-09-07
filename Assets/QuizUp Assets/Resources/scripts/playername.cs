using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class playername : MonoBehaviour {



	[SerializeField]
	private Text username;

	void Start (){

		username.text = "Welcome  " + PlayerPrefs.GetString ("User");
	
	}
	public void saveusername(string inputFieldString){

		username.text = "Welcome  "+ inputFieldString;
		PlayerPrefs.SetString ("User", inputFieldString);
		PlayerPrefs.Save ();

	}
		

}
