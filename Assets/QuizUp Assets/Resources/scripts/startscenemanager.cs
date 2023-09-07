using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class startscenemanager : MonoBehaviour {

	public GameObject multiplayer_note_panel;


	void Start(){
		multiplayer_note_panel.SetActive (false);	
	}


	public void changescene(string changeto){

		SceneManager.LoadScene (changeto);

	}

	public void multiplayergame(){

		multiplayer_note_panel.SetActive (true);
	}

	public void close_multiplayernotice(){

		multiplayer_note_panel.SetActive (false);
	}


	public void share(){

		#pragma warning disable 0219
		string subject = "Share App";
		string body = "I am playing an absolutely amazing game QuizUp. Download your's now @ market://details?id=com.digiart.quizup/";

		//execute the below lines if being run on a Android device
		#if UNITY_ANDROID
		//Reference of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		//Reference of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		//start the activity by sending the intent data
		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
		currentActivity.Call("startActivity", jChooser);
		#endif

	}
}
