using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Bitszer.Extensions;

namespace Bitszer
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        [Header("UI")]
        public TMPro.TMP_Text titleText;
        public TMPro.TMP_Text usernameText;
        public TMPro.TMP_Text balanceText;
        public TMPro.TMP_InputField searchInputField;

        [Space]
        [Space]
        public Toggle homeToggle;
        public Toggle buyToggle;
        public Toggle sellToggle;
        public Toggle myAuctionsToggle;

        [Space]
        [Space]
        public GameObject loginPanel;
        public GameObject signupPanel;
        public GameObject tabPanel;
        public GameObject sellItemPanel;
        public GameObject itemDescPopup;
        public GameObject profilePopup;
        public GameObject buyoutPopup;
        public GameObject bidPopup;
        public GameObject cancelPopup;
        public GameObject searchScrollView;

        [Space]
        [Space]
        public Transform buyItemParent;
        public Transform buyItemPrefab;
        public Transform sellItemParent;
        public Transform sellItemPrefab;
        public Transform auctionItemParent;
        public Transform auctionItemPrefab;
        public Transform similarItemParent;
        public Transform searchBuyItemParent;

        private int _buyItemsLength = 0;
        private int _sellItemsLength = 0;
        private int _myAuctionsLength = 0;
        private int _searchItemsLength = 0;

        private bool _isHomeToggleOn = false;
        private bool _isBuyToggleOn = false;
        private bool _isSellToggleOn = false;
        private bool _isMyAuctionsToggleOn = false;

        private Profile _profile;

        private string _nextToken = null;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            Events.OnProfileReceived.AddListener(OnProfileReceived);
            Events.OnScrolledToBottom.AddListener(OnScrolledToBottom);

            homeToggle.onValueChanged.AddListener(HomeToggleValueChanged);
            buyToggle.onValueChanged.AddListener(BuyToggleValueChanged);
            sellToggle.onValueChanged.AddListener(SellToggleValueChanged);
            myAuctionsToggle.onValueChanged.AddListener(MyAuctionsToggleValueChanged);
        }

        private void OnDisable()
        {
            Events.OnProfileReceived.RemoveListener(OnProfileReceived);
            Events.OnScrolledToBottom.RemoveListener(OnScrolledToBottom);

            homeToggle.onValueChanged.RemoveListener(HomeToggleValueChanged);
            buyToggle.onValueChanged.RemoveListener(BuyToggleValueChanged);
            sellToggle.onValueChanged.RemoveListener(SellToggleValueChanged);
            myAuctionsToggle.onValueChanged.RemoveListener(MyAuctionsToggleValueChanged);
        }

        private void Start()
        {
            searchInputField.onValueChanged.AddListener(value =>
            {
                if (value.Length <= 0)
                {
                    searchBuyItemParent.Clear();
                    searchScrollView.SetActive(false);
                }
            });
        }
        #endregion

        #region Action Methods
        private void OnProfileReceived(Profile profile)
        {
            _profile = profile;

            usernameText.text = _profile.data.getMyProfile.name;
            balanceText.text = _profile.data.getMyProfile.balance.ToString("F3");
        }

        private void OnScrolledToBottom(ScrollController.SCROLL_PANEL scrollPanel)
        {
            if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.BUY))
                GetBuyData("", 10, _nextToken);

            if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.SELL))
                GetSellData(10, _nextToken);

            if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.MY_AUCTIONS))
                GetMyAuctionsData(10, _nextToken);

            if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.SIMILAR_ITEMS))
                GetBuyData("", 10, _nextToken);

            if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.SEARCH_ITEMS))
                GetSearchItemsData("", 10, _nextToken);
        }
        #endregion

        #region Toggle Methods
        private void HomeToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                if (!_isHomeToggleOn)
                {
                    _isHomeToggleOn = true;
                    _isBuyToggleOn = false;
                    _isSellToggleOn = false;
                    _isMyAuctionsToggleOn = false;

                    buyItemParent.Clear();
                    sellItemParent.Clear();
                    auctionItemParent.Clear();

                    titleText.text = "Home";

                    if (sellItemPanel.activeSelf)
                        sellItemPanel.SetActive(false);
                }
            }
        }

        private void BuyToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                if (!_isBuyToggleOn)
                {
                    _isHomeToggleOn = false;
                    _isBuyToggleOn = true;
                    _isSellToggleOn = false;
                    _isMyAuctionsToggleOn = false;

                    sellItemParent.Clear();
                    auctionItemParent.Clear();

                    titleText.text = "Buy";

                    GetBuyData("", 10, null);

                    if (sellItemPanel.activeSelf)
                        sellItemPanel.SetActive(false);
                }
            }
        }

        private void SellToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                if (!_isSellToggleOn)
                {
                    _isHomeToggleOn = false;
                    _isBuyToggleOn = false;
                    _isSellToggleOn = true;
                    _isMyAuctionsToggleOn = false;

                    buyItemParent.Clear();
                    auctionItemParent.Clear();

                    GetSellData(10, null);

                    titleText.text = "Sell";
                }
            }
        }

        private void MyAuctionsToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                if (!_isMyAuctionsToggleOn)
                {
                    _isHomeToggleOn = false;
                    _isBuyToggleOn = false;
                    _isSellToggleOn = false;
                    _isMyAuctionsToggleOn = true;

                    buyItemParent.Clear();
                    sellItemParent.Clear();

                    GetMyAuctionsData(10, null);

                    titleText.text = "My Auctions";

                    if (sellItemPanel.activeSelf)
                        sellItemPanel.SetActive(false);
                }
            }
        }
        #endregion

        #region Get Data
        public void GetBuyData(string itemName, int limit, string nextToken)
        {
            AuctionHouse dataProvider = AuctionHouse.Instance;
            APIManager.Instance.RaycastBlock(true);

            _buyItemsLength = 0;

            StartCoroutine(dataProvider.GetAuctions(itemName, limit, nextToken, result =>
            {
                if (result == null)
                {
                    APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                    APIManager.Instance.RaycastBlock(false);
                    return;
                }

                if (string.IsNullOrEmpty(itemName))
                    StartCoroutine(PopulateBuyData(result));
                else
                    StartCoroutine(PopulateSimilarItems(result));
            }));
        }

        public void GetSellData(int limit, string nextToken)
        {
            AuctionHouse dataProvider = AuctionHouse.Instance;
            APIManager.Instance.RaycastBlock(true);

            _sellItemsLength = 0;

            StartCoroutine(dataProvider.GetMyInventoryByGame(limit, nextToken, result =>
            {
                if (result == null)
                {
                    APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                    APIManager.Instance.RaycastBlock(false);
                    return;
                }

                StartCoroutine(PopulateSellData(result));
            }));
        }

        public void GetMyAuctionsData(int limit, string nextToken)
        {
            AuctionHouse dataProvider = AuctionHouse.Instance;
            APIManager.Instance.RaycastBlock(true);

            _myAuctionsLength = 0;

            StartCoroutine(dataProvider.GetUserAuctions(_profile.data.getMyProfile.id, limit, nextToken, result =>
            {
                if (result == null)
                {
                    APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                    APIManager.Instance.RaycastBlock(false);
                    return;
                }

                StartCoroutine(PopulateGetUserAuctionsData(result));
            }));
        }

        public void GetSearchItemsData(string itemName, int limit, string nextToken)
        {
            AuctionHouse dataProvider = AuctionHouse.Instance;
            APIManager.Instance.RaycastBlock(true);

            _searchItemsLength = 0;

            StartCoroutine(dataProvider.GetAuctions(itemName, limit, nextToken, result =>
            {
                if (result == null)
                {
                    APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                    APIManager.Instance.RaycastBlock(false);
                    return;
                }

                if (!string.IsNullOrEmpty(itemName))
                    StartCoroutine(PopulateSearchItems(result));
            }));
        }
        #endregion

        #region Populate Data
        private IEnumerator PopulateBuyData(GetAuction getAuction)
        {
            var count = getAuction.data.getAuctions.auctions.Count;

            if (count <= 0)
            {
                APIManager.Instance.RaycastBlock(false);
                yield break;
            }

            _nextToken = getAuction.data.getAuctions.nextToken;

            var item = getAuction.data.getAuctions.auctions[_buyItemsLength];

            if (!item.sellerProfile.id.Equals(_profile.data.getMyProfile.id))
            { 
                GameObject go = Instantiate(buyItemPrefab.gameObject, buyItemParent);
                var buyItem = go.GetComponent<BuyItem>();
                StartCoroutine(APIManager.Instance.GetImageFromUrl(item.gameItem.imageUrl, texture =>
                {
                    buyItem.itemImage.texture = texture;
                }));
                buyItem.qtyText.text = item.quantity.ToString();
                buyItem.itemNameText.text = item.gameItem.itemName;
                buyItem.usernameText.text = item.sellerProfile.name;

                DateTime currentDate = DateTime.Now;
                DateTime expirationDate = DateTime.Parse(item.expiration, null, System.Globalization.DateTimeStyles.RoundtripKind);

                buyItem.expirationText.text = $"Less than {(expirationDate - currentDate).Days} Days";
                buyItem.buyoutText.text = item.buyout.ToString();
                buyItem.bidText.text = item.bid.ToString();

                buyItem.itemImageButton.onClick.AddListener(() =>
                {
                    var itemDescPopupData = itemDescPopup.GetComponent<ItemDescPopup>();
                    itemDescPopupData.descriptionText.text = item.gameItem.description;

                    itemDescPopup.SetActive(true);
                });

                buyItem.itemNameButton.onClick.AddListener(() =>
                {
                    var itemDescPopupData = itemDescPopup.GetComponent<ItemDescPopup>();
                    itemDescPopupData.descriptionText.text = item.gameItem.description;

                    itemDescPopup.SetActive(true);
                });

                buyItem.usernameButton.onClick.AddListener(() =>
                {
                    var profilePopupData = profilePopup.GetComponent<ProfilePopup>();
                    StartCoroutine(APIManager.Instance.GetImageFromUrl(item.sellerProfile.imageUrl, texture =>
                    {
                        profilePopupData.profileImage.texture = texture;
                    }));
                    profilePopupData.usernameText.text = item.sellerProfile.screenName;
                    profilePopupData.titleText.text = item.sellerProfile.title;
                    profilePopupData.lifetimePurchasedText.text = item.sellerProfile.buyCount.ToString();
                    profilePopupData.lifetimeSpentText.text = item.sellerProfile.buyAmount.ToString();
                    profilePopupData.lifetimeSoldText.text = item.sellerProfile.soldCount.ToString();
                    profilePopupData.lifetimeEarnedText.text = item.sellerProfile.soldAmount.ToString();

                    profilePopup.SetActive(true);
                });

                buyItem.buyoutButton.onClick.AddListener(() =>
                {
                    buyoutPopup.SetActive(true);
                    var buyoutPopupData = buyoutPopup.GetComponent<BuyoutPopup>();
                    buyoutPopupData.titleText.text = $"Are you sure you want to purchase {buyItem.qtyText.text} {buyItem.itemNameText.text} for {buyItem.buyoutText.text}?";
                    buyoutPopupData.itemImage.texture = buyItem.itemImage.texture;
                    buyoutPopupData.qtyValueText.text = buyItem.qtyText.text;
                    buyoutPopupData.itemNameText.text = buyItem.itemNameText.text;
                    buyoutPopupData.usernameText.text = buyItem.usernameText.text;
                    buyoutPopupData.expirationText.text = buyItem.expirationText.text;
                    buyoutPopupData.priceText.text = buyItem.buyoutText.text;

                    buyoutPopupData.confirmButton.onClick.AddListener(() =>
                    {
                        if (float.Parse(buyoutPopupData.priceText.text) > _profile.data.getMyProfile.balance)
                        {
                            APIManager.Instance.SetError("Not sufficient balance.", "Okay", ErrorType.CustomMessage);
                            return;
                        }

                        APIManager.Instance.RaycastBlock(true);

                        StartCoroutine(AuctionHouse.Instance.Buyout(item.id, result =>
                        {
                            if (result == null || result.data == null)
                            {
                                APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                                APIManager.Instance.RaycastBlock(false);
                                return;
                            }

                            if (result.data.buyout)
                            {
                                getAuction.data.getAuctions.auctions.Remove(item);
                                Destroy(go);

                                APIManager.Instance.RaycastBlock(false);
                            }
                        }));
                    });
                });

                buyItem.bidButton.onClick.AddListener(() =>
                {
                    bidPopup.SetActive(true);
                    var bidPopupData = bidPopup.GetComponent<BidPopup>();
                    bidPopupData.titleText.text = $"Place bid for {buyItem.itemNameText.text}. Your bid must be greater than {buyItem.bidText.text}.";
                    bidPopupData.itemImage.texture = buyItem.itemImage.texture;
                    bidPopupData.qtyValueText.text = buyItem.qtyText.text;
                    bidPopupData.itemNameText.text = buyItem.itemNameText.text;
                    bidPopupData.usernameText.text = buyItem.usernameText.text;
                    bidPopupData.expirationText.text = buyItem.expirationText.text;
                    bidPopupData.bid = item.bid;

                    bidPopupData.totalBidInputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
                    bidPopupData.totalQuantityInputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
                    bidPopupData.confirmButton.onClick.AddListener(() =>
                    {
                        if (string.IsNullOrEmpty(bidPopupData.totalBidInputField.text))
                            return;

                        if (float.Parse(bidPopupData.totalBidInputField.text) > _profile.data.getMyProfile.balance)
                        {
                            APIManager.Instance.SetError("Not sufficient balance.", "Okay", ErrorType.CustomMessage);
                            return;
                        }

                        APIManager.Instance.RaycastBlock(true);

                        StartCoroutine(AuctionHouse.Instance.Bid(item.id, float.Parse(bidPopupData.totalBidInputField.text), result =>
                        {
                            if (result == null || result.data == null)
                            {
                                APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                                APIManager.Instance.RaycastBlock(false);
                                return;
                            }

                            if (result.data.bid)
                            {
                                getAuction.data.getAuctions.auctions.Remove(item);
                                Destroy(go);

                                APIManager.Instance.RaycastBlock(false);
                            }
                        }));
                    });
                });
            }

            yield return null;

            _buyItemsLength++;

            if (_buyItemsLength < count)
                StartCoroutine(PopulateBuyData(getAuction));
            else
            {
                _buyItemsLength = 0;
                APIManager.Instance.RaycastBlock(false);
            }
        }

        private IEnumerator PopulateSellData(GetMyInventoryByGame getMyInventoryByGame)
        {
            var count = getMyInventoryByGame.data.getMyInventorybyGame.inventory.Count;

            if (count <= 0)
            {
                APIManager.Instance.RaycastBlock(false);
                yield break;
            }

            _nextToken = getMyInventoryByGame.data.getMyInventorybyGame.nextToken;

            var item = getMyInventoryByGame.data.getMyInventorybyGame.inventory[_sellItemsLength];

            if (item.ItemCount > 0)
            {
                GameObject go = Instantiate(sellItemPrefab.gameObject, sellItemParent);
                var sellItem = go.GetComponent<SellItem>();
                StartCoroutine(APIManager.Instance.GetImageFromUrl(item.gameItem.imageUrl, texture =>
                {
                    sellItem.itemImage.texture = texture;
                }));
                sellItem.itemNameText.text = item.gameItem.itemName;
                sellItem.availableText.text = item.ItemCount.ToString();

                sellItem.itemImageButton.onClick.AddListener(() =>
                {
                    var itemDescPopupData = itemDescPopup.GetComponent<ItemDescPopup>();
                    itemDescPopupData.descriptionText.text = item.gameItem.description;

                    itemDescPopup.SetActive(true);
                });

                sellItem.itemNameButton.onClick.AddListener(() =>
                {
                    var itemDescPopupData = itemDescPopup.GetComponent<ItemDescPopup>();
                    itemDescPopupData.descriptionText.text = item.gameItem.description;

                    itemDescPopup.SetActive(true);
                });

                sellItem.sellButton.onClick.AddListener(() =>
                {
                    sellItem.sellButton.onClick.RemoveAllListeners();

                    similarItemParent.Clear();
                    GetBuyData(item.gameItem.itemName, 10, null);

                    var sellItemPanelData = sellItemPanel.GetComponent<SellItemPanel>();
                    sellItemPanelData.usernameText.text = _profile.data.getMyProfile.name;
                    sellItemPanelData.balanceText.text = _profile.data.getMyProfile.balance.ToString();
                    sellItemPanelData.itemImage.texture = sellItem.itemImage.texture;
                    sellItemPanelData.qtyValueText.text = sellItem.availableText.text;
                    sellItemPanelData.itemsSoldValueInputField.text = sellItem.availableText.text;
                    sellItemPanelData.itemNameText.text = sellItem.itemNameText.text;
                    sellItemPanelData.totalItemsValueText.text = sellItem.availableText.text;

                    sellItemPanelData.itemsSoldValueInputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
                    sellItemPanelData.buyoutItemValueInputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
                    sellItemPanelData.startingBidItemValueInputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
                    sellItemPanelData.totalBuyoutValueInputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
                    sellItemPanelData.totalBidValueInputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;

                    sellItemPanel.SetActive(true);

                    sellItemPanelData.confirmButton.onClick.RemoveAllListeners();
                    sellItemPanelData.confirmButton.onClick.AddListener(() =>
                    {

                        if (float.Parse(sellItemPanelData.buyoutItemValueInputField.text) <= 0)
                            return;
                        if (float.Parse(sellItemPanelData.startingBidItemValueInputField.text) <= 0)
                            return;

                        if (float.Parse(sellItemPanelData.itemsSoldValueInputField.text) > float.Parse(sellItemPanelData.totalItemsValueText.text))
                        {
                            APIManager.Instance.SetError("You can't sell more than Total Items.", "Okay", ErrorType.CustomMessage);
                            return;
                        }

                        APIManager.Instance.RaycastBlock(true);

                        AuctionInput newAuction = new AuctionInput();
                        newAuction.gameId = AuctionHouse.Instance.configuration.gameId;
                        newAuction.itemId = item.gameItem.itemId;

                        int duration = sellItemPanelData.sellDurationDropdown.value switch
                        {
                            0 => 24,
                            1 => 48,
                            2 => 72,
                            _ => 24
                        };
                        newAuction.auctionDuration = duration;

                        float bidResult, buyoutResult;
                        int qtyResult;

                        if (float.TryParse(sellItemPanelData.startingBidItemValueInputField.text, out bidResult))
                            newAuction.bid = float.Parse(sellItemPanelData.startingBidItemValueInputField.text);

                        if (float.TryParse(sellItemPanelData.buyoutItemValueInputField.text, out buyoutResult))
                            newAuction.buyout = float.Parse(sellItemPanelData.buyoutItemValueInputField.text);

                        if (int.TryParse(sellItemPanelData.itemsSoldValueInputField.text, out qtyResult))
                            newAuction.quantity = int.Parse(sellItemPanelData.itemsSoldValueInputField.text);

                        StartCoroutine(AuctionHouse.Instance.CreateAuction(newAuction, result =>
                        {
                            if (result == null || result.data == null)
                            {
                                APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                                APIManager.Instance.RaycastBlock(false);
                                return;
                            }

                            if (result.data != null)
                            {
                                sellItemPanel.SetActive(false);

                                myAuctionsToggle.isOn = true;
                            }
                        }));
                    });

                    sellItemPanelData.resetAllButton.onClick.RemoveAllListeners();
                    sellItemPanelData.resetAllButton.onClick.AddListener(() =>
                    {
                        sellItemPanelData.itemsSoldValueInputField.text = sellItem.availableText.text;
                        sellItemPanelData.buyoutItemValueInputField.text = "0";
                        sellItemPanelData.startingBidItemValueInputField.text = "0";
                        sellItemPanelData.sellDurationDropdown.value = 0;
                    });
                });
            }

            yield return null;

            _sellItemsLength++;

            if (_sellItemsLength < count)
                StartCoroutine(PopulateSellData(getMyInventoryByGame));
            else
            {
                _sellItemsLength = 0;
                APIManager.Instance.RaycastBlock(false);
            }
        }

        private IEnumerator PopulateGetUserAuctionsData(GetUserAuction getUserAuctions)
        {
            var count = getUserAuctions.data.getUserAuctions.auctions.Count;

            if (count <= 0)
            {
                APIManager.Instance.RaycastBlock(false);
                yield break;
            }

            _nextToken = getUserAuctions.data.getUserAuctions.nextToken;

            var item = getUserAuctions.data.getUserAuctions.auctions[_myAuctionsLength];

            if (item.sellerProfile.id.Equals(_profile.data.getMyProfile.id))
            {
                GameObject go = Instantiate(auctionItemPrefab.gameObject, auctionItemParent);
                var auctionItem = go.GetComponent<AuctionItem>();
                StartCoroutine(APIManager.Instance.GetImageFromUrl(item.gameItem.imageUrl, texture =>
                {
                    auctionItem.itemImage.texture = texture;
                }));
                auctionItem.qtyText.text = item.quantity.ToString();
                auctionItem.itemNameText.text = item.gameItem.itemName;

                DateTime currentDate = DateTime.Now;
                DateTime expirationDate = DateTime.Parse(item.expiration, null, System.Globalization.DateTimeStyles.RoundtripKind);

                auctionItem.expirationText.text = $"Less than {(expirationDate - currentDate).Days} Days";
                auctionItem.cancelButton.onClick.RemoveAllListeners();
                auctionItem.cancelButton.onClick.AddListener(() =>
                {
                    cancelPopup.SetActive(true);
                    var cancelPopupData = cancelPopup.GetComponent<CancelPopup>();
                    cancelPopupData.titleText.text = $"Are you sure you want to cancel {auctionItem.itemNameText.text}";
                    cancelPopupData.itemImage.texture = auctionItem.itemImage.texture;
                    cancelPopupData.qtyText.text = auctionItem.qtyText.text;
                    cancelPopupData.itemNameText.text = auctionItem.itemNameText.text;
                    cancelPopupData.expirationText.text = auctionItem.expirationText.text;

                    cancelPopupData.cancelAuctionButton.onClick.RemoveAllListeners();
                    cancelPopupData.cancelAuctionButton.onClick.AddListener(() =>
                    {
                        APIManager.Instance.RaycastBlock(true);

                        StartCoroutine(AuctionHouse.Instance.CancelAuction(item.id, result =>
                        {
                            if (result == null || result.data == null)
                            {
                                APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                                APIManager.Instance.RaycastBlock(false);
                                return;
                            }

                            if (result.data.cancelAuction)
                            {
                                getUserAuctions.data.getUserAuctions.auctions.Remove(item);
                                Destroy(go);

                                APIManager.Instance.RaycastBlock(false);
                            }
                        }));
                    });
                });
            }

            yield return null;

            _myAuctionsLength++;

            if (_myAuctionsLength < count)
                StartCoroutine(PopulateGetUserAuctionsData(getUserAuctions));
            else
            {
                _myAuctionsLength = 0;
                APIManager.Instance.RaycastBlock(false);
            }
        }

        private IEnumerator PopulateSimilarItems(GetAuction getAuction)
        {
            var count = getAuction.data.getAuctions.auctions.Count;

            if (count <= 0)
            {
                APIManager.Instance.RaycastBlock(false);
                yield break;
            }

            _nextToken = getAuction.data.getAuctions.nextToken;

            var item = getAuction.data.getAuctions.auctions[_buyItemsLength];

            if (!item.sellerProfile.id.Equals(_profile.data.getMyProfile.id))
            {
                GameObject go = Instantiate(buyItemPrefab.gameObject, similarItemParent);
                var buyItem = go.GetComponent<BuyItem>();
                StartCoroutine(APIManager.Instance.GetImageFromUrl(item.gameItem.imageUrl, texture =>
                {
                    buyItem.itemImage.texture = texture;
                }));
                buyItem.qtyText.text = item.quantity.ToString();
                buyItem.itemNameText.text = item.gameItem.itemName;
                buyItem.usernameText.text = item.sellerProfile.name;

                DateTime currentDate = DateTime.Now;
                DateTime expirationDate = DateTime.Parse(item.expiration, null, System.Globalization.DateTimeStyles.RoundtripKind);

                buyItem.expirationText.text = $"Less than {(expirationDate - currentDate).Days} Days";
                buyItem.buyoutText.text = item.buyout.ToString();
                buyItem.bidText.text = item.bid.ToString();

                buyItem.usernameButton.onClick.AddListener(() =>
                {
                    var profilePopupData = profilePopup.GetComponent<ProfilePopup>();
                    StartCoroutine(APIManager.Instance.GetImageFromUrl(item.sellerProfile.imageUrl, texture =>
                    {
                        profilePopupData.profileImage.texture = texture;
                    }));
                    profilePopupData.usernameText.text = item.sellerProfile.screenName;
                    profilePopupData.titleText.text = item.sellerProfile.title;
                    profilePopupData.lifetimePurchasedText.text = item.sellerProfile.buyCount.ToString();
                    profilePopupData.lifetimeSpentText.text = item.sellerProfile.buyAmount.ToString();
                    profilePopupData.lifetimeSoldText.text = item.sellerProfile.soldCount.ToString();
                    profilePopupData.lifetimeEarnedText.text = item.sellerProfile.soldAmount.ToString();

                    profilePopup.SetActive(true);
                });

                buyItem.buyoutButton.onClick.AddListener(() =>
                {
                    buyoutPopup.SetActive(true);
                    var buyoutPopupData = buyoutPopup.GetComponent<BuyoutPopup>();
                    buyoutPopupData.titleText.text = $"Are you sure you want to purchase {buyItem.qtyText.text} {buyItem.itemNameText.text} for {buyItem.buyoutText.text}?";
                    buyoutPopupData.itemImage.texture = buyItem.itemImage.texture;
                    buyoutPopupData.qtyValueText.text = buyItem.qtyText.text;
                    buyoutPopupData.itemNameText.text = buyItem.itemNameText.text;
                    buyoutPopupData.usernameText.text = buyItem.usernameText.text;
                    buyoutPopupData.expirationText.text = buyItem.expirationText.text;
                    buyoutPopupData.priceText.text = buyItem.buyoutText.text;

                    buyoutPopupData.confirmButton.onClick.AddListener(() =>
                    {
                        APIManager.Instance.RaycastBlock(true);

                        StartCoroutine(AuctionHouse.Instance.Buyout(item.id, result =>
                        {
                            if (result == null || result.data == null)
                            {
                                APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                                APIManager.Instance.RaycastBlock(false);
                                return;
                            }

                            if (result.data.buyout)
                            {
                                getAuction.data.getAuctions.auctions.Remove(item);
                                Destroy(go);

                                APIManager.Instance.RaycastBlock(false);
                            }
                        }));
                    });
                });

                buyItem.bidButton.onClick.AddListener(() =>
                {
                    bidPopup.SetActive(true);
                    var bidPopupData = bidPopup.GetComponent<BidPopup>();
                    bidPopupData.titleText.text = $"Place bid for {buyItem.itemNameText.text}. Your bid must be greater than {buyItem.bidText.text}.";
                    bidPopupData.itemImage.texture = buyItem.itemImage.texture;
                    bidPopupData.qtyValueText.text = buyItem.qtyText.text;
                    bidPopupData.itemNameText.text = buyItem.itemNameText.text;
                    bidPopupData.usernameText.text = buyItem.usernameText.text;
                    bidPopupData.expirationText.text = buyItem.expirationText.text;

                    bidPopupData.confirmButton.onClick.AddListener(() =>
                    {
                        APIManager.Instance.RaycastBlock(true);

                        StartCoroutine(AuctionHouse.Instance.Bid(item.id, float.Parse(bidPopupData.totalBidInputField.text), result =>
                        {
                            if (result == null || result.data == null)
                            {
                                APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                                APIManager.Instance.RaycastBlock(false);
                                return;
                            }

                            if (result.data.bid)
                            {
                                getAuction.data.getAuctions.auctions.Remove(item);
                                Destroy(go);

                                APIManager.Instance.RaycastBlock(false);
                            }
                        }));
                    });
                });
            }

            yield return null;

            _buyItemsLength++;

            if (_buyItemsLength < count)
                StartCoroutine(PopulateSimilarItems(getAuction));
            else
            {
                _buyItemsLength = 0;
                APIManager.Instance.RaycastBlock(false);
            }
        }

        private IEnumerator PopulateSearchItems(GetAuction getAuction)
        {
            var count = getAuction.data.getAuctions.auctions.Count;

            if (count <= 0)
            {
                APIManager.Instance.RaycastBlock(false);
                yield break;
            }

            _nextToken = getAuction.data.getAuctions.nextToken;

            var item = getAuction.data.getAuctions.auctions[_searchItemsLength];

            GameObject go = Instantiate(buyItemPrefab.gameObject, searchBuyItemParent);
            var buyItem = go.GetComponent<BuyItem>();
            StartCoroutine(APIManager.Instance.GetImageFromUrl(item.gameItem.imageUrl, texture =>
            {
                buyItem.itemImage.texture = texture;
            }));
            buyItem.qtyText.text = item.quantity.ToString();
            buyItem.itemNameText.text = item.gameItem.itemName;
            buyItem.usernameText.text = item.sellerProfile.name;

            DateTime currentDate = DateTime.Now;
            DateTime expirationDate = DateTime.Parse(item.expiration, null, System.Globalization.DateTimeStyles.RoundtripKind);

            buyItem.expirationText.text = $"Less than {(expirationDate - currentDate).Days} Days";
            buyItem.buyoutText.text = item.buyout.ToString();
            buyItem.bidText.text = item.bid.ToString();

            buyItem.usernameButton.onClick.AddListener(() =>
            {
                var profilePopupData = profilePopup.GetComponent<ProfilePopup>();
                StartCoroutine(APIManager.Instance.GetImageFromUrl(item.sellerProfile.imageUrl, texture =>
                {
                    profilePopupData.profileImage.texture = texture;
                }));
                profilePopupData.usernameText.text = item.sellerProfile.screenName;
                profilePopupData.titleText.text = item.sellerProfile.title;
                profilePopupData.lifetimePurchasedText.text = item.sellerProfile.buyCount.ToString();
                profilePopupData.lifetimeSpentText.text = item.sellerProfile.buyAmount.ToString();
                profilePopupData.lifetimeSoldText.text = item.sellerProfile.soldCount.ToString();
                profilePopupData.lifetimeEarnedText.text = item.sellerProfile.soldAmount.ToString();

                profilePopup.SetActive(true);
            });

            buyItem.buyoutButton.onClick.AddListener(() =>
            {
                buyoutPopup.SetActive(true);
                var buyoutPopupData = buyoutPopup.GetComponent<BuyoutPopup>();
                buyoutPopupData.titleText.text = $"Are you sure you want to purchase {buyItem.qtyText.text} {buyItem.itemNameText.text} for {buyItem.buyoutText.text}?";
                buyoutPopupData.itemImage.texture = buyItem.itemImage.texture;
                buyoutPopupData.qtyValueText.text = buyItem.qtyText.text;
                buyoutPopupData.itemNameText.text = buyItem.itemNameText.text;
                buyoutPopupData.usernameText.text = buyItem.usernameText.text;
                buyoutPopupData.expirationText.text = buyItem.expirationText.text;
                buyoutPopupData.priceText.text = buyItem.buyoutText.text;

                buyoutPopupData.confirmButton.onClick.AddListener(() =>
                {
                    APIManager.Instance.RaycastBlock(true);

                    StartCoroutine(AuctionHouse.Instance.Buyout(item.id, result =>
                    {
                        if (result == null || result.data == null)
                        {
                            APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                            APIManager.Instance.RaycastBlock(false);
                            return;
                        }

                        if (result.data.buyout)
                        {
                            getAuction.data.getAuctions.auctions.Remove(item);
                            Destroy(go);

                            APIManager.Instance.RaycastBlock(false);
                        }
                    }));
                });
            });

            buyItem.bidButton.onClick.AddListener(() =>
            {
                bidPopup.SetActive(true);
                var bidPopupData = bidPopup.GetComponent<BidPopup>();
                bidPopupData.titleText.text = $"Place bid for {buyItem.itemNameText.text}. Your bid must be greater than {buyItem.bidText.text}.";
                bidPopupData.itemImage.texture = buyItem.itemImage.texture;
                bidPopupData.qtyValueText.text = buyItem.qtyText.text;
                bidPopupData.itemNameText.text = buyItem.itemNameText.text;
                bidPopupData.usernameText.text = buyItem.usernameText.text;
                bidPopupData.expirationText.text = buyItem.expirationText.text;

                bidPopupData.confirmButton.onClick.AddListener(() =>
                {
                    APIManager.Instance.RaycastBlock(true);

                    StartCoroutine(AuctionHouse.Instance.Bid(item.id, float.Parse(bidPopupData.totalBidInputField.text), result =>
                    {
                        if (result == null || result.data == null)
                        {
                            APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                            APIManager.Instance.RaycastBlock(false);
                            return;
                        }

                        if (result.data.bid)
                        {
                            getAuction.data.getAuctions.auctions.Remove(item);
                            Destroy(go);

                            APIManager.Instance.RaycastBlock(false);
                        }
                    }));
                });
            });

            yield return null;

            _searchItemsLength++;

            if (_searchItemsLength < count)
                StartCoroutine(PopulateSearchItems(getAuction));
            else
            {
                searchScrollView.SetActive(true);
                _searchItemsLength = 0;
                APIManager.Instance.RaycastBlock(false);
            }
        }
        #endregion

        #region Public Methods
        // Assigned to "SearchButton" in the inspector
        public void SearchButton()
        {
            if (!string.IsNullOrEmpty(searchInputField.text))
            {
                searchBuyItemParent.Clear();
                GetSearchItemsData(searchInputField.text, 10, "");
            }
        }

        // Assigned to "LoginButton" in the inspector under "SignupPanel"
        public void OpenLoginPanel()
        {
            loginPanel.SetActive(true);
            signupPanel.SetActive(false);
            tabPanel.SetActive(false);
        }

        // Assigned to "RegisterButton" in the inspector under "LoginPanel"
        public void OpenSignupPanel()
        {
            signupPanel.SetActive(true);
            loginPanel.SetActive(false);
            tabPanel.SetActive(false);
        }

        public void OpenTabPanel()
        {
            signupPanel.SetActive(false);
            loginPanel.SetActive(false);
            tabPanel.SetActive(true);
        }

        // Assigned to "ReturnToGameButton" in the inspector
        public void CloseAuctionHouse()
        {
            AuctionHouse.Instance.Close();
        }
        #endregion
    }
}