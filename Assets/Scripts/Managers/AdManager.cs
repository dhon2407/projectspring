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

        private BannerView _bannerAd;

        protected override void Init()
        {
            MobileAds.Initialize(status => { });
            RequestBannerAd();
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
        
        private void HandleOnAdLoaded(object sender, EventArgs e)
        {
            this.Log($"Ad loaded for {sender.GetType()}");
        }

        private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            this.Log($"Ad load failed for {sender.GetType()} error code {e.Message}");
        }

        public void HideBanner()
        {
            _bannerAd.Hide();
        }
   
        #endregion
        
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