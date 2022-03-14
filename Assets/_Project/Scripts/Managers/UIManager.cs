using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
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

    private int buyItemsLength = 0;
    private int sellItemsLength = 0;
    private int myAuctionsLength = 0;
    private int searchItemsLength = 0;

    private Profile _profile;
    private GetAuction _getAuction;
    private GetInventory _getInventory;
    private GetAuctionbyGame _getAuctionbyGame;

    private string _nextToken = null;

    private void OnEnable()
    {
        Events.OnProfileReceived.AddListener(OnProfileReceived);
        Events.OnAuctionsReceived.AddListener(OnAuctionsReceived);
        Events.OnInventoryReceived.AddListener(OnInventoryReceived);
        Events.OnAuctionsByGameReceived.AddListener(OnAuctionByGameReceived);

        Events.OnScrolledToBottom.AddListener(OnScrolledToBottom);

        homeToggle.onValueChanged.AddListener(HomeToggleValueChanged);
        buyToggle.onValueChanged.AddListener(BuyToggleValueChanged);
        sellToggle.onValueChanged.AddListener(SellToggleValueChanged);
        myAuctionsToggle.onValueChanged.AddListener(MyAuctionsToggleValueChanged);
    }

    private void OnDisable()
    {
        Events.OnProfileReceived.RemoveListener(OnProfileReceived);
        Events.OnAuctionsReceived.RemoveListener(OnAuctionsReceived);
        Events.OnInventoryReceived.RemoveListener(OnInventoryReceived);
        Events.OnAuctionsByGameReceived.RemoveListener(OnAuctionByGameReceived);

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

    private void OnProfileReceived(Profile profile)
    {
        _profile = profile;

        usernameText.text = _profile.data.getMyProfile.name;
        balanceText.text = _profile.data.getMyProfile.balance.ToString();
    }

    private void OnAuctionsReceived(GetAuction getAuction)
    {
        _getAuction = getAuction;
    }

    private void OnInventoryReceived(GetInventory getInventory)
    {
        _getInventory = getInventory;
    }

    private void OnAuctionByGameReceived(GetAuctionbyGame getAuctionByGame)
    {
        _getAuctionbyGame = getAuctionByGame;
    }

    private void OnScrolledToBottom(ScrollController.SCROLL_PANEL scrollPanel)
    {
        if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.BUY))
            StartCoroutine(GetBuyData("", 10, _nextToken));

        if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.SELL))
            StartCoroutine(GetSellData(10, _nextToken));

        if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.MY_AUCTIONS))
            StartCoroutine(GetAuctionsByGameData(10, _nextToken));

        if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.SIMILAR_ITEMS))
            StartCoroutine(GetBuyData("", 10, _nextToken));

        if (!string.IsNullOrEmpty(_nextToken) && scrollPanel.Equals(ScrollController.SCROLL_PANEL.SEARCH_ITEMS))
            StartCoroutine(GetSearchItemsData("", 10, _nextToken));
    }

    #region Toggle Methods
    private void HomeToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            buyItemParent.Clear();
            sellItemParent.Clear();
            auctionItemParent.Clear();

            titleText.text = "Home";
        }
    }

    private void BuyToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            sellItemParent.Clear();
            auctionItemParent.Clear();

            titleText.text = "Buy";
            StartCoroutine(GetBuyData("", 10, null));
        }
    }

    private void SellToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            buyItemParent.Clear();
            auctionItemParent.Clear();

            StartCoroutine(GetSellData(10, null));

            titleText.text = "Sell";
        }
    }

    private void MyAuctionsToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            buyItemParent.Clear();
            sellItemParent.Clear();

            StartCoroutine(GetAuctionsByGameData(3, null));

            titleText.text = "My Auctions";
        }
    }
    #endregion

    public IEnumerator GetBuyData(string itemName, int limit, string nextToken)
    {
        DataProvider dataProvider = DataProvider.Instance;
        APIManager.Instance.RaycastBlock(true);

        buyItemsLength = 0;

        GetAuction result = null;
        var getAuction = dataProvider.GetAuctions(itemName, limit, nextToken);

        getAuction.ContinueWith(_ =>
        {
            if (_.IsCompleted)
                result = _.Result;
        });

        yield return new WaitUntil(() => result != null);

        if (string.IsNullOrEmpty(itemName))
            StartCoroutine(PopulateBuyData(result));
        else
            StartCoroutine(PopulateSimilarItems(result));
    }

    public IEnumerator GetSellData(int limit, string nextToken)
    {
        DataProvider dataProvider = DataProvider.Instance;
        APIManager.Instance.RaycastBlock(true);

        sellItemsLength = 0;

        GetInventory result = null;
        var getInventory = dataProvider.GetInventory(limit, nextToken);

        getInventory.ContinueWith(_ =>
        {
            if (_.IsCompleted)
                result = _.Result;
        });

        yield return new WaitUntil(() => result != null);

        StartCoroutine(PopulateSellData(result));
    }

    public IEnumerator GetAuctionsByGameData(int limit, string nextToken)
    {
        DataProvider dataProvider = DataProvider.Instance;
        APIManager.Instance.RaycastBlock(true);

        myAuctionsLength = 0;

        GetAuctionbyGame result = null;
        var getAuctionsByGame = dataProvider.GetAuctionsByGame(limit, nextToken);

        getAuctionsByGame.ContinueWith(_ =>
        {
            if (_.IsCompleted)
                result = _.Result;
        });

        yield return new WaitUntil(() => result != null);

        StartCoroutine(PopulateAuctionsByGameData(result));
    }

    public IEnumerator GetSearchItemsData(string itemName, int limit, string nextToken)
    {
        DataProvider dataProvider = DataProvider.Instance;
        APIManager.Instance.RaycastBlock(true);

        searchItemsLength = 0;

        GetAuction result = null;
        var getAuction = dataProvider.GetAuctions(itemName, limit, nextToken);

        getAuction.ContinueWith(_ =>
        {
            if (_.IsCompleted)
                result = _.Result;
        });

        yield return new WaitUntil(() => result != null);

        if (!string.IsNullOrEmpty(itemName))
            StartCoroutine(PopulateSearchItems(result));
    }

    private IEnumerator PopulateBuyData(GetAuction getAuction)
    {
        var count = getAuction.data.getAuctions.auctions.Count;
        
        if (count <= 0)
        {
            APIManager.Instance.RaycastBlock(false);
            yield break;
        }

        _nextToken = getAuction.data.getAuctions.nextToken;

        var item = getAuction.data.getAuctions.auctions[buyItemsLength];

        GameObject go = Instantiate(buyItemPrefab.gameObject, buyItemParent);
        var buyItem = go.GetComponent<BuyItem>();
        StartCoroutine(GetImageFromUrl(item.gameItem.imageUrl, texture =>
        {
            buyItem.itemImage.texture = texture;
        }));
        buyItem.qtyText.text = item.quantity.ToString();
        buyItem.itemNameText.text = item.gameItem.itemName;
        buyItem.usernameText.text = item.sellerProfile.name;
        buyItem.expirationText.text = item.expiration;
        buyItem.buyoutText.text = item.buyout.ToString();
        buyItem.bidText.text = item.bid.ToString();

        buyItem.usernameButton.onClick.AddListener(() =>
        {
            var profilePopupData = profilePopup.GetComponent<ProfilePopup>();
            StartCoroutine(GetImageFromUrl(item.sellerProfile.imageUrl, texture =>
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
                DataProvider.Instance.Buyout(item.id);
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

            bidPopupData.confirmButton.onClick.AddListener(() =>
            {
                DataProvider.Instance.Bid(item.id, float.Parse(bidPopupData.totalBidInputField.text));
            });
        });

        yield return null;

        buyItemsLength++;

        if (buyItemsLength < count)
            StartCoroutine(PopulateBuyData(getAuction));
        else
        {
            buyItemsLength = 0;
            APIManager.Instance.RaycastBlock(false);
        }
    }

    IEnumerator PopulateSellData(GetInventory getInventory)
    {
        var count = getInventory.data.getInventory.inventory.Count;

        if (count <= 0)
        {
            APIManager.Instance.RaycastBlock(false);
            yield break;
        }

        _nextToken = getInventory.data.getInventory.nextToken;

        var item = getInventory.data.getInventory.inventory[sellItemsLength];

        GameObject go = Instantiate(sellItemPrefab.gameObject, sellItemParent);
        var sellItem = go.GetComponent<SellItem>();
        StartCoroutine(GetImageFromUrl(item.gameItem.imageUrl, texture =>
        {
            sellItem.itemImage.texture = texture;
        }));
        sellItem.itemNameText.text = item.gameItem.itemName;
        sellItem.availableText.text = item.ItemCount.ToString();

        sellItem.sellButton.onClick.AddListener(() =>
        {
            similarItemParent.Clear();
            StartCoroutine(GetBuyData(item.gameItem.itemName, 10, null));

            var sellItemPanelData = sellItemPanel.GetComponent<SellItemPanel>();
            sellItemPanelData.usernameText.text = _profile.data.getMyProfile.name;
            sellItemPanelData.balanceText.text = _profile.data.getMyProfile.balance.ToString();
            sellItemPanelData.itemImage.texture = sellItem.itemImage.texture;
            sellItemPanelData.qtyValueText.text = sellItem.availableText.text;
            sellItemPanelData.itemsSoldValueInputField.text = sellItem.availableText.text;
            sellItemPanelData.itemNameText.text = sellItem.itemNameText.text;
            sellItemPanelData.totalItemsValueText.text = sellItem.availableText.text;

            AuctionInput newAuction = new AuctionInput();
            newAuction.gameId = DataProvider.Instance.configuration.gameId;
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

            if (float.TryParse(sellItemPanelData.totalBidValueText.text, out bidResult))
                newAuction.bid = bidResult;

            if (float.TryParse(sellItemPanelData.totalBuyoutValueText.text, out buyoutResult))
                newAuction.buyout = buyoutResult;

            if (int.TryParse(sellItemPanelData.itemsSoldValueInputField.text, out qtyResult))
                newAuction.quantity = qtyResult;

            sellItemPanel.SetActive(true);

            sellItemPanelData.confirmButton.onClick.AddListener(() =>
            {
                DataProvider.Instance.CreateAuction(newAuction);
            });

            sellItemPanelData.resetAllButton.onClick.AddListener(() =>
            {

            });
        });

        yield return null;

        sellItemsLength++;

        if (sellItemsLength < count)
            StartCoroutine(PopulateSellData(getInventory));
        else
        {
            sellItemsLength = 0;
            APIManager.Instance.RaycastBlock(false);
        }
    }

    private IEnumerator PopulateAuctionsByGameData(GetAuctionbyGame getAuctionbyGame)
    {
        var count = getAuctionbyGame.data.getAuctionsbyGame.auctions.Count;

        if (count <= 0)
        {
            APIManager.Instance.RaycastBlock(false);
            yield break;
        }

        _nextToken = getAuctionbyGame.data.getAuctionsbyGame.nextToken;

        var item = getAuctionbyGame.data.getAuctionsbyGame.auctions[myAuctionsLength];

        GameObject go = Instantiate(auctionItemPrefab.gameObject, auctionItemParent);
        var auctionItem = go.GetComponent<AuctionItem>();
        StartCoroutine(GetImageFromUrl(item.gameItem.imageUrl, texture =>
        {
            auctionItem.itemImage.texture = texture;
        }));
        auctionItem.qtyText.text = item.quantity.ToString();
        auctionItem.itemNameText.text = item.gameItem.itemName;
        auctionItem.expirationText.text = item.expiration;
        auctionItem.buyoutText.text = item.buyout.ToString();
        auctionItem.bidText.text = item.bid.ToString();

        if (!item.sellerProfile.id.Equals(_profile.data.getMyProfile.id))
            auctionItem.cancelButton.gameObject.SetActive(false);

        auctionItem.cancelButton.onClick.AddListener(() =>
        {
            cancelPopup.SetActive(true);
            var cancelPopupData = cancelPopup.GetComponent<CancelPopup>();
            cancelPopupData.titleText.text = $"Are you sure you want to cancel {auctionItem.itemNameText.text}";
            cancelPopupData.itemImage.texture = auctionItem.itemImage.texture;
            cancelPopupData.qtyText.text = auctionItem.qtyText.text;
            cancelPopupData.itemNameText.text = auctionItem.itemNameText.text;
            cancelPopupData.expirationText.text = auctionItem.expirationText.text;

            cancelPopupData.cancelAuctionButton.onClick.AddListener(() =>
            {
                DataProvider.Instance.CancelAuction(item.id);
            });
        });

        auctionItem.buyoutButton.onClick.AddListener(() =>
        {
            buyoutPopup.SetActive(true);
            var buyoutPopupData = buyoutPopup.GetComponent<BuyoutPopup>();
            buyoutPopupData.titleText.text = $"Are you sure you want to purchase {auctionItem.qtyText.text} {auctionItem.itemNameText.text} for {auctionItem.buyoutText.text}?";
            buyoutPopupData.itemImage.texture = auctionItem.itemImage.texture;
            buyoutPopupData.qtyValueText.text = auctionItem.qtyText.text;
            buyoutPopupData.itemNameText.text = auctionItem.itemNameText.text;
            buyoutPopupData.usernameText.text = "";
            buyoutPopupData.expirationText.text = auctionItem.expirationText.text;
            buyoutPopupData.priceText.text = auctionItem.buyoutText.text;

            buyoutPopupData.confirmButton.onClick.AddListener(() =>
            {
                DataProvider.Instance.Buyout(item.id);
            });
        });

        auctionItem.bidButton.onClick.AddListener(() =>
        {
            bidPopup.SetActive(true);
            var bidPopupData = bidPopup.GetComponent<BidPopup>();
            bidPopupData.titleText.text = $"Place bid for {auctionItem.itemNameText.text}. Your bid must be greater than {auctionItem.bidText.text}.";
            bidPopupData.itemImage.texture = auctionItem.itemImage.texture;
            bidPopupData.qtyValueText.text = auctionItem.qtyText.text;
            bidPopupData.itemNameText.text = auctionItem.itemNameText.text;
            bidPopupData.usernameText.text = "";
            bidPopupData.expirationText.text = auctionItem.expirationText.text;
            bidPopupData.bid = item.bid;

            bidPopupData.confirmButton.onClick.AddListener(() =>
            {
                DataProvider.Instance.Bid(item.id, float.Parse(bidPopupData.totalBidInputField.text));
            });
        });

        yield return null;

        myAuctionsLength++;

        if (myAuctionsLength < count)
            StartCoroutine(PopulateAuctionsByGameData(getAuctionbyGame));
        else
        {
            myAuctionsLength = 0;
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

        var item = getAuction.data.getAuctions.auctions[buyItemsLength];

        GameObject go = Instantiate(buyItemPrefab.gameObject, similarItemParent);
        var buyItem = go.GetComponent<BuyItem>();
        StartCoroutine(GetImageFromUrl(item.gameItem.imageUrl, texture =>
        {
            buyItem.itemImage.texture = texture;
        }));
        buyItem.qtyText.text = item.quantity.ToString();
        buyItem.itemNameText.text = item.gameItem.itemName;
        buyItem.usernameText.text = item.sellerProfile.name;
        buyItem.expirationText.text = item.expiration;
        buyItem.buyoutText.text = item.buyout.ToString();
        buyItem.bidText.text = item.bid.ToString();

        buyItem.usernameButton.onClick.AddListener(() =>
        {
            var profilePopupData = profilePopup.GetComponent<ProfilePopup>();
            StartCoroutine(GetImageFromUrl(item.sellerProfile.imageUrl, texture =>
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
                DataProvider.Instance.Buyout(item.id);
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
                DataProvider.Instance.Bid(item.id, float.Parse(bidPopupData.totalBidInputField.text));
            });
        });

        yield return null;

        buyItemsLength++;

        if (buyItemsLength < count)
            StartCoroutine(PopulateSimilarItems(getAuction));
        else
        {
            buyItemsLength = 0;
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

        var item = getAuction.data.getAuctions.auctions[searchItemsLength];

        GameObject go = Instantiate(buyItemPrefab.gameObject, searchBuyItemParent);
        var buyItem = go.GetComponent<BuyItem>();
        StartCoroutine(GetImageFromUrl(item.gameItem.imageUrl, texture =>
        {
            buyItem.itemImage.texture = texture;
        }));
        buyItem.qtyText.text = item.quantity.ToString();
        buyItem.itemNameText.text = item.gameItem.itemName;
        buyItem.usernameText.text = item.sellerProfile.name;
        buyItem.expirationText.text = item.expiration;
        buyItem.buyoutText.text = item.buyout.ToString();
        buyItem.bidText.text = item.bid.ToString();

        buyItem.usernameButton.onClick.AddListener(() =>
        {
            var profilePopupData = profilePopup.GetComponent<ProfilePopup>();
            StartCoroutine(GetImageFromUrl(item.sellerProfile.imageUrl, texture =>
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
                DataProvider.Instance.Buyout(item.id);
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
                DataProvider.Instance.Bid(item.id, float.Parse(bidPopupData.totalBidInputField.text));
            });
        });

        yield return null;

        searchItemsLength++;

        if (searchItemsLength < count)
            StartCoroutine(PopulateSearchItems(getAuction));
        else
        {
            searchScrollView.SetActive(true);
            searchItemsLength = 0;
            APIManager.Instance.RaycastBlock(false);
        }
    }

    public IEnumerator GetImageFromUrl(string url, Action<Texture> texture)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
            texture(((DownloadHandlerTexture)request.downloadHandler).texture);
    }

    public void SearchButton()
    {
        if (!string.IsNullOrEmpty(searchInputField.text))
        {
            searchBuyItemParent.Clear();
            StartCoroutine(GetSearchItemsData(searchInputField.text, 10, ""));
        }
    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        tabPanel.SetActive(false);
    }

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
}