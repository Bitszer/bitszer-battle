using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace Bitszer
{
    public enum ErrorType : byte
    {
        InternetError, ServerError, CustomMessage,
    }

    public class APIManager : Singleton<APIManager>
    {
        public Button okayButton;
        public TMP_Text errorText;

        public GameObject errorDialog;
        public GameObject raycastBlocker;

        public ErrorType type;

        public override void Awake()
        {
            base.Awake();

            okayButton.onClick.AddListener(() => OkayButton());
        }

        private void OkayButton()
        {
            errorDialog.SetActive(true);

            switch (type)
            {
                case ErrorType.InternetError:
                    CheckConnection();
                    break;
                case ErrorType.ServerError:
                    errorDialog.SetActive(false);
                    break;
                case ErrorType.CustomMessage:
                    errorDialog.SetActive(false);
                    break;
                default:
                    errorDialog.SetActive(false);
                    break;
            }
        }

        public void SetError(string errorMessage, string buttonText, ErrorType errorType)
        {
            this.errorText.text = errorMessage;
            okayButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = buttonText;
            type = errorType;
            errorDialog.SetActive(true);
        }

        public void RaycastBlock(bool setActive)
        {
            raycastBlocker.SetActive(setActive);
        }

        public IEnumerator CheckInternet(Action<bool> action)
        {
            RaycastBlock(true);
            UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/");
            yield return www.SendWebRequest();

            if (www.error != null)
            {
                action(false);
                SetError("Internet connection is required.", "Try Again", ErrorType.InternetError);
                RaycastBlock(false);
            }
            else
                action(true);
        }

        private void CheckConnection()
        {
            StartCoroutine(CheckInternet(x =>
            {
                if (x)
                    RaycastBlock(false);
            }));
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
    }
}