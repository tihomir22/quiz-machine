using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class categoriesscenechanger : MonoBehaviour {

	/////////////////////TF Categories ///////////////////////////////////	


	public void tfmovies(){

		PlayerPrefs.SetString ("Category", "tfMovies");
		PlayerPrefs.Save ();
		SceneManager.LoadScene("timerscene");

	}

	public void mcqMovies(){

		SceneManager.LoadScene("timerscene");
		PlayerPrefs.SetString ("Category", "mcqMovies");
		PlayerPrefs.Save ();

	}

}
