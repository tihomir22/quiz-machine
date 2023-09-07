using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Stats
{
    public int base_reward;
    public int minimum_gwei;
    public string dynamic_key;
}

public class ChainAvalaible
{
    public decimal minWithdrawal;
    public string placeholder;
    public string symbol;
    public string unitName;
    public int decimals;
    public bool disabled;
    public float baseReward;
}

public class Withdraw
{
    public decimal weiRequested;
    public string network;
}

public class User
{
    public string sub;
    public string email;
    public int role_code;
}

public class GameProgress
{
    public string userId;
    public string game;
    public string sub;
    public string playerPrefsAsProgress;
}

public class APIComunication : MonoBehaviour
{
    // Start is called before the first frame update
    public static APIComunication instance;
    public int base_reward = 100;
    public int minimum_gwei = 0;
    public string dynamic_key = "TU_QUIERE_MI_P_EN_TU_C";
    public ChainAvalaible[] avaibleChains;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    void Start()
    {
        StartCoroutine(this.obtainStats());
        StartCoroutine(this.obtainChains());
        
    }

    public void externalSet()
    {
        StartCoroutine(this.obtainStats());
        StartCoroutine(this.obtainChains());
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerPrefs.HasKey("TIMESTAMP_ID"))
        {
            var timeStamp = System.DateTimeOffset.Now.ToUnixTimeSeconds();
            PlayerPrefs.SetString("TIMESTAMP_ID", timeStamp + "");
        }
    }

    public IEnumerator obtainStats()
    {
        UnityWebRequest www = UnityWebRequest.Get(Coordinator.HOST + "/stats");
        yield return www.SendWebRequest();
        if (!www.isNetworkError)
        {
            var data = JsonConvert.DeserializeObject<Stats>(www.downloadHandler.text);
            try
            {
                this.base_reward = Int32.Parse(data.base_reward+"");
                this.minimum_gwei = Int32.Parse(data.minimum_gwei+"");
                this.dynamic_key = data.dynamic_key;
                if (!PlayerPrefs.HasKey("FirstTime"))
                {
                    PlayerPrefs.SetString("FirstTime", Crypto.encryptData(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss").ToString(), this.dynamic_key));
                }
                if (PlayerPrefs.HasKey("gwei_amount"))
                {
                    PlayerPrefs.SetString("ETH_AMOUNT", Crypto.encryptData(PlayerPrefs.GetString("gwei_amount"), APIComunication.instance.dynamic_key));
                    PlayerPrefs.DeleteKey("gwei_amount");
                }

            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
                this.base_reward = 1;
                this.minimum_gwei = 10000000;
                this.dynamic_key = "TU_QUIERE_MI_P_EN_TU_C";
            }
        }
    }

    public IEnumerator obtainChains()
    {
        UnityWebRequest www = UnityWebRequest.Get(Coordinator.HOST+"/chainsAvalaible");
        yield return www.SendWebRequest();
        if (!www.isNetworkError)
        {
            var data = JsonConvert.DeserializeObject<ChainAvalaible[]>(www.downloadHandler.text);
            try
            {
                this.avaibleChains = data;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public ChainAvalaible findBySymbolActiveChain(string symbol)
    {
        ChainAvalaible activeChain = null;
        if(avaibleChains == null || avaibleChains.Length == 0)
        {
            return null;
        }
        if (!PlayerPrefs.HasKey("SELECTED_CHAIN"))
        {
            for (int i = 0; i < instance.avaibleChains.Length; i++)
            {
                var activeChainLoop = avaibleChains[i];
                if (activeChainLoop.symbol == "DOGE")
                {
                    PlayerPrefs.SetString("SELECTED_CHAIN", activeChainLoop.symbol);
                    return activeChainLoop;
                }
            }
        }
        for (int i = 0; i < avaibleChains.Length; i++)
        {
            if (avaibleChains[i].symbol == symbol)
            {
                activeChain = avaibleChains[i];
                return activeChain;
            }
        }
        return activeChain;
    }


    public IEnumerator createWithdrawFramework() {
        Debug.Log(PlayerPrefs.GetString("TIMESTAMP_ID"));
        Debug.Log(Coordinator.HOST);
        IEnumerator isUserLoggedIn = instance.isUserLoggedIn();
        yield return isUserLoggedIn;
        string sub = "NON_LOGGED";
        var data = JsonConvert.DeserializeObject<User>(isUserLoggedIn.Current.ToString());
        if (isUserLoggedIn!=null && data.sub != null)
        {
            sub = data.sub;
        }
        UnityWebRequest www = UnityWebRequest.Post(Coordinator.HOST+  "/withdraws/createWithdrawFramework/"+ PlayerPrefs.GetString("TIMESTAMP_ID"), this.getFormMetadata(sub));
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log("Error");
            Debug.LogError("ERROR " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            yield return www.downloadHandler.text;
        }
    }

    public IEnumerator createLoginFramework()
    {
        Debug.Log(PlayerPrefs.GetString("TIMESTAMP_ID"));
        Debug.Log(Coordinator.HOST);
        WWWForm form = new WWWForm();
        form.AddField("game", Application.productName);
        UnityWebRequest www = UnityWebRequest.Post(Coordinator.HOST + "/login/createLoginFramework/" + PlayerPrefs.GetString("TIMESTAMP_ID"), form);
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log("Error");
            Debug.LogError("ERROR " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            yield return www.downloadHandler.text;
        }
    }

    public IEnumerator isUserLoggedIn()
    {
        Debug.Log(PlayerPrefs.GetString("TIMESTAMP_ID"));
        Debug.Log(Coordinator.HOST);
        UnityWebRequest www = UnityWebRequest.Get(Coordinator.HOST+ "/login/isUserLoggedIn/" + PlayerPrefs.GetString("TIMESTAMP_ID") + "/" + Uri.EscapeDataString(Application.productName));
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.responseCode != 200)
        {
            Debug.Log("Error");
            Debug.LogError("ERROR " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            yield return www.downloadHandler.text;
        }
    }

    public IEnumerator getSubstractableWithdraws()
    {
        UnityWebRequest www = UnityWebRequest.Get(Coordinator.HOST + "/getSubstractableWithdraws/" + PlayerPrefs.GetString("TIMESTAMP_ID")+"/"+Uri.EscapeDataString(Application.productName));
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("ERROR " + www.error);
        }
        else
        {
            yield return www.downloadHandler.text;
        }
    }
    public IEnumerator saveProgress()
    {
        IEnumerator isUserLoggedIn = instance.isUserLoggedIn();
        yield return isUserLoggedIn;
        var data = JsonConvert.DeserializeObject<User>(isUserLoggedIn.Current.ToString());
        if (isUserLoggedIn != null && data.sub != null)
        {
            var sub = data.sub;
            UnityWebRequest www = UnityWebRequest.Post(Coordinator.HOST + "/saveProgress/", instance.getFormMetadata(sub));
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log("Error");
                Debug.LogError("ERROR " + www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                yield return www.downloadHandler.text;
            }
        }
    }

    public IEnumerator getProgress()
    {
        IEnumerator isUserLoggedIn = instance.isUserLoggedIn();
        yield return isUserLoggedIn;
        var data = JsonConvert.DeserializeObject<User>(isUserLoggedIn.Current.ToString());
        if (isUserLoggedIn != null && data.sub != null)
        {
            UnityWebRequest www = UnityWebRequest.Get(Coordinator.HOST + "/getSavedProgress/"+ Uri.EscapeDataString(Application.productName)+"/"+Uri.EscapeDataString(data.sub));
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log("Error");
                Debug.LogError("ERROR " + www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                var deserializedFullObject = JsonConvert.DeserializeObject<GameProgress>(www.downloadHandler.text);
                this.setProgress(deserializedFullObject);
                yield return www.downloadHandler.text;

            }
        }
    }

    private void setProgress(GameProgress gameProgress)
    {
        var deserializedPlayerPrefs = JsonConvert.DeserializeObject(gameProgress.playerPrefsAsProgress);
        JObject jsonObject = JObject.FromObject(deserializedPlayerPrefs);

        // Acceder a los valores dentro de "DynamicProperties"
        JObject dynamicProperties = jsonObject["DynamicProperties"] as JObject;

        if (dynamicProperties != null)
        {
            // Iterar a través de las propiedades anidadas dentro de "DynamicProperties"
            foreach (var property in dynamicProperties.Properties())
            {
                string propertyName = property.Name;
                string propertyValue = property.Value.ToString();
                PlayerPrefs.SetString(propertyName, propertyValue);
                Debug.Log($"Property: {propertyName}, Value: {propertyValue}");
            }
        }
    }


    private class PlayerPrefsData
    {
        public int UnityGraphicsQuality;
        public string unity_player_session_count;
        public string unity_player_sessionid;
        public string unity_cloud_userid;
        public int NrOfSessions;
        public int TimeSinceOpen;
        public int TimeSinceStart;
        public int FirstShow;
        public int NrOfEvents;
        public string DIFFICULTY;
        public string SHOWN_INTERSTITIAL;
        public string SHOWN_REWARDED;
        public string VERSION;
        public string UNITY_VERSION;
        public int TIME_DISABLED;
        //public List<KeyValuePair<string, string>> DynamicProperties = new List<KeyValuePair<string, string>>();
        public Dictionary<string, object> DynamicProperties = new Dictionary<string, object>();
        // Otros campos de PlayerPrefs...
        public string HAS_REMOVED_ADS;
    }

        public WWWForm getFormMetadata(string subUser)
    {
        var selectedChain = findBySymbolActiveChain(PlayerPrefs.GetString("SELECTED_CHAIN")); ;

        WWWForm form = new WWWForm();
        form.AddField("sub", subUser);
        form.AddField("game", Application.productName);
        form.AddField("transactionHash", "");
        form.AddField("network", selectedChain.symbol);
        form.AddField("unityId", PlayerPrefs.GetString("TIMESTAMP_ID"));

        //IDictionary<string, object> objPlayerPrefs = new System.Dynamic.ExpandoObject();
        var objPlayerPrefsClass = new PlayerPrefsData();
        objPlayerPrefsClass.unity_player_session_count = PlayerPrefs.GetString("unity.player_session_count");
        objPlayerPrefsClass.unity_player_sessionid = PlayerPrefs.GetString("unity.player_sessionid");
        objPlayerPrefsClass.unity_cloud_userid = PlayerPrefs.GetString("unity.cloud_userid");
        objPlayerPrefsClass.NrOfSessions = PlayerPrefs.GetInt("NrOfSessions");
        objPlayerPrefsClass.TimeSinceOpen = PlayerPrefs.GetInt("TimeSinceOpen");
        objPlayerPrefsClass.TimeSinceStart = PlayerPrefs.GetInt("TimeSinceStart");
        objPlayerPrefsClass.TimeSinceStart = PlayerPrefs.GetInt("TimeSinceStart");
        objPlayerPrefsClass.FirstShow = PlayerPrefs.GetInt("FirstShow");
        objPlayerPrefsClass.NrOfEvents = PlayerPrefs.GetInt("NrOfEvents");
        objPlayerPrefsClass.DIFFICULTY = PlayerPrefs.GetString("DIFFICULTY");
        objPlayerPrefsClass.SHOWN_INTERSTITIAL = PlayerPrefs.GetString("SHOWN_INTERSTITIAL");
        objPlayerPrefsClass.SHOWN_REWARDED = PlayerPrefs.GetString("SHOWN_REWARDED");
        objPlayerPrefsClass.VERSION = Application.version;
        objPlayerPrefsClass.UNITY_VERSION = Application.unityVersion;
        objPlayerPrefsClass.TIME_DISABLED = PlayerPrefs.GetInt("TIME_DISABLED");


        for (int i = 0; i < 100; i++)
        {
            if (PlayerPrefs.HasKey("LEVEL_" + (i + 1)))
            {
                objPlayerPrefsClass.DynamicProperties["LEVEL_" + (i + 1)] = PlayerPrefs.GetString("LEVEL_" + (i + 1));
                string easy = "LEVEL_" + (i + 1) + "-EASY";
                string medium = "LEVEL_" + (i + 1) + "-MEDIUM";
                string hard = "LEVEL_" + (i + 1) + "-HARD";
                if (PlayerPrefs.HasKey(easy)) {
                    objPlayerPrefsClass.DynamicProperties[easy] = PlayerPrefs.GetString(easy);
                };
                if (PlayerPrefs.HasKey(medium)) {
                     objPlayerPrefsClass.DynamicProperties[medium] = PlayerPrefs.GetString(medium); 
                };
                if (PlayerPrefs.HasKey(hard)) {
                    objPlayerPrefsClass.DynamicProperties[hard] = PlayerPrefs.GetString(hard);
                };
            }
        }
        for (int i = 0; i < avaibleChains.Length; i++)
        {
            var currentLoopedChain = avaibleChains[i];
            if (PlayerPrefs.HasKey(currentLoopedChain.symbol + "_AMOUNT"))
            {
                //objPlayerPrefsClass.DynamicProperties.Add(new KeyValuePair<string, string>(currentLoopedChain.symbol + "_AMOUNT", PlayerPrefs.GetString(currentLoopedChain.symbol + "_AMOUNT")));
               objPlayerPrefsClass.DynamicProperties[currentLoopedChain.symbol + "_AMOUNT"] = PlayerPrefs.GetString(currentLoopedChain.symbol + "_AMOUNT");
            }
        }
        if (PlayerPrefs.HasKey("anuncios_eliminados"))
        {
            objPlayerPrefsClass.HAS_REMOVED_ADS = PlayerPrefs.GetString("anuncios_eliminados");
        }
        else
        {
            objPlayerPrefsClass.HAS_REMOVED_ADS = "NOPE";
        }
        string serialized = JsonConvert.SerializeObject(objPlayerPrefsClass);
        Debug.Log(serialized);
        //string serialized = JsonConvert.SerializeObject(objPlayerPrefs);
        //ebug.Log(serialized);
        form.AddField("playerPrefsOnRequest", serialized);
        //Debug.Log("Checkpoint 4");
        return form;
    }

   
}
