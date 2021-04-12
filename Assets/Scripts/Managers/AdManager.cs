using System;
using CustomHelper;
using GoogleMobileAds.Api;
using Managers.Core;
using UnityEngine;

namespace Managers
{
    public class AdManager : SingletonManager<AdManager>
    {
        private const string BannerAdIDTest = "ca-app-pub-3940256099942544/6300978111";
        private const string BannerAdID = "ca-app-pub-8971229083845313/5794514243";
        private const string InterstitialAdIDTest = "ca-app-pub-3940256099942544/1033173712";
        private const string InterstitialAdID = "ca-app-pub-8971229083845313/8229105890";

        private BannerView _bannerAd;
        private InterstitialAd _interstitial;


        public static void ShowBannerAds(bool show)
        {
            if (show)
                Instance.ShowBanner();
            else
                Instance.HideBanner();
        }

        public static void ShowInterstitial()
        {
            Instance.ShowInterstitialAd();
        }

        protected override void Init()
        {
            MobileAds.Initialize(status => { });
            RequestBannerAd();
            RequestInterstitial();
        }
        
        #region Banner Ads

        private void RequestBannerAd()
        {
            _bannerAd = new BannerView(BannerAdIDTest, AdSize.Banner, AdPosition.Bottom);
            _bannerAd.OnAdLoaded += HandleOnAdLoaded;
            _bannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;

            var request = new AdRequest.Builder().Build();
            _bannerAd.LoadAd(request);
        }

        private void ShowBanner()
        {
            _bannerAd.Show();
        }

        private void HideBanner()
        {
            _bannerAd.Hide();
        }
   
        #endregion

        #region  Interstitial Ads

        private void RequestInterstitial()
        {
            _interstitial = new InterstitialAd(InterstitialAdIDTest);

            _interstitial.OnAdLoaded += HandleOnAdLoaded;
            _interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            _interstitial.OnAdOpening += HandleOnAdOpened;
            _interstitial.OnAdClosed += HandleOnAdClosed;
            _interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

            var request = new AdRequest.Builder().Build();

            _interstitial.LoadAd(request);
        }

        private void ShowInterstitialAd()
        {
            if (_interstitial.IsLoaded())
                _interstitial.Show();
        }
        
        #endregion
        
        private void HandleOnAdLoaded(object sender, EventArgs e)
        {
            this.Log($"Ad loaded for {sender.GetType()}");
            if (sender is BannerView)
                HideBanner();
        }

        private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            this.Log($"Ad load failed for {sender.GetType()} error code {e.Message}");
        }

        private void HandleOnAdLeavingApplication(object sender, EventArgs e)
        {
            this.Log($"Leaving application on Ad for {sender.GetType()}");
        }

        private void HandleOnAdClosed(object sender, EventArgs e)
        {
            this.Log($"Ad close for {sender.GetType()}");
            if (sender is InterstitialAd)
                Time.timeScale = 1;
        }

        private void HandleOnAdOpened(object sender, EventArgs e)
        {
            this.Log($"Ad open for {sender.GetType()}");
            if (sender is InterstitialAd)
                Time.timeScale = 0;
        }

        #region Singleton Attributes

        private static AdManager Instance =>
            _instance ? _instance : throw new UnityException($"No instance of {nameof(AdManager)}");
        
        private static AdManager _instance;

        protected override AdManager Self
        {
            set => _instance = value;
            get => _instance;
        }
        
        #endregion
    }
}