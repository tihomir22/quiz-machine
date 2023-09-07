using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MCQGameManager : MonoBehaviour {

    [System.Serializable]
    public class QuestionData
    {
        public string question;
        public string[] possibleAnswers;
        public int rightAnswer;
    }

	public class WrapperQuestionData
	{
		public QuestionData[] questionData;
	}


    private mcqquestion[] questions;   // creates an array which has a fixed size
	private static List<mcqquestion> unansweredQuestions; //creates list which changes its size during gameplay 
	private mcqquestion currentQuestion;

	public static int newhighscore;

	public Animator answers, GameOver;


	private static int totalquestionstoask = -1;     //Change this value to set how many questions you have to ask in the game.


	[SerializeField]
	private Text factText;

	[SerializeField]
	private Text option1;

	[SerializeField]
	private Text option2;

	[SerializeField]
	private Text option3;

	[SerializeField]
	private Text option4;

	[SerializeField]
	private Text answerdialogbox;

	[SerializeField]
	public float timebetweenquestions;

	[SerializeField]
	private Text counter;

	[SerializeField]
	public float timeforeachquestion;

	[SerializeField]
	private Text correctanswerstext;

	[SerializeField]
	private Text wronganswerstext;


	private static int correctanswers = 0;
	private static int wronganswers = 0;

	float end=0;
	private Coordinator coord;
    private void Awake()
    {

        // Cargar el JSON desde la carpeta Resources
        if (totalquestionstoask == -1)
        {
			this.reloadQuestions();
        }
		this.coord = new Coordinator();
		this.coord.externalSet();
    }


    /*section 1. This section selects a random question from mcqquestion.cs script along with associated 4 options
    and displays it */

    void Start (){
        GameObject targetObject = GameObject.Find("imagedisplay"); // Reemplaza con el nombre correcto
        if (targetObject != null)
        {
            // Obtiene el componente "Image" del GameObject encontrado
            Image imageComponent = targetObject.GetComponent<Image>();

            if (imageComponent != null)
            {
				imageComponent.sprite = RandomImageLoader.DisplayRandomImage();
            }
            else
            {
                Debug.LogError("El componente 'Image' no se encontró en el GameObject.");
            }
        }

		
        if (totalquestionstoask > 0) {
			SetCurrentQuestion ();
		} 

		if (totalquestionstoask == 0) {
		
			stopgame ();
		
		};
    }

	void SetCurrentQuestion(){

		int randomQuestionIndex = Random.Range (0, unansweredQuestions.Count);
		currentQuestion = unansweredQuestions [randomQuestionIndex];

		factText.text = currentQuestion.mcq;
		option1.text = currentQuestion.option1;
		option2.text = currentQuestion.option2;
		option3.text = currentQuestion.option3;
		option4.text = currentQuestion.option4;

	}

	//Section 1.1 
	// In this section we set the timer clock and logic behind it

	void Update(){


		if (end == 1 ) {
		
			return;
		} 

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

	//end of section1.

	// section 2. this section is to show if the user's selected choice is correct or wrong


	public void retry()
	{
		correctanswers = 0;
		wronganswers = 0;
		this.reloadQuestions();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void reloadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("multiple-questions");
        if (jsonFile != null)
        {
            WrapperQuestionData questionData = JsonUtility.FromJson<WrapperQuestionData>(jsonFile.text);
            List<mcqquestion> questionsTmp = new List<mcqquestion>();

            foreach (QuestionData question in questionData.questionData)
            {
                mcqquestion newQuestion = new mcqquestion();
                newQuestion.mcq = question.question;
                newQuestion.option1 = question.possibleAnswers[0];
                newQuestion.option2 = question.possibleAnswers[1];
                newQuestion.option3 = question.possibleAnswers[2];
                newQuestion.option4 = question.possibleAnswers[3];
                newQuestion.atrue = question.rightAnswer == 0;
                newQuestion.btrue = question.rightAnswer == 1;
                newQuestion.ctrue = question.rightAnswer == 2;
                newQuestion.dtrue = question.rightAnswer == 3;
                questionsTmp.Add(newQuestion);
            }
            questions = questionsTmp.ToArray();
            unansweredQuestions = questions.ToList<mcqquestion>();
            totalquestionstoask = questions.Length;
        }
        else
        {
            Debug.LogError("No se pudo cargar el archivo JSON.");
        }
    }

    public void option1selected(){

		if (currentQuestion.atrue) {

			answerdialogbox.text = "CORRECT";
			correctanswers = correctanswers + 1;
		} 

		else 
		{
		
			answerdialogbox.text = "WRONG";
			wronganswers = wronganswers + 1;
		}
			
		answers.SetTrigger ("mcqanswershow");
		StartCoroutine (TransitionToNextQuestion ());
	}
		

	public void option2selected(){

		if (currentQuestion.btrue) {

			answerdialogbox.text = "CORRECT";
			correctanswers = correctanswers + 1;
		} 

		else 
		{

			answerdialogbox.text = "WRONG";
			wronganswers = wronganswers + 1;

		}

		answers.SetTrigger ("mcqanswershow");
		StartCoroutine (TransitionToNextQuestion ());
	}

	public void option3selected(){

		if (currentQuestion.ctrue) {

			answerdialogbox.text = "CORRECT";
			correctanswers = correctanswers + 1;
		} 

		else 
		{

			answerdialogbox.text = "WRONG";
			wronganswers = wronganswers + 1;

		}


		answers.SetTrigger ("mcqanswershow");
		StartCoroutine (TransitionToNextQuestion ());
	}

	public void option4selected(){

		if (currentQuestion.dtrue) {

			answerdialogbox.text = "CORRECT";
			correctanswers = correctanswers + 1;
		} 

		else 
		{
			answerdialogbox.text = "WRONG";
			wronganswers = wronganswers + 1;

		}
			
		answers.SetTrigger ("mcqanswershow");
		StartCoroutine (TransitionToNextQuestion ());
	}
		

	//end of section 2.

	//section 3. This section sets the time delay between questions and reloads the scene after that time to show next question.

	IEnumerator TransitionToNextQuestion()
	{

		unansweredQuestions.Remove(currentQuestion);
		totalquestionstoask = totalquestionstoask - 1;
		yield return new WaitForSeconds (timebetweenquestions);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void skip(){
	
		unansweredQuestions.Remove(currentQuestion);
		totalquestionstoask = totalquestionstoask - 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	
	}


	//section 4. Displays results of game

	void stopgame(){

		end = 1;
		factText.text = "END OF QUESTIONS";
		correctanswerstext.text = correctanswers.ToString ();
		wronganswerstext.text = wronganswers.ToString ();


		GameOver.SetTrigger ("mcqover");

		sethighscores ();
		this.coord.displayInterstitial();
	}
		

	void sethighscores(){


		if (PlayerPrefs.GetString ("Category") == "mcqMovies") {

			int newhighscore = correctanswers;
			int oldhighscore = PlayerPrefs.GetInt ("mcqMoviesHighScore", 0);

			if (newhighscore > oldhighscore) {

				PlayerPrefs.SetInt ("mcqMoviesHighScore", newhighscore);
				PlayerPrefs.Save ();

			}
		}


		reset();
	}
	// Reset all variables to initial values

	void reset(){

		totalquestionstoask = -1; //change this value to initial value
		correctanswers = 0;
		wronganswers = 0;
		unansweredQuestions = null;
		questions = null;
	}


	string ReplaceSpace (string val) {
		return val.Replace(" ", "%20");
	}

	public void cancelquiz(){

		totalquestionstoask = -1; //change this value to initial value
		correctanswers = 0;
		wronganswers = 0;
		unansweredQuestions = null;
		questions = null;
		SceneManager.LoadScene ("start");


	}
}
