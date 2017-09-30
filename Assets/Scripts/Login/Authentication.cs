using UnityEngine;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using DCIL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Authentication : Singleton<Authentication> {

    private string url = "https://fedstudiomini3.deloittenet.com/techfluency";

    public bool isAuthenticated = false;
    
    [TextArea(1,20)]
    public string accessToken = "";
    [TextArea(1,20)]
    public string refreshToken = "";


    // Events
    public delegate void LoginError();
    public static event LoginError OnLoginError;
    public delegate void LoginSuccess();
    public static event LoginSuccess OnLoginSuccess;

    public IEnumerator Start()
    {
        yield return null;
    }

    public void Login(string username, string password)
    {
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            NewSession(username, password);
    }

    public void NewSession(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            Debug.Log("Please enter both a username and password to proceed");
        
        var creds = new JObject();
        creds["username"] = username;
        creds["password"] = password;

        Debug.Log("New login attempt");
        HTTP.Instance.Post<HTTP.Response>(url + "/login", null, JsonConvert.SerializeObject(creds))
            .Fail(error => {
                Debug.Log(error);
                if(OnLoginError != null)
                {
                    OnLoginError();
                }
            })
            .Done(response => {
                if(response.body == "Unauthorized")
                {
                    Debug.Log(response.body);
                    return;
                }
                JObject body = (JObject)JsonConvert.DeserializeObject(response.body);
                DataStore.Instance["access_token"] = accessToken = (string)body["access_token"];
                DataStore.Instance["refresh_token"] = refreshToken = (string)body["refresh_token"];
                DataStore.Instance["user_profile"] = JsonConvert.SerializeObject(body["user_profile"]);
                Debug.Log("New session started. Welcome " + body["user_profile"]["FirstName"] + ", " + body["user_profile"]["LastName"]);
                isAuthenticated = true;
                DataStore.Instance.Save();
                if(OnLoginSuccess != null)
                {
                    OnLoginSuccess();
                }
            });
    }
    public void Ping()
    {
        StartCoroutine(_Ping());
    }

    public IEnumerator _Ping()
    {
        DataStore.Instance.Load();
        yield return new WaitUntil(() => DataStore.Instance.isLoaded);
        accessToken = (string)DataStore.Instance["access_token"];
        refreshToken = (string)DataStore.Instance["refresh_token"];
        HTTP.Instance.Get<HTTP.Response>(url + "/example/1", new Dictionary<string, string> {
            { "Authorization", "Bearer " + accessToken },
            { "Refresh-Token", refreshToken }
        })
        .Fail(error => {
            Debug.Log(error);
            if (OnLoginError != null){
                OnLoginError();
            }
        })
        .Done(response => {
            Debug.Log(response.body);
            if (response.body.Contains("\"status\":401"))
            {
                if (OnLoginError != null)
                {
                    OnLoginError();
                }
            }
            else
            {
                isAuthenticated = true;
                if (OnLoginSuccess != null)
                {
                    OnLoginSuccess();
                }
            }            
        });
    }

    public void Logout()
    {
        if (string.IsNullOrEmpty(accessToken) == false)
        {
            DataStore.Instance.Remove("access_token"); 
            accessToken = "";
        }

        if (string.IsNullOrEmpty(refreshToken) == false)
        {
            DataStore.Instance.Remove("refresh_token");
            refreshToken = "";
        }

        if (string.IsNullOrEmpty((string)DataStore.Instance["user_profile"]) == false)
        {
            DataStore.Instance.Remove("user_profile");
        }

        isAuthenticated = false;
        DataStore.Instance.Save();
    }
}
