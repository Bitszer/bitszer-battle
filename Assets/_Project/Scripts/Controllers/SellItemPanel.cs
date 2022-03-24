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
        public TMP_Text totalBuyoutValueText;

        [Header("ThirdRow")]
        [Space]
        [Space]
        public TMP_InputField startingBidItemValueInputField;
        public TMP_Text totalBidValueText;

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
                if (int.Parse(value) > int.Parse(totalItemsValueText.text))
                {
                    itemsSoldValueInputField.text = totalItemsValueText.text;
                    return;
                }

                totalBuyoutValueText.text = (int.Parse(value) * int.Parse(buyoutItemValueInputField.text)).ToString();
                totalBidValueText.text = (int.Parse(value) * int.Parse(startingBidItemValueInputField.text)).ToString();
            });

            buyoutItemValueInputField.onValueChanged.AddListener(value =>
            {
                if (value.Length <= 0)
                {
                    totalBuyoutValueText.text = "0";
                    return;
                }

                totalBuyoutValueText.text = (int.Parse(value) * int.Parse(itemsSoldValueInputField.text)).ToString();
            });

            startingBidItemValueInputField.onValueChanged.AddListener(value =>
            {
                if (value.Length <= 0)
                {
                    totalBidValueText.text = "0";
                    return;
                }

                totalBidValueText.text = (int.Parse(value) * int.Parse(itemsSoldValueInputField.text)).ToString();
            });
        }

        // Assigned to MaxButton
        public void MaxButton()
        {
            itemsSoldValueInputField.text = totalItemsValueText.text;
        }
    }
}