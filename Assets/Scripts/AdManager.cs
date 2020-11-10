using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
	public bool testing = true;

    public int interstitialReccurrence = 3;

    [Space(5)]
    public string AndroidappId = "APP_ID_HERE";
	public string IOSappId = "APP_ID_HERE";
	public string TestAndroidappId = "ca-app-pub-3940256099942544~3347511713";
	public string TestIOSappId = "ca-app-pub-3940256099942544~1458002511";

    [Space(5)]
    public string AndroidBannerTopAdUnitId = "UNIT_HERE";
	public string IOSBannerTopAdUnitId = "UNIT_HERE";
	public string TestAndroidBannerTopAdUnitId = "ca-app-pub-3940256099942544/6300978111";
	public string TestIOSBannerTopAdUnitId = "ca-app-pub-3940256099942544/2934735716";

    [Space(5)]
    public string AndroidInterstitialAdUnitId = "UNIT_HERE";
	public string IOSInterstitialAdUnitId = "UNIT_HERE";
	public string TestAndroidInterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
	public string TestIOSInterstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";

    [Space(5)]
    public string AndroidRewardedAdUnitId = "UNIT_HERE";
    public string IOSRewardedAdUnitId = "UNIT_HERE";
    public string TestAndroidRewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    public string TestIOSRewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";

    public BannerView bannerView;
	public InterstitialAd interstitial;
    public RewardedAd rewardedAd;

    int reason;

    // Start is called before the first frame update
    void Start()
    {
		#if UNITY_ANDROID
		string appId = (!testing) ? AndroidappId : TestAndroidappId;
        this.rewardedAd = (!testing) ? new RewardedAd(AndroidRewardedAdUnitId) : new RewardedAd(TestAndroidRewardedAdUnitId);

#elif UNITY_IPHONE
		string appId = (!testing) ? IOSappId : TestIOSappId;
                this.rewardedAd = (!testing) ? new RewardedAd(IOSRewardedAdUnitId) : new RewardedAd(TestIOSRewardedAdUnitId);

#else
		string appId = "unexpected_platform";
        this.rewardedAd = new RewardedAd(TestAndroidRewardedAdUnitId);
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
		this.RequestTopBanner();
        this.RequestInterstitial();
        this.RequestRewarded();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RequestRewarded() {

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    private void RequestTopBanner()
	{
		#if UNITY_ANDROID
		string adUnitId = (!testing) ? AndroidBannerTopAdUnitId : TestAndroidBannerTopAdUnitId;
		#elif UNITY_IPHONE
		string adUnitId = (!testing) ? IOSBannerTopAdUnitId : TestIOSBannerTopAdUnitId;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
		AdRequest request;
		if(testing){
			request = new AdRequest.Builder()
				.AddTestDevice(AdRequest.TestDeviceSimulator)
				.AddTestDevice(SystemInfo.deviceUniqueIdentifier)
			.Build();
		}else{
			request = new AdRequest.Builder().Build();
		}

		bannerView.LoadAd(request);
        bannerView.Hide();
	}

    public void showTopBanner() {
        bannerView.Show();
    }

	public void RequestInterstitial()
	{
		#if UNITY_ANDROID
		string adUnitId = (!testing) ? AndroidInterstitialAdUnitId : TestAndroidInterstitialAdUnitId;
		#elif UNITY_IPHONE
		string adUnitId = (!testing) ? IOSInterstitialAdUnitId : TestIOSInterstitialAdUnitId;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		interstitial = new InterstitialAd(adUnitId);
		AdRequest request;
		if(testing){
			request = new AdRequest.Builder()
				.AddTestDevice(AdRequest.TestDeviceSimulator)
				.AddTestDevice(SystemInfo.deviceUniqueIdentifier)
			.Build();
		}else{
			request = new AdRequest.Builder().Build();
		}
		interstitial.LoadAd(request);
	}

	public void ShowInterstitial()
	{
		if (interstitial.IsLoaded()) {
			interstitial.Show();
		}
		RequestInterstitial();
	}

    public void WatchRewardedAd(int reason) {
        this.reason = reason;
        if (this.rewardedAd.IsLoaded()) {
            this.rewardedAd.Show();
        }
    }

    public void RemoveBanner()
	{
		Debug.Log("HIDE BANNER");
		bannerView.Hide();
		bannerView.Destroy();
	}

    public void HandleRewardedAdLoaded(object sender, EventArgs args) {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args) {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args) {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        RequestRewarded();
    }

    public void HandleUserEarnedReward(object sender, Reward args) {
        if(reason == 0) {
            GetComponent<MoveOnTrack>().AdCallbackhandlerBuy();
        } else {
            GetComponent<MoveOnTrack>().AdCallbackhandlerContinue();
        }
    }
}
