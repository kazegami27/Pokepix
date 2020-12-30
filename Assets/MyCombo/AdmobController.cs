using UnityEngine;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdmobController : MonoBehaviour
{
    private BannerView bannerView;
    public InterstitialAd interstitial;
    public RewardBasedVideoAd rewardBasedVideo;
    public string ID_AdInter;

    public static AdmobController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        MobileAds.SetiOSAppPauseOnBackground(true);
        ID_AdInter = "ca-app-pub-4973559944609228/7342158657";
        if (!CUtils.IsAdsRemoved())
        {
            RequestInterstitial();
        }

        //RequestInterstitial();
        InitRewardedVideo();
        RequestRewardBasedVideo();
        RequestBanner();
    }

    private void InitRewardedVideo()
    {
        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
        this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
        this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
        this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
        this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
        this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
        this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;
    }

    public void RequestBanner()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = GameConfig.instance.admob.androidBanner.Trim();
#elif UNITY_IPHONE
        string adUnitId = GameConfig.instance.admob.iosBanner.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Register for ad events.
        this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerView.OnAdOpening += this.HandleAdOpened;
        this.bannerView.OnAdClosed += this.HandleAdClosed;
        this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        // Load a banner ad.
        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    public void RequestInterstitial()
    {
        // These ad units are configured to always serve test ads.
//#if UNITY_EDITOR
//        string adUnitId = "unused";
//#elif UNITY_ANDROID
//        string adUnitId = GameConfig.instance.admob.androidInterstitial.Trim();
//#elif UNITY_IPHONE
//        string adUnitId = GameConfig.instance.admob.iosInterstitial.Trim();
//#else
//        string adUnitId = "unexpected_platform";
//#endif

        // Create an interstitial.
        this.interstitial = new InterstitialAd(ID_AdInter);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void RequestRewardBasedVideo()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = GameConfig.instance.admob.androidRewarded.Trim();
#elif UNITY_IPHONE
        string adUnitId = GameConfig.instance.admob.iosRewarded.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif

        this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("3C94990AA9A387A256D3B2BBBFEA51EA")
                .AddTestDevice("6F599887BC401CFB1C7087F15D7C0834")
                .AddTestDevice("B543DCF2C7591C7FB8B52A3A1E7138F6")
                .AddTestDevice("8619926A823916A224795141B93B7E0B")
                .AddTestDevice("6399D5AEE5C75205B6C0F6755365CF21")
                .AddTestDevice("A0518C6FA4396B91F82B9656DE83AFC7")
                .AddTestDevice("C8EEFFC32272E3F1018FC72ECBD46F0C")
                .AddTestDevice("2E379568A9F147A64B0E0C9571DE812D")
                .AddKeyword("game")
                .TagForChildDirectedTreatment(false)
                .AddExtra("color_bg", "9B30FF")
                .Build();
    }

    public void ShowInterstitial(InterstitialAd ad)
    {
        if (ad != null && ad.IsLoaded())
        {
            ad.Show();
        }
    }

    public void ShowBanner()
    {
        if (bannerView != null)
        {
            bannerView.Show();
        }
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    public bool ShowInterstitial()
    {
        if (interstitial != null && interstitial.IsLoaded())
        {
            interstitial.Show();
            return true;
        }
        return false;
    }

    public void ShowRewardBasedVideo()
    {
        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            MonoBehaviour.print("Reward based video ad is not ready yet");
            Toast.instance.ShowMessage("Reward based video ad is not ready yet");
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
        GameController.instance.popUploading.SetActive(false);
        RequestInterstitial();
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        HideBanner();
        GameController.instance.popUploading.SetActive(false);
        print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
        GameController.instance.popUploading.SetActive(false);
        RequestBanner();
        ShowBanner();
        RequestInterstitial();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        HideBanner();
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        RequestBanner();
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
        ModelControl.countPaint += 3;
        Board.instance.UpdateUINumb();
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    #endregion
}
