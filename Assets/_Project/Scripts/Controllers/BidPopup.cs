using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bitszer
{
    public class BidPopup : MonoBehaviour
    {
        public TMP_Text titleText;
        public RawImage itemImage;
        public TMP_Text qtyValueText;
        public TMP_Text itemNameText;
        public TMP_Text usernameText;
        public TMP_Text expirationText;
        public TMP_InputField totalBidInputField;
        public TMP_InputField totalQuantityInputField;
        public Button confirmButton;

        [HideInInspector] public double bid;

        private double totalBid, totalQuantity, result;

        private void Start()
        {
            totalBidInputField.onValueChanged.AddListener(value =>
            {
                if (value.Length <= 0)
                    return;

                if (double.Parse(value) <= bid)
                    totalBidInputField.text = bid.ToString();

                if (double.TryParse(totalBidInputField.text, out result))
                    totalBid = result;

                totalQuantityInputField.text = (totalBid / bid).ToString();
            });

            totalQuantityInputField.onValueChanged.AddListener(value =>
            {
                if (value.Length <= 0)
                    return;

                if (double.TryParse(totalQuantityInputField.text, out result))
                    totalQuantity = result;

                totalBidInputField.text = (totalQuantity * bid).ToString();
            });
        }
    }
}