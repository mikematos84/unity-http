using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour {

    public InputField username;
    public InputField password;
    public Text status;

    public void Start()
    {
        Authentication.Instance.Ping();
        Authentication.OnLoginError += Authentication_OnLoginError;
        Authentication.OnLoginSuccess += Authentication_OnLoginSuccess;
    }

    public void OnLoginClick()
    {
        Authentication.Instance.Login(username.text, password.text);
    }

    public void OnLogoutClick()
    {
        Authentication.Instance.Logout();
    }

    private void Authentication_OnLoginSuccess()
    {
        Debug.Log("Success login");
    }

    private void Authentication_OnLoginError()
    {
        Debug.Log("Login Error");
    }

    public void OnDisable()
    {
        Authentication.OnLoginError -= Authentication_OnLoginError;
        Authentication.OnLoginSuccess -= Authentication_OnLoginSuccess;
    }
}
