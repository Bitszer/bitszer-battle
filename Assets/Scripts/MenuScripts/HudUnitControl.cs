using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MenuScripts
{
    [RequireComponent(typeof(Button))]
    public sealed class HudUnitControl : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerSpawnManager playerSpawnManager = null;
        
        [Header("Components")]
        [SerializeField] private Text  textCost = null;
        [SerializeField] private Text  textLevel = null;
        [SerializeField] private Image imageFill = null;
        
        [Header("Settings")]
        [SerializeField] private float coolDownInitialization = 2.0F;

        public float CoolDownInitialization => coolDownInitialization;
        
        private HeroUnitData _unitData;
        
        private Button _button;
        
        /*
         * Mono Behavior.
         */

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }
        
        /*
         * Public.
         */

        public void SetUnitData(HeroUnitData unitData)
        {
            _unitData = unitData;
            
            _button.interactable = _unitData.IsUnlocked;
            
            if (_unitData.IsUnlocked)
            {
                textCost.text = _unitData.SpawnCost.ToString();
                textLevel.text = _unitData.Level.ToString();
            }
            else
            {
                textCost.text = string.Empty;
                textLevel.text = string.Empty;
            }
        }

        public void PlayCoolDownAnimation(float coolDown)
        {
            _button.interactable = false;
            StartCoroutine(CoolDownCoroutine(coolDown));
        }
        
        /*
         * Private.
         */
        
        IEnumerator CoolDownCoroutine(float coolDown)
        {
            imageFill.fillAmount = 1;
            
            LeanTween.value(gameObject, val =>
                     {
                         imageFill.fillAmount = val;
                     }, 1f, 0f, coolDown)
                     .setOnComplete(() =>
                     {
                         imageFill.fillAmount = 0;
                     });
            
            yield return new WaitForSeconds(coolDown);
            
            _button.interactable = true;
        }

        /*
         * Event Handlers.
         */

        private void OnClick()
        {
            if (playerSpawnManager.TrySpawnUnit(_unitData.id))
                PlayCoolDownAnimation(_unitData.CoolDown);
        }
    }
}