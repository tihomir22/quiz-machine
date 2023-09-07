using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class HighScores : MonoBehaviour {

	[SerializeField]
	private Text tfmoviesHighscore;


	[SerializeField]
	private Text mcqmoviesHighscore;

	void Start(){

		tfmoviesHighscore.text = PlayerPrefs.GetInt ("TFMoviesHighScore").ToString ();
	
		mcqmoviesHighscore.text = PlayerPrefs.GetInt ("mcqMoviesHighScore").ToString ();
		
	

	}
}