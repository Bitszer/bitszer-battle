using TMPro;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

public class UserAuth : MonoBehaviour
{
    [Header("UIManager")]
    public UIManager uiManager;

    [Header("Login UI")]
    public TMP_InputField emailLoginInputField;
    public TMP_InputField passwordLoginInputField;

    [Header("Signup UI")]
    public TMP_InputField fullNameSignupInputField;
    public TMP_InputField emailSignupInputField;
    public TMP_InputField passwordSignupInputField;
    public TMP_InputField confirmPasswordSignupInputField;

    private string poolId = "us-west-2_wItToCbsB";
    private string clientId = "553o5tjm99c10p22m6aopmtaat";

    private void Start()
    {
        emailLoginInputField.text = "dhaval3879@gmail.com";
        passwordLoginInputField.text = "sumeru@1234#";

        if(PlayerPrefs.HasKey("email") && PlayerPrefs.HasKey("password"))
        {
            LoginUser(PlayerPrefs.GetString("email"), PlayerPrefs.GetString("password"));
        }
    }

    public void SignUpUser()
    {
        RegisterUser();
    }

    public void SignInUser()
    {
        LoginUser(emailLoginInputField.text, passwordLoginInputField.text);
    }

    private async Task LoginUser(string email, string password)
    {
        DataProvider.Instance.loadingPanel.SetActive(true);

        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USWest2);

        CognitoUserPool userPool = new CognitoUserPool(poolId, clientId, provider);

        CognitoUser user = new CognitoUser(email, clientId, userPool, provider);

        InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
        {
            Password = password,
        };

        try
        {
            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

            GetUserRequest getUserRequest = new GetUserRequest();
            getUserRequest.AccessToken = authResponse.AuthenticationResult.AccessToken;

            DataProvider.Instance.graphApi.SetAuthToken(getUserRequest.AccessToken);

            Debug.Log("User Access Token: " + getUserRequest.AccessToken);

            UnityMainThread.wkr.AddJob(() =>
            {
                PlayerPrefs.SetString("email", email);
                PlayerPrefs.SetString("password", password);

                DataProvider.Instance.GetMyProfile();
                DataProvider.Instance.loadingPanel.SetActive(false);
                uiManager.OpenTabPanel();
            });
        }
        catch(Exception e)
        {
            Debug.Log("EXCEPTION" + e);
            return;
        }
    }

    private async Task RegisterUser()
    {
        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USWest2);

        SignUpRequest signUpRequest = new SignUpRequest()
        {
            ClientId = clientId,
            Username = emailSignupInputField.text,
            Password = passwordSignupInputField.text,
        };

        List<AttributeType> attributes = new List<AttributeType>()
        {
            new AttributeType() { Name = "fullName", Value = fullNameSignupInputField.text },
            new AttributeType() { Name = "email", Value = emailSignupInputField.text },
        };

        signUpRequest.UserAttributes = attributes;

        try
        {
            SignUpResponse request = await provider.SignUpAsync(signUpRequest);
            Debug.Log("Signed up");
        }
        catch(Exception e)
        {
            Debug.Log("Exception" + e);
            return;
        }
    }
}