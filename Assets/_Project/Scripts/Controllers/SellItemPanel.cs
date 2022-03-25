using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bitszer
{
    public class SellItemPanel : MonoBehaviour
    {
        [Header("Topbar")]
        public TMP_Text usernameText;
        public TMP_Text balanceText;

        [Header("ItemBar")]
        public RawImage itemImage;
        public TMP_Text qtyValueText;
        public TMP_Text itemNameText;

        [Header("FirstRow")]
        [Space]
        [Space]
        public TMP_Text totalItemsValueText;
        public TMP_InputField itemsSoldValueInputField;

        [Header("SecondRow")]
        [Space]
        [Space]
        public TMP_InputField buyoutItemValueInputField;
        public TMP_InputField totalBuyoutValueInputField;

        [Header("ThirdRow")]
        [Space]
        [Space]
        public TMP_InputField startingBidItemValueInputField;
        public TMP_InputField totalBidValueInputField;

        [Header("FourthRow")]
        [Space]
        [Space]
        public TMP_Dropdown sellDurationDropdown;

        [Header("Buttons")]
        [Space]
        [Space]
        public Button confirmButton;
        public Button resetAllButton;

        private void Start()
        {
            itemsSoldValueInputField.onValueChanged.AddListener(value =>
            {
                if (float.Parse(value) > float.Parse(totalItemsValueText.text))
                {
                    itemsSoldValueInputField.text = totalItemsValueText.text;
                    return;
                }

                totalBuyoutValueInputField.text = (float.Parse(value) * float.Parse(buyoutItemValueInputField.text)).ToString();
                totalBidValueInputField.text = (float.Parse(value) * float.Parse(startingBidItemValueInputField.text)).ToString();
            });

            buyoutItemValueInputField.onValueChanged.AddListener(value =>
            {
                if (value.Length <= 0)
                {
                    totalBuyoutValueInputField.text = "0";
                    return;
                }

                totalBuyoutValueInputField.text = (float.Parse(value) * float.Parse(itemsSoldValueInputField.text)).ToString();
            });

            startingBidItemValueInputField.onValueChanged.AddListener(value =>
            {
                if (value.Length <= 0)
                {
                    totalBidValueInputField.text = "0";
                    return;
                }

                totalBidValueInputField.text = (float.Parse(value) * float.Parse(itemsSoldValueInputField.text)).ToString();
            });

            totalBuyoutValueInputField.onValueChanged.AddListener(value =>
            {
                buyoutItemValueInputField.text = (float.Parse(value) / float.Parse(itemsSoldValueInputField.text)).ToString();
            });

            totalBidValueInputField.onValueChanged.AddListener(value =>
            {
                startingBidItemValueInputField.text = (float.Parse(value) / float.Parse(itemsSoldValueInputField.text)).ToString();
            });
        }

        // Assigned to MaxButton
        public void MaxButton()
        {
            itemsSoldValueInputField.text = totalItemsValueText.text;
        }
    }
}