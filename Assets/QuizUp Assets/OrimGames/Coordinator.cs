using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using Hellmade.Net;

public class Coordinator : MonoBehaviour
{
    public AdControllerInterstitial adControllerInterstitial;
    public AdControllerInterstitial adControllerVideo;
    public static string FRONTEND_WEB = "https://orimgames.com";
    public static string HOST = "https://api.orimgames.com"; // https://api.orimgames.com
    public static string FRONTEND_DASHBOARD = "https://webapp.orimgames.com";//"https://webapp.orimgames.com";
    // Start is called before the first frame update
    void Start()
    {
        this.adControllerInterstitial = new AdControllerInterstitial();
        this.adControllerInterstitial.initialize();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void externalSet()
    {
        Start();
    }

    public bool hasConnection()
    {
        return EazyNetChecker.Status == NetStatus.Connected || EazyNetChecker.Status == NetStatus.PendingCheck;
    }

    public void displayInterstitial()
    {
        if (!PlayerPrefs.HasKey("anuncios_eliminados") || PlayerPrefs.GetString("anuncios_eliminados") != SystemInfo.deviceUniqueIdentifier)
        {
            int random = (int)Random.Range(0, 100);
            if(random <= 70) { 
                this.adControllerInterstitial.ShowAd();
            }
        }
    }

    public void displayVideo()
    {
        this.adControllerVideo.ShowAd();
    }



    public ChainAvalaible getActiveChain()
    {
        return APIComunication.instance.findBySymbolActiveChain(PlayerPrefs.GetString("SELECTED_CHAIN"));
    }

    public static void reduceAmount(string symbol,decimal amount)
    {
        var currentAmount = Coordinator.getCryptoAmount(symbol);
        currentAmount = currentAmount - amount;
        if(currentAmount <0)
        {
            currentAmount = 0;
        }
        setCryptoAmount(symbol, (float)currentAmount);
    }

    public static void setCryptoAmount(string symbol, float amount)
    {
        //CultureInfo currentCulture = System.Globalization.CultureInfo.CurrentCulture;
  //      PlayerPrefs.SetString(symbol + "_AMOUNT", Crypto.encryptData(amount.ToString("N5", currentCulture), APIComunication.instance.dynamic_key));
        PlayerPrefs.SetString(symbol + "_AMOUNT", Crypto.encryptData(amount.ToString(), APIComunication.instance.dynamic_key));
    }

    public static decimal getCryptoAmount(string symbol)
    {
        decimal currentAmount = 0;

        if (PlayerPrefs.HasKey(symbol + "_AMOUNT"))
        {
            string encryptedNumber = PlayerPrefs.GetString(symbol + "_AMOUNT");
            var decryptedNumber = Crypto.decryptData(encryptedNumber, APIComunication.instance.dynamic_key);
            currentAmount = System.Decimal.Parse(decryptedNumber);
        }
        return currentAmount;
    }

    public float getReward(int level)
    {
        if (!this.hasConnection()) { return 0; };
        ChainAvalaible activeChain = APIComunication.instance.findBySymbolActiveChain(PlayerPrefs.GetString("SELECTED_CHAIN"));
        return (float)((activeChain.baseReward * level) * 0.5);
    }

    public void updateLevelCompletedPlayerPrefs(int level)
    {
        PlayerPrefs.SetString("LEVEL_" + level, "TERMINADO");
        PlayerPrefs.SetString("LEVEL_" + level + "-MEDIUM", "TERMINADO");
    }

}
