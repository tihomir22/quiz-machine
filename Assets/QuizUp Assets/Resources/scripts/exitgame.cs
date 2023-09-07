using UnityEngine;
using System.Collections;

public class exitgame : MonoBehaviour {

	[SerializeField]
	GameObject exitpanel;


	void Update(){

		if (Input.GetKey (KeyCode.Escape)) {
		
			exitpanel.SetActive (true);
		}

	}

	public void yes(){

		Application.Quit();

	}

	public void no(){

		exitpanel.SetActive (false);
	}

}


