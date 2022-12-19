using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System.Collections.Generic;
//using static Constants.Utility;


namespace Bitszer
{
    public class AdManager : MonoBehaviour
    {
        public static AdManager instance;


        private AppOpenAd appOpenAd;
        private RewardedAd rewardedAd;
        private BannerView bannerView;
        private InterstitialAd interstitialAd;
        private RewardedInterstitialAd rewardedInterstitialAd;

        public UnityEvent OnAdLoadedEvent;
        public UnityEvent OnAdClosedEvent;
        public UnityEvent OnAdOpeningEvent;
        public UnityEvent OnAdFailedToLoadEvent;
        public UnityEvent OnAdFailedToShowEvent;
        public UnityEvent OnUserEarnedRewardEvent;
        //    public Text fpsMeter;
        public Text statusText;
        public bool showFpsMeter = true;

        public const string SHOW_APP_OPEN_AD = "ShowAppOpenAd";
        public const string SHOW_REWARDED_AD = "ShowRewardedAd";
        public const string SHOW_INTERSTITIAL_AD = "ShowInterstitialAd";

        public const string DESTROY_BANNER_AD = "DestroyBannerAd";

        public const string REQUEST_REWARDED_AD = "RequestRewardedAd";
        public const string REQUEST_APP_OPEN_AD = "RequestAppOpenAd";
        public const string REQUEST_TOP_BANNER_AD = "RequestTopBannerAd";
        public const string REQUEST_INTERSTITIAL_AD = "RequestInterstitialAd";
        public const string REQUEST_BOTTOM_BANNER_AD = "RequestBottomBannerAd";

#if UNITY_ANDROID
        public const string AppOpenAdId = "ca-app-pub-3946311877444508/7594323217";
        public const string BannerAdId = "ca-app-pub-3946311877444508/3701758782";
        public const string RewardedAdId = "ca-app-pub-3946311877444508/2935472020";
        public const string RewardedInterstitialAdId = "ca-app-pub-3946311877444508/2935472020";
        public const string InterstitialAdId = "ca-app-pub-3946311877444508/5753207051";
#endif
#if UNITY_IOS
                public const string AppOpenAdId = "ca-app-pub-8530336860846959/1777759486"; 
                public const string BannerAdId = "ca-app-pub-8530336860846959/6724622032"; 
                public const string RewardedAdId = "ca-app-pub-8530336860846959/7493125831"; 
                public const string RewardedInterstitialAdId = "ca-app-pub-3946311877444508/2935472020";
                public const string InterstitialAdId = "ca-app-pub-8530336860846959/2405462440"; 
#endif


        #region UNITY MONOBEHAVIOR METHODS

        public void Awake()
        {
            instance = this;
            MobileAds.SetiOSAppPauseOnBackground(true);

            // Configure TagForChildDirectedTreatment and test device IDs.
            RequestConfiguration requestConfiguration =
                 new RequestConfiguration.Builder()
                 .SetSameAppKeyEnabled(true).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(HandleInitCompleteAction);
        }

        public void ButtonClick(string whichCase)
        {

            switch (whichCase)
            {
                case REQUEST_BOTTOM_BANNER_AD:
                    RequestBannerAd(0);
                    break;

                case REQUEST_TOP_BANNER_AD:
                    RequestBannerAd(1);
                    break;

                case DESTROY_BANNER_AD:
                    BannerAdDelete();
                    break;

                case SHOW_INTERSTITIAL_AD:
                    ShowInterstitialAd();
                    ButtonClick(REQUEST_INTERSTITIAL_AD);
                    break;

                case REQUEST_INTERSTITIAL_AD:
                    RequestAndLoadInterstitialAd();
                    break;

                case SHOW_REWARDED_AD:
                    ShowRewardedAd();
                    ButtonClick(REQUEST_REWARDED_AD);
                    break;

                case REQUEST_REWARDED_AD:
                    RequestAndLoadRewardedAd();
                    break;

                case SHOW_APP_OPEN_AD:
                    //ShowAppOpenAd();
                    break;

                case REQUEST_APP_OPEN_AD:
                    //RequestAndLoadAppOpenAd();
                    break;
            }
        }

        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            // Callbacks from GoogleMobileAds are not guaranteed to be called on
            // main thread.
            // In this example we use MobileAdsEventExecutor to schedule these calls on
            // the next Update() loop.
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                statusText.text = "Initialization complete";

