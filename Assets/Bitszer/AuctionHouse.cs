using System;
using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync.SyncManager;
using UnityEngine;

namespace Bitszer
{
    public sealed class AuctionHouse : MonoBehaviour
    {
        private static readonly AuctionHouseLogger _log = new AuctionHouseLogger().Disable();

        public static bool Initializing  { get; private set; }
        public static bool Initialized   { get; private set; }
        public static bool Synchronizing { get; private set; }

        public static RegionEndpoint RegionEndpoint        { get; }      = RegionEndpoint.USWest2;
        public static float          UpdateIntervalSeconds { get; set; } = 5.0F;

        public static bool HasItems => _itemsDataSet.Records.Count > 0;
        public static bool IsEmpty  => _itemsDataSet.Records.Count == 0;

        public static Action            OnInitialized;
        public static Action<Exception> OnInitializationFailed;

        public static Action            OnSynchronized;
        public static Action<Exception> OnSynchronizationFailed;

        public static Action<Dictionary<string, int>> OnItemsChanged;

        private static string _poolId;
        private static string _identityId;

        private static CognitoSyncManager    _syncManager;
        private static CognitoAWSCredentials _credentials;
        private static Dataset               _itemsDataSet;

        private static AuctionHouse _instance;

        private static Dictionary<string, int> _pendingItems;
        private static Dictionary<string, int> _localItems;

        private static bool _waitingForSync;

        /*
         * Initialization.
         */

        public static void Initialize(string poolId)
        {
            if (Initializing)
            {
                _log.Error("Already initializing");
                return;
            }

            if (Initialized)
            {
                _log.Error("Already initialized");
                return;
            }

            _log.Debug("Initializing");

            Initializing = true;

            _poolId = poolId;
            _instance = new GameObject("BitszerAuctionHouse").AddComponent<AuctionHouse>();

            UnityInitializer.AttachToGameObject(_instance.gameObject);

            AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
            AWSConfigs.LoggingConfig.LogTo = LoggingOptions.None;

            _credentials = new CognitoAWSCredentials(_poolId, RegionEndpoint);
            _credentials.GetIdentityIdAsync(OnIdentityReceived);

            _syncManager = new CognitoSyncManager(_credentials, RegionEndpoint);
        }

        private static void OnIdentityReceived(AmazonCognitoIdentityResult<string> result)
        {
            if (result.Exception != null)
            {
                _log.Debug("Initialization failed");
                _log.Error(result.Exception);
                OnInitializationFailed?.Invoke(result.Exception);
                return;
            }

            _identityId = result.Response;

            _itemsDataSet = _syncManager.OpenOrCreateDataset("gameItems");
            _itemsDataSet.OnSyncSuccess += OnSyncSuccess;
            _itemsDataSet.OnSyncFailure += OnSyncFailure;

            _log.Debug($"Identity received. IdentityId: {_identityId}");

            SyncImpl();
        }

        /*
         * Sync.
         */

        /*
         * Will initiate syncing immediately.
         */
        public static void Sync()
        {
            _log.Debug("Sync");

            if (!Initialized)
            {
                _log.Error("Not initialized");
                return;
            }

            SyncImpl();
        }

        private static void StartSyncing()
        {
            _log.Debug("StartSyncing");

            if (_waitingForSync)
            {
                _log.Debug("Waiting for Sync");
                return;
            }

            _instance.StartCoroutine(SyncCoroutine());
        }

        private static IEnumerator SyncCoroutine()
        {
            _waitingForSync = true;
            yield return new WaitForSeconds(UpdateIntervalSeconds);
            _waitingForSync = false;
            SyncImpl();
        }

        private static void SyncImpl()
        {
            _log.Debug("SyncImpl");

            if (Synchronizing)
            {
                _log.Debug("Sync is in progress.");
                return;
            }

            Synchronizing = true;
            _itemsDataSet.SynchronizeOnConnectivity();
        }

        private static void OnSyncSuccess(object sender, SyncSuccessEventArgs eventArguments)
        {
            _log.Debug("OnSyncSuccess");

            foreach (var record in _itemsDataSet.Records)
                _log.Debug(record.Key + " " + record.Value);

            var itemsRemote = GetItems();
            var itemsLocal = GetLocalItems();

            Dictionary<string, int> itemsDeltas = null;

            if (itemsLocal != null)
            {
                _log.Debug("Calculating Deltas");

                itemsDeltas = CalculateItemsDeltas(itemsLocal, itemsRemote);
                if (itemsDeltas.Count > 0)
                {
                    _log.Debug("Dispatching Deltas");
                    OnItemsChanged?.Invoke(itemsDeltas);
                }
                else
                {
                    _log.Debug("No Changes");
                }
            }

            SetLocalItems(itemsRemote);

            if (_pendingItems != null)
            {
                _log.Debug("Processing Pending Sync");

                if (itemsDeltas != null && itemsDeltas.Count > 0)
                    itemsLocal = SumDictionaries(_pendingItems, itemsDeltas);
                else
                    itemsLocal = _pendingItems;

                foreach (var item in itemsLocal)
                    _itemsDataSet.Put(item.Key, item.Value.ToString());

                SetLocalItems(itemsLocal);

                _pendingItems = null;
                _itemsDataSet.SynchronizeOnConnectivity();
                return;
            }

            Synchronizing = false;

            if (!Initialized)
            {
                Initializing = false;
                Initialized = true;
                OnInitialized?.Invoke();
            }

            OnSynchronized?.Invoke();

            StartSyncing();
        }

