using TMPro;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

namespace Bitszer
{
    public class UserAuth : MonoBehaviour
    {
        [Header("UIManager")]
        public UIManager uiManager;

        [Header("Login UI")]
        public TMP_Text loginErrorText;
        public TMP_InputField emailLoginInputField;
        public TMP_InputField passwordLoginInputField;

        [Header("Signup UI")]
        public TMP_Text signupErrorText;
        public TMP_InputField emailSignupInputField;
        public TMP_InputField passwordSignupInputField;
        public TMP_InputField confirmPasswordSignupInputField;

        private string poolId = "us-west-2_wItToCbsB";
        private string clientId = "553o5tjm99c10p22m6aopmtaat";

        private AmazonCognitoIdentityProviderClient _provider;

        public async void Start()
        {
            _provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USWest2);

            if (PlayerPrefs.HasKey("email") && PlayerPrefs.HasKey("password"))
                await LoginUser(PlayerPrefs.GetString("email"), PlayerPrefs.GetString("password"));
        }

        public async void SignInUser()
        {
            await LoginUser(emailLoginInputField.text.ToLowerInvariant(), passwordLoginInputField.text);
        }

        public async void SignUpUser()
        {
            await RegisterUser(emailSignupInputField.text.ToLowerInvariant(), passwordSignupInputField.text);
        }

        private async Task LoginUser(string email, string password)
        {
            loginErrorText.gameObject.SetActive(false);

            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                loginErrorText.SetText("All fields are required!");
                loginErrorText.gameObject.SetActive(true);
                return;
            }
            else if (string.IsNullOrEmpty(email))
            {
                loginErrorText.SetText("Email is required!");
                loginErrorText.gameObject.SetActive(true);
                return;
            }
            else if (string.IsNullOrEmpty(password))
            {
                loginErrorText.SetText("Password is required!");
                loginErrorText.gameObject.SetActive(true);
                return;
            }

            APIManager.Instance.RaycastBlock(true);

            CognitoUserPool userPool = new CognitoUserPool(poolId, clientId, _provider);

            CognitoUser user = new CognitoUser(email, clientId, userPool, _provider);

            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
            {
                Password = password,
            };

            try
            {
                AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

                GetUserRequest getUserRequest = new GetUserRequest();
                getUserRequest.AccessToken = authResponse.AuthenticationResult.AccessToken;

                AuctionHouse.Instance.graphApi.SetAuthToken(getUserRequest.AccessToken);

                Debug.Log("User Access Token: " + getUserRequest.AccessToken);

                UnityMainThread.wkr.AddJob(() =>
                {
                    PlayerPrefs.SetString("email", email);
                    PlayerPrefs.SetString("password", password);

                    StartCoroutine(AuctionHouse.Instance.GetMyProfile(result =>
                    {
                        if (result == null)
                        {
                            APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                            APIManager.Instance.RaycastBlock(false);
                            return;
                        }
                    }));

                    uiManager.loginSuccessfullPanel.gameObject.SetActive(true);
                    APIManager.Instance.RaycastBlock(false);
                    uiManager.OpenTabPanel();

                    Events.OnAuctionHouseInitialized.Invoke();
                });
            }
            catch (Exception e)
            {
                UnityMainThread.wkr.AddJob(() =>
                {
                    Debug.Log("EXCEPTION: " + e);

                    loginErrorText.SetText("Something went wrong!");
                    loginErrorText.gameObject.SetActive(true);
                    APIManager.Instance.RaycastBlock(false);

                    return;
                });
            }
        }

        private async Task RegisterUser(string email, string password)
        {
            if ((string.IsNullOrEmpty(password) && string.IsNullOrEmpty(confirmPasswordSignupInputField.text))
            || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPasswordSignupInputField.text)
            || string.IsNullOrEmpty(email))
            {
                signupErrorText.SetText("All fields are required!");
                signupErrorText.gameObject.SetActive(true);
                return;
            }

            if (passwordSignupInputField.text.Length < 8)
            {
                signupErrorText.color = Color.red;
                signupErrorText.SetText("Password must be at least 8 characters long.");
                signupErrorText.gameObject.SetActive(true);
                return;
            }

            if (!confirmPasswordSignupInputField.text.Equals(password))
            {
                signupErrorText.color = Color.red;
                signupErrorText.SetText("Passwords doesn't match");
                signupErrorText.gameObject.SetActive(true);
                return;
            }

            APIManager.Instance.RaycastBlock(true);

            SignUpRequest signUpRequest = new SignUpRequest()
            {
                ClientId = clientId,
                Username = email,
                Password = password,
            };

            List<AttributeType> attributes = new List<AttributeType>()
            {
                new AttributeType() { Name = "email", Value = email },
            };

            signUpRequest.UserAttributes = attributes;

            try
            {
                SignUpResponse request = await _provider.SignUpAsync(signUpRequest);
                Debug.Log("Signed up");

                signupErrorText.color = Color.green;
                signupErrorText.SetText("Registered successfully!\nYou can Login now.");
                uiManager.signupSuccessfullPanel.gameObject.SetActive(true);
                signupErrorText.gameObject.SetActive(true);

                APIManager.Instance.RaycastBlock(false);
            }
            catch (Exception e)
            {
                UnityMainThread.wkr.AddJob(() =>
                {
                    Debug.Log("EXCEPTION: " + e);

                    signupErrorText.color = Color.red;
                    signupErrorText.SetText("Something went wrong!");
                    signupErrorText.gameObject.SetActive(true);
                    APIManager.Instance.RaycastBlock(false);

                    return;
                });
            }
        }
    }
}