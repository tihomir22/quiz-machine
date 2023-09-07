using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class randomquizmanager : MonoBehaviour {

	public randomquiz[] questions;   // creates an array which has a fixed size
	private static List<randomquiz> unansweredQuestions; //creates list which changes its size during gameplay 
	private randomquiz currentQuestion;
	private static int correctanswers = 0, wronganswers = 0;
	private static int totalquestionstoask = 10, end = 0;


	public float timeforeachquestion;
	public Text factText, option1, option2, option3, option4, answersshow, counter, highscore, showscore;
	public Image imageholder;
	public GameObject option1btn, option2btn, option3btn, option4btn, imagedisplay;

	public Animator answersanim, gameend;

	void Start () {
	
		highscore.text = "Best : "+ PlayerPrefs.GetInt ("RandomquizHighScore").ToString ();

		if (unansweredQuestions == null || unansweredQuestions.Count == 0) 
		{

			unansweredQuestions = questions.ToList<randomquiz>();
		}

		if (totalquestionstoask > 0)
			SetCurrentQuestion ();
		else if (totalquestionstoask == 0)
			endgame ();
	}
	

	void Update () {
	
		if (end == 1)
			return;

		if (end == 0) 
		{
			timeforeachquestion = timeforeachquestion - Time.deltaTime;
			counter.text = timeforeachquestion.ToString ("F1");
		}

		if (timeforeachquestion < 0.0f)
		{
			unansweredQuestions.Remove(currentQuestion);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			totalquestionstoask = totalquestionstoask - 1;
		}

	}

	private void SetCurrentQuestion(){

			int randomQuestionIndex = Random.Range (0, unansweredQuestions.Count);
			currentQuestion = unansweredQuestions [randomQuestionIndex];
		    factText.text = currentQuestion.question;
		    option1.text = currentQuestion.option1;
		    option2.text = currentQuestion.option2;
		    option3.text = currentQuestion.option3;
		    option4.text = currentQuestion.option4;
		    

		if (currentQuestion.istruefalse) {

			option3btn.SetActive (false);
			option4btn.SetActive (false);
			imagedisplay.SetActive (false);
		}
		    
		if (currentQuestion.ismcq) {
			option3btn.SetActive (true);
			option4btn.SetActive (true);
			imagedisplay.SetActive (false);
		}
			
		if (currentQuestion.ispic) {

			imageholder.sprite = currentQuestion.picture;
			option3btn.SetActive (true);
			option4btn.SetActive (true);
		}


			imageholder.sprite = currentQuestion.picture;

	}

	public void option1selected(){
	
		if (currentQuestion.atrue) 
		{
			correctanswers++;
			answersshow.text = "CORRECT";

		} else {

			wronganswers++;
			answersshow.text = "FALSE";
		}
	
		answersanim.SetTrigger ("mcqanswershow");
		StartCoroutine (transitiontonextquestion ());
	
	}

	public void option2selected(){

		if (currentQuestion.btrue) 
		{
			correctanswers++;
			answersshow.text = "CORRECT";

		} else {

			wronganswers++;
			answersshow.text = "FALSE";
		}

		answersanim.SetTrigger ("mcqanswershow");
		StartCoroutine (transitiontonextquestion ());
	}


	public void option3selected(){

		if (currentQuestion.ctrue) 
		{
			correctanswers++;
			answersshow.text = "CORRECT";

		} else {

			wronganswers++;
			answersshow.text = "FALSE";
		}

		answersanim.SetTrigger ("mcqanswershow");
		StartCoroutine (transitiontonextquestion ());
	}

	public void option4selected(){

		if (currentQuestion.dtrue) 
		{
			correctanswers++;
			answersshow.text = "CORRECT";

		} else {

			wronganswers++;
			answersshow.text = "FALSE";
		}

		answersanim.SetTrigger ("mcqanswershow");
		StartCoroutine (transitiontonextquestion ());
	}

	IEnumerator transitiontonextquestion(){

		unansweredQuestions.Remove(currentQuestion);
		totalquestionstoask = totalquestionstoask - 1;
		yield return new WaitForSeconds (0.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

	public void skip(){

		unansweredQuestions.Remove(currentQuestion);
		totalquestionstoask = totalquestionstoask - 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

	public void retry(){
	
		end = 0;
		totalquestionstoask = 10; //change this value to initial value
		correctanswers = 0;
		wronganswers = 0;
		unansweredQuestions = null;
		questions = null;

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	
	}

	public void cancelquiz(){

		end = 0;
		totalquestionstoask = 10; //change this value to initial value
		correctanswers = 0;
		wronganswers = 0;
		unansweredQuestions = null;
		questions = null;

		SceneManager.LoadScene ("start");

	}

	private void endgame(){

		int newscore = correctanswers;
		int oldscore = PlayerPrefs.GetInt ("RandomquizHighScore", 0);

		if (newscore > oldscore) {
			
			PlayerPrefs.SetInt ("RandomquizHighScore", newscore);
			PlayerPrefs.Save ();
		}

		showscore.text = "SCORE : "+ correctanswers.ToString ();
		gameend.SetTrigger ("mcqover");
		reset ();
		end = 1;
	}

	private void reset(){
	
		totalquestionstoask = 10; //change this value to initial value
		correctanswers = 0;
		wronganswers = 0;
		unansweredQuestions = null;
		questions = null;
	}


}
