using UnityEngine;
using UnityEngine.UI;

namespace Bitszer
{
    public class ScrollController : MonoBehaviour
    {
        public enum SCROLL_PANEL : byte
        {
            BUY, SELL, MY_AUCTIONS, MY_LOGS, SIMILAR_ITEMS, SEARCH_ITEMS,
        }

        public SCROLL_PANEL scrollPanel;

        private ScrollRect _scrollRect;
        private bool _dataLoadingStarted = false;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();

            _scrollRect.onValueChanged.AddListener(OnScoll);
        }

        private void OnScoll(Vector2 pos)
        {
            if (_scrollRect.normalizedPosition.y < 1f)
            {
                if (!_dataLoadingStarted)
                {
                    Events.OnScrolledToBottom.Invoke(scrollPanel);

                    _dataLoadingStarted = true;
                }
            }
            else
                _dataLoadingStarted = false;
        }
    }
}