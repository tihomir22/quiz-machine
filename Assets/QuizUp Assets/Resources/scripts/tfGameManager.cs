using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static MCQGameManager;

public class tfGameManager : MonoBehaviour {


    [System.Serializable]
    public class QuestionBinaryData
    {
        public string question;
        public bool isTrue;
    }

    public class WrapperQuestionBinaryData
    {
        public QuestionBinaryData[] questionData;
    }


    public tfQuestion[] questions;                       // creates an array which has a fixed size
	private static List<tfQuestion> unansweredQuestions; //creates list which changes its size during gameplay 
	private tfQuestion currentQuestion;

	public Animator gameover;

	private static int totalquestionstoask = -1;     //Change this value to set how many questions you have to ask in the game.
	private static int correctanswers = 0;
	private static int wronganswers = 0;

	[SerializeField]
	private Text factText;

	[SerializeField]
	private Text trueAnswerText;

	[SerializeField]
	private Text falseAnswerText;

	[SerializeField]
	public float timeBetweenQuestions;

	[SerializeField]
	private Text timer;

	[SerializeField]
	private Text totalcorrectanswers;

	[SerializeField]
	private Text totalwronganswers;

	[SerializeField]
	public float timeforeachquestion;

	float end = 0;
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

    void Start ()
	{

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

		if(totalquestionstoask == 0){
		
			endgame ();
		}
		}

	void Update()
	{
		if (end == 1) {
		
			return;
		}

		else
		{
			timeforeachquestion = timeforeachquestion - Time.deltaTime;
			timer.text = timeforeachquestion.ToString ("F1");        
		}

		if (timeforeachquestion < 0.0f)
		{
			unansweredQuestions.Remove(currentQuestion);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			totalquestionstoask = totalquestionstoask - 1;
		}



	}

	public Animator trueslected;
	public Animator falseselected;

	void SetCurrentQuestion(){



		int randomQuestionIndex = Random.Range (0, unansweredQuestions.Count);
		currentQuestion = unansweredQuestions [randomQuestionIndex];
		factText.text = currentQuestion.fact;


		if (currentQuestion.iSTrue) {
			trueAnswerText.text = "CORRECT";
			falseAnswerText.text = "WRONG";
		} 
		else {
		
			trueAnswerText.text = "WRONG";
			falseAnswerText.text = "CORRECT";
		}
	}



	IEnumerator TransitionToNextQuestion()
	{
	
		unansweredQuestions.Remove(currentQuestion);
		totalquestionstoask = totalquestionstoask - 1;
		yield return new WaitForSeconds (timeBetweenQuestions);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}



	public void userSelectTrue(){
	
		trueslected.SetTrigger ("trueselect");

		if (currentQuestion.iSTrue) 
		{
			Debug.Log ("Correct");
			correctanswers = correctanswers + 1;


		} 

		else {
			Debug.Log ("Wrong");
			wronganswers = wronganswers + 1;



		}
		StartCoroutine (TransitionToNextQuestion ());
	
	}

	public void userSelectFalse(){

		falseselected.SetTrigger ("falsesel");

		if (currentQuestion.iSTrue) 
		{
			Debug.Log ("Wrong");
			wronganswers = wronganswers + 1;
		} 

		else {
			Debug.Log ("Correct");
			correctanswers = correctanswers + 1;
		}

		StartCoroutine (TransitionToNextQuestion ());
	}

    private void reloadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("binary-questions");
        if (jsonFile != null)
        {
            WrapperQuestionBinaryData questionData = JsonUtility.FromJson<WrapperQuestionBinaryData>(jsonFile.text);
            List<tfQuestion> questionsTmp = new List<tfQuestion>();

            foreach (QuestionBinaryData question in questionData.questionData)
            {
                tfQuestion newQuestion = new tfQuestion();
				newQuestion.fact = question.question;
				newQuestion.iSTrue = question.isTrue;
                questionsTmp.Add(newQuestion);
            }
            questions = questionsTmp.ToArray();
            unansweredQuestions = questions.ToList<tfQuestion>();
            totalquestionstoask = questions.Length;
        }
        else
        {
            Debug.LogError("No se pudo cargar el archivo JSON.");
        }
    }

    public void retry()
    {
        correctanswers = 0;
        wronganswers = 0;
        this.reloadQuestions();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void skip(){

		unansweredQuestions.Remove(currentQuestion);
		totalquestionstoask = totalquestionstoask - 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
		

	void endgame(){
		end = 1;
		factText.text = "GAME OVER";
		timer.text = "5";
		totalcorrectanswers.text = correctanswers.ToString ();
		totalwronganswers.text = wronganswers.ToString ();



		gameover.SetTrigger ("over");
		sethighscores ();
        this.coord.displayInterstitial();

    }
		

	void sethighscores(){
	
	

		if (PlayerPrefs.GetString ("Category") == "tfMovies") {

			int newhighscore = correctanswers;
			int oldhighscore = PlayerPrefs.GetInt ("TFMoviesHighScore", 0);

			if (newhighscore > oldhighscore) {

				PlayerPrefs.SetInt ("TFMoviesHighScore", newhighscore);
				PlayerPrefs.Save ();

			}
		}


		resetvariables ();
	
	}
	void resetvariables(){

		totalquestionstoask = -1; //change this to initial value
		correctanswers = 0;
		wronganswers = 0;
		unansweredQuestions = null;
		questions = null;
	}



	public void cancelquiz(){

		totalquestionstoask = -1; //change this to initial value
		correctanswers = 0;
		wronganswers = 0;
		unansweredQuestions = null;
		questions = null;
		SceneManager.LoadScene ("start");

	}
		
}
