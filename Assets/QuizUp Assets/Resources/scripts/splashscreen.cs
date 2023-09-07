using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class splashscreen : MonoBehaviour {


	void Start () {
	
		StartCoroutine (splash());
	}

	IEnumerator splash(){
	
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("start");
	}

}
