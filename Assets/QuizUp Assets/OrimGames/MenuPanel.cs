// /*
// Created by Darsan
// */

using Newtonsoft.Json;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuPanel : MonoBehaviour
    {

        private bool userHasLogged = false;

        [System.Serializable]
        public class GameData
        {
            public string _id;
            public string game;
        }

        public class IdData
        {
            public string _id;
        }

        private void Start()
        {

            //Coordinator.setCryptoAmount("DOGE", 1000);
        }

        public void OnClickPlay()
        {
        }

        public void OnClickExit()
        {

        }

        public void onClickOptions()
        {
            SceneManager.LoadScene(3);
        }

        public void goMenu()
        {
            SceneManager.LoadScene(1);
        }

        public void onWithdrawClickWrapper()
        {
            StartCoroutine(onWithdrawClick());
        }

        public void onLoginClickWrapper()
        {
            StartCoroutine(onLoginClick());
        }

        public IEnumerator onWithdrawClick()
        {
            string unityId =  PlayerPrefs.GetString("TIMESTAMP_ID");
            string game = Application.productName;
            IEnumerator createWithdrawCoroutine = APIComunication.instance.createWithdrawFramework();
            yield return StartCoroutine(createWithdrawCoroutine);
            GameData data = JsonUtility.FromJson<GameData>(createWithdrawCoroutine.Current.ToString());
            Application.OpenURL(Coordinator.FRONTEND_WEB+ "/withdraws.html?id=" + data._id+"&game="+ Uri.EscapeDataString(game) + "&unityId="+unityId);
        }

        public IEnumerator onLoginClick()
        {
            IEnumerator createLoginWithdrawCoroutine = APIComunication.instance.createLoginFramework();
            yield return StartCoroutine(createLoginWithdrawCoroutine);
            IdData data = JsonUtility.FromJson<IdData>(createLoginWithdrawCoroutine.Current.ToString());

            if (this.userHasLogged)
            {
                Application.OpenURL(Coordinator.FRONTEND_DASHBOARD);
            }
            else {
                Debug.Log(Coordinator.FRONTEND_DASHBOARD + "/unity/autologin/" + data._id);
                Application.OpenURL(Coordinator.FRONTEND_DASHBOARD + "/unity/autologin/"+data._id);
            }
        }

        void OnApplicationFocus(bool hasFocus)
        {
            StartCoroutine(CheckClosedFrameworks());
            StartCoroutine(checkIfUserIsLoggedIn());
        }
        IEnumerator CheckClosedFrameworks()
        {
            IEnumerator allClosedFrameworksCoroutine = APIComunication.instance.getSubstractableWithdraws();
            yield return allClosedFrameworksCoroutine;
            var data = JsonConvert.DeserializeObject<Withdraw[]>(allClosedFrameworksCoroutine.Current.ToString());
            foreach (Withdraw withdraw in data)
            {
                Coordinator.reduceAmount(withdraw.network, withdraw.weiRequested);
            }
        }

        IEnumerator checkIfUserIsLoggedIn()
        {
            IEnumerator isUserLoggedIn = APIComunication.instance.isUserLoggedIn();
            yield return isUserLoggedIn;
            
            var login = GameObject.Find("Login");
            var loginText = GameObject.Find("LOGIN_Text").GetComponent<TMP_Text>();
            var loginImage = GameObject.Find("LOGIN_PlayIcon").GetComponent<Image>();
            var loginSubtitle = GameObject.Find("LOGIN_Subtitle").GetComponent<TMP_Text>();

            var textToSet = "Login";
            var subtitleToSet = "Your progress is saved locally!";
            this.userHasLogged = false;
            try
            {
                if (isUserLoggedIn != null)
                {
                    var data = JsonConvert.DeserializeObject<User>(isUserLoggedIn.Current.ToString());
                    textToSet = data.email.Split("@")[0];
                    subtitleToSet = "Your progress will be saved on the cloud!";
                    this.userHasLogged = true;
                    StartCoroutine(APIComunication.instance.getProgress());
                }
                else
                {
                    Debug.Log("User not logged in catched error");
                }
            }catch (Exception ex)
            {
                Debug.Log(ex);
            }
           

            if (login != null && loginText != null)
            {
                loginText.text = textToSet;
                loginSubtitle.text = subtitleToSet;
            }
          

        }

        public void openStore()
        {
            Application.OpenURL("https://play.google.com/store/apps/dev?id=9205738346587560692");
        }

        public void openTwitter()
        {
            Application.OpenURL("https://twitter.com/OrimGames");
        }
    }
}