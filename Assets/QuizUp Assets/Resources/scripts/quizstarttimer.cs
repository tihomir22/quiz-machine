using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class quizstarttimer : MonoBehaviour {

	[SerializeField]
	private Text timer;


	void Start(){
		StartCoroutine (delay ());
	}


	IEnumerator delay(){
		timer.text = "3";

		yield return new WaitForSeconds (1);
		timer.text = "2";

		yield return new WaitForSeconds (1);
		timer.text = "1";

		yield return new WaitForSeconds (1);
		timer.text = "GO !";

		startquiz ();

	}
	void startquiz(){


		if (PlayerPrefs.GetString ("Category") == "tfMovies") {

			SceneManager.LoadScene ("binary_options");
		}

		//MCQS 

		if (PlayerPrefs.GetString ("Category") == "mcqMovies") {

			SceneManager.LoadScene ("multiple_options");
		}


	}
}