        private static void OnSyncFailure(object sender, SyncFailureEventArgs eventArguments)
        {
            var dataset = sender as Dataset;
            if (dataset?.Metadata != null)
                _log.Error($"Sync failed for dataset \"{dataset.Metadata.DatasetName}\"");
            else
                _log.Error("Sync failed");

            _log.Error(eventArguments.Exception);

            Synchronizing = false;
            OnSynchronizationFailed?.Invoke(eventArguments.Exception);

            StartSyncing();
        }

        /*
         * Get.
         */

        /*
         * Returns a new dictionary with items data.
         */
        public static Dictionary<string, int> GetItems()
        {
            var items = new Dictionary<string, int>();
            GetItems(ref items);
            return items;
        }

        /*
         * Use this for memory efficient implementations.
         * Define a dictionary on your side and update it by calling this method after OnSynchronized event was fired. 
         */
        public static void GetItems(ref Dictionary<string, int> items)
        {
            if (items == null)
                items = new Dictionary<string, int>();

            foreach (var record in _itemsDataSet.Records)
                items[record.Key] = int.Parse(record.Value);
        }

        private static Dictionary<string, int> GetLocalItems()
        {
            return _localItems;
        }

        /*
         * Set.
         */

        public static void SetItems(Dictionary<string, int> items, bool sync)
        {
            if (!Initialized)
            {
                _log.Error("Not initialized");
                return;
            }

            _log.Debug("SetItems");

            _pendingItems = items;

            if (sync)
                Sync();
        }

        private static void SetLocalItems(Dictionary<string, int> items)
        {
            _localItems = items;
        }

        /*
         * Helpers.
         */

        public static Dictionary<string, int> CalculateItemsDeltas(Dictionary<string, int> dict01,
                                                                   Dictionary<string, int> dict02)
        {
            var keys = new HashSet<string>();
            keys.UnionWith(dict01.Keys);
            keys.UnionWith(dict02.Keys);

            var res = new Dictionary<string, int>();

            foreach (var key in keys)
            {
                var value01 = GetValueOrDefault(dict01, key);
                var value02 = GetValueOrDefault(dict02, key);
                if (value01 != value02)
                    res[key] = value02 - value01;
            }

            return res;
        }

        private static Dictionary<string, int> SumDictionaries(Dictionary<string, int> dict01,
                                                               Dictionary<string, int> dict02)
        {
            var keys = new HashSet<string>();
            keys.UnionWith(dict01.Keys);
            keys.UnionWith(dict02.Keys);

            var res = new Dictionary<string, int>();

            foreach (var key in keys)
            {
                var value01 = GetValueOrDefault(dict01, key);
                var value02 = GetValueOrDefault(dict02, key);
                if (value01 != value02)
                    res[key] = value02 + value01;
            }

            return res;
        }

        /*
         * Open.
         */

        public static void Open()
        {
            if (!Initialized)
            {
                _log.Error("Not initialized");
                return;
            }

            _log.Debug("Open");

            #if UNITY_IPHONE
                OpenAuctionHouseIOS();
            #endif

            #if UNITY_ANDROID
            OpenAuctionHouseAndroid();
            #endif
        }

        #if UNITY_IPHONE
        
        private static void OpenAuctionHouseIOS()
        {
            throw new NotImplementedException("Not implemented for iOS platform.");
        }

        #endif

        #if UNITY_ANDROID

        private static void OpenAuctionHouseAndroid()
        {
            try
            {
                var pluginClass = new AndroidJavaClass("com.davisonc.bitszerv4.AuctionHouse");
                var playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                var poolId = _poolId;
                var identityId = _identityId;
                object[] args =
                {
                    activity, poolId, identityId
                };
                pluginClass.CallStatic("open", args);
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }
        }

        /*
         * Android fix for IL2CPP mode.
         */

        public static void UsedOnlyForAOTCodeGeneration()
        {
            //Bug reported on github https://github.com/aws/aws-sdk-net/issues/477
            //IL2CPP restrictions: https://docs.unity3d.com/Manual/ScriptingRestrictions.html
            //Inspired workaround: https://docs.unity3d.com/ScriptReference/AndroidJavaObject.Get.html

            new AndroidJavaObject("android.os.Message").Get<int>("what");
        }

        #endif

        /*
         * Helpers.
         */

        private static V GetValueOrDefault<K, V>(IReadOnlyDictionary<K, V> dictionary, K key, V defaultValue = default)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;
            return defaultValue;
        }
    }
}