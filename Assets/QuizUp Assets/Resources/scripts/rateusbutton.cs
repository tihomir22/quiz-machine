using UnityEngine;
using System.Collections;

public class rateusbutton : MonoBehaviour {

	
  public void rateus(){

         // place link to your playstore game
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.OrimGames.WhataboutOppenheimerQuiz");


  }

    public void twitter()
    {
        Application.OpenURL("https://twitter.com/OrimGames");
    }

}
