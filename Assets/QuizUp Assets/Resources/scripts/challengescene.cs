using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class challengescene : MonoBehaviour {

	//Element 0 of this array is level 1. Element 1 is level 2 and sO on...

	public challengemanager[] level;
	public Text infofield;
	public GameObject infopanel;

	void Start ()
	{
		int unlockedlevels = PlayerPrefs.GetInt ("levelsunlocked");

		for (int levelsindex = 0; levelsindex < unlockedlevels; levelsindex++) {
		
			level [levelsindex].unlockedobject.SetActive (true);

		}
		     
		for (int levelsindex = 0; levelsindex < unlockedlevels; levelsindex++) {
			
				int actuallevel = levelsindex +1;       //this is done because 0 is the base index of array but we want to assign stars to actual level which is 1

				if (PlayerPrefs.GetInt ("level" + actuallevel + "stars") == 1) {

					level [levelsindex].onestar.SetActive (true);
				}

				if (PlayerPrefs.GetInt ("level" + actuallevel + "stars") == 2) {

					level [levelsindex].twostar.SetActive (true);
				}

				if (PlayerPrefs.GetInt ("level" + actuallevel + "stars") == 3) {

					level [levelsindex].threestar.SetActive (true);
				}
			
			}
			
	}

	public void cancellevel(){
	
		infopanel.SetActive (false);
	
	}

	public void level1info(){

		infofield.text = level[0].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 1");
		PlayerPrefs.Save ();
	}

	public void level2info(){

		infofield.text = level[1].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 2");
		PlayerPrefs.Save ();
	}

	public void level3info(){

		infofield.text = level[2].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 3");
		PlayerPrefs.Save ();
	}

	public void level4info(){

		infofield.text = level[3].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 4");
		PlayerPrefs.Save ();
	}


	public void level5info(){

		infofield.text = level[4].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 5");
		PlayerPrefs.Save ();
	}


	public void level6info(){

		infofield.text = level[5].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 6");
		PlayerPrefs.Save ();
	}


	public void level7info(){

		infofield.text = level[6].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 7");
		PlayerPrefs.Save ();
	}

	public void level8info(){

		infofield.text = level[7].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 8");
		PlayerPrefs.Save ();
	}


	public void level9info(){

		infofield.text = level[8].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 9");
		PlayerPrefs.Save ();
	}

	public void level10info(){

		infofield.text = level[9].challengeinfo;
		infopanel.SetActive (true);
		PlayerPrefs.SetString ("challengelevel", "level 10");
		PlayerPrefs.Save ();
	}


	public void loadlevel(){

		string leveltoload = PlayerPrefs.GetString ("challengelevel");
		SceneManager.LoadScene (leveltoload);
	}


}
