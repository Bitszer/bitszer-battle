using UnityEngine;
using UnityEngine.UI;

namespace MenuScripts
{
    public sealed class HudManaBar : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Text  textMana  = null;
        [SerializeField] private Image imageFill = null;
        
        /*
         * Public.
         */

        public void UpdateMana(float mana, float manaMax)
        {
            imageFill.fillAmount = ClampRange.Clamp(0, manaMax, 0, 1, mana);
            textMana.text = Mathf.FloorToInt(mana) + " / " + Mathf.FloorToInt(manaMax);
        }
    }
}