                BannerAdDelete();
                RequestBannerAd(0);
            });
        }

        #endregion

        #region HELPER METHODS

        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                .Build();
        }

        public void OnApplicationPause(bool paused)
        {
            // Display the app open ad when the app is foregrounded.
            /*if (!paused)
            {
                ShowAppOpenAd();
            }*/
        }

        #endregion

        #region BANNER ADS

        private void BannerAdDelete()
        {
            if (bannerView != null)
            {
                bannerView.Destroy();
            }
        }

        private void RequestBannerAd(int i)
        {
            statusText.text = "Requesting Banner Ad.";

            // These ad units are configured to always serve test ads.

#if UNITY_ANDROID
            string adUnitId = BannerAdId;
#elif UNITY_IPHONE
        string adUnitId = BannerAdId;
#else
        string adUnitId = "unexpected_platform";
#endif

            // Clean up banner before reusing
            if (bannerView != null)
            {
                bannerView.Destroy();
            }

            // AdSize newAdSize = new AdSize(288, 45);
            // Create a 320x50 banner at top of the screen

            bannerView = new BannerView(adUnitId, AdSize.Banner, i == 0 ? AdPosition.Bottom : AdPosition.Top);


            // Add Event Handlers
            bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
            bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
            bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
            bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

            // Load a banner ad
            bannerView.LoadAd(CreateAdRequest());
        }

        #endregion

        #region INTERSTITIAL ADS

        private void RequestAndLoadInterstitialAd()
        {
            statusText.text = "Requesting Interstitial Ad.";

#if UNITY_ANDROID
            string adUnitId = InterstitialAdId;
#elif UNITY_IPHONE
        string adUnitId = InterstitialAdId;
#else
        string adUnitId = "unexpected_platform";
#endif

            interstitialAd = new InterstitialAd(adUnitId);

            // Add Event Handlers
            interstitialAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
            interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
            interstitialAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
            interstitialAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

            // Load an interstitial ad
            interstitialAd.LoadAd(CreateAdRequest());
        }

        private void ShowInterstitialAd()
        {
            if (interstitialAd != null && interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
            }
            else
            {
                statusText.text = "Interstitial ad is not ready yet";
            }
        }

        #endregion

        #region REWARDED ADS

        private void RequestAndLoadRewardedAd()
        {
            statusText.text = "Requesting Rewarded Ad.";

#if UNITY_ANDROID
            string adUnitId = RewardedAdId;
#elif UNITY_IPHONE
        string adUnitId = RewardedAdId;
#else
        string adUnitId = "unexpected_platform";
#endif

            // create new rewarded ad instance
            rewardedAd = new RewardedAd(adUnitId);

            // Add Event Handlers
            rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
            rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
            rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
            rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
            rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
            rewardedAd.OnUserEarnedReward += OnUserEarnedRewardEvents;
            // Create empty ad request
            rewardedAd.LoadAd(CreateAdRequest());
        }

        public void OnUserEarnedRewardEvents(object sender, Reward args)
        {
            //Set Reward
        }

        private void ShowRewardedAd()
        {
            if (rewardedAd != null)
            {
                statusText.text = "Rewarded ad is not null.";
                rewardedAd.Show();
            }
            else
            {
                statusText.text = "Rewarded ad is not ready yet.";
            }
        }

        private void RequestAndLoadRewardedInterstitialAd()
        {
            statusText.text = "Requesting Rewarded Interstitial Ad.";

#if UNITY_ANDROID
            string adUnitId = RewardedInterstitialAdId;
#elif UNITY_IPHONE
        string adUnitId = RewardedInterstitialAdId;
#else
        string adUnitId = "unexpected_platform";
#endif
            // Create an interstitial.
            RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
            {
                if (error != null)
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        statusText.text = "RewardedInterstitialAd load failed, error: " + error;
                    });
                    return;
                }
                this.rewardedInterstitialAd = rewardedInterstitialAd;
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    statusText.text = "RewardedInterstitialAd loaded";
                });
                // Register for ad events.
                this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        statusText.text = "Rewarded Interstitial presented.";
                    });
                };
                this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        statusText.text = "Rewarded Interstitial dismissed.";
                    });
                    this.rewardedInterstitialAd = null;
                };
                this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        statusText.text = "Rewarded Interstitial failed to present.";
                    });
                    this.rewardedInterstitialAd = null;
                };
            });
        }

        private void ShowRewardedInterstitialAd()
        {
            if (rewardedInterstitialAd != null)
            {
                rewardedInterstitialAd.Show((reward) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        statusText.text = "User Rewarded: " + reward.Amount;
                    });
                });
            }
            else
            {
                statusText.text = "Rewarded ad is not ready yet.";
            }
        }

        #endregion

        #region APPOPEN ADS

        private void RequestAndLoadAppOpenAd()
        {
            statusText.text = "Requesting App Open Ad.";

#if UNITY_ANDROID
            string adUnitId = AppOpenAdId;
#elif UNITY_IPHONE
        string adUnitId = AppOpenAdId;
#else
        string adUnitId = "unexpected_platform";
#endif

            // create new app open ad instance
            AppOpenAd.LoadAd(adUnitId, ScreenOrientation.Portrait, CreateAdRequest(), (appOpenAd, error) =>
            {
                if (error != null)
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        statusText.text = "AppOpenAd load failed, error: " + error;
                    });
                    return;
                }
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    statusText.text = "AppOpenAd loaded. Please background the app and return.";
                });
                this.appOpenAd = appOpenAd;
            });
        }

        private void ShowAppOpenAd()
        {
            // Register for ad events.
            this.appOpenAd.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    // Debug.Log("AppOpenAd dismissed.");
                    if (this.appOpenAd != null)
                    {
                        this.appOpenAd.Destroy();
                        this.appOpenAd = null;
                    }
                });
            };
            this.appOpenAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
            {
                var msg = args.AdError.GetMessage();
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    statusText.text = "AppOpenAd present failed, error: " + msg;
                    if (this.appOpenAd != null)
                    {
                        this.appOpenAd.Destroy();
                        this.appOpenAd = null;
                    }
                });
            };
            this.appOpenAd.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    //  Debug.Log("AppOpenAd presented.");
                });
            };
            this.appOpenAd.OnAdDidRecordImpression += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Debug.Log("AppOpenAd recorded an impression.");
                });
            };
            this.appOpenAd.OnPaidEvent += (sender, args) =>
            {
                string currencyCode = args.AdValue.CurrencyCode;
                long adValue = args.AdValue.Value;
                string suffix = "AppOpenAd received a paid event.";
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}", suffix, currencyCode, adValue);
                    statusText.text = msg;
                });
            };
            appOpenAd.Show();
        }

        #endregion

        #region AD INSPECTOR

        private void OpenAdInspector()
        {
            statusText.text = "Open Ad Inspector.";

            MobileAds.OpenAdInspector((error) =>
            {
                if (error != null)
                {
                    string errorMessage = error.GetMessage();
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        statusText.text = "Ad Inspector failed to open, error: " + errorMessage;
                    });
                }
                else
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        statusText.text = "Ad Inspector closed.";
                    });
                }
            });
        }

        #endregion


    }
}