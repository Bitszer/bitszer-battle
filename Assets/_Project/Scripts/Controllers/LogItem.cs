using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogItem : MonoBehaviour
{

    public GameObject ItemObj;
    public GameObject AuctionFeeObj;
    public GameObject BidObj;
    public GameObject BuyOutObj;
    public GameObject QuantityObj;
    public GameObject ProfitObj;
    public GameObject BuyerNameObj;
    public GameObject SellerNameObj;

    public RawImage itemImage;
    public TMP_Text ActionName;
    public TMP_Text bid;
    public TMP_Text buyout;
    public TMP_Text auctionHouseFee;
    public TMP_Text Profit;

    public TMP_Text createdAt;
    public TMP_Text expiration;
    public TMP_Text quantity;
    public TMP_Text itemName;
    public TMP_Text gameName;
    public TMP_Text highBidderProfile;
    public TMP_Text sellerName;
    public TMP_Text buyerName;
    public TMP_Text timestamp;
}
