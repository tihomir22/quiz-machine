using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdControllerInterstitial : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
     string adUnitId = "Interstitial_Android";

    void Awake()
    {
        this.initialize();
    }


    public void Start(){
    }

    public void initialize()
    {
        this.InitializeAds();
        this.LoadAd();
    }

    public void InitializeAds()
    {
        Advertisement.Initialize("5381867", false, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }


    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + this.adUnitId);
        Advertisement.Load(this.adUnitId, this);
    }

    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + this.adUnitId);
        Advertisement.Show(this.adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        this.LoadAd();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        this.LoadAd();
    }

    public void OnUnityAdsShowStart(string adUnitId) {
        Debug.Log("OnUnityAdsShowStart");
        Debug.Log("OnUnityAdsShowComplete");
        int viewedInter = 0;
        if (PlayerPrefs.HasKey("SHOWN_INTERSTITIAL"))
        {
            string encryptedNumber = PlayerPrefs.GetString("SHOWN_INTERSTITIAL");
            viewedInter = int.Parse(Crypto.decryptData(encryptedNumber, APIComunication.instance.dynamic_key));
        }
        viewedInter = viewedInter + 1;
        PlayerPrefs.SetString("SHOWN_INTERSTITIAL", Crypto.encryptData(viewedInter.ToString(), APIComunication.instance.dynamic_key));
       
    }
    public void OnUnityAdsShowClick(string adUnitId) {
        Debug.Log("OnUnityAdsShowClick");
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) {
        Debug.Log("OnUnityAdsShowComplete");
        this.LoadAd();
    }


}
