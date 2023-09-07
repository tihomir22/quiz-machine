using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class Banner : MonoBehaviour
{
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
    void Start()
    {
        // Initialize the SDK if you haven't already done so:
        Advertisement.Banner.SetPosition(_bannerPosition);
        Advertisement.Initialize("5381867", false);
        this.LoadBanner();
    }

    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load("Banner", options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        this.ShowBannerAd();
    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
        this.LoadBanner();
    }

    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show("Banner", options);
    }

    // Implement a method to call when the Hide Banner button is clicked:
    void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }



}
