using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class RestAPI : REST {

    public RestAPI(MonoBehaviour mono, string path) : base(mono , path){}
    
    protected void SetHandlers(UnityWebRequest www)
    {
        SetHandlers(www, new JSONObject());
    }

    protected void SetHandlers(UnityWebRequest www, JSONObject json)
    {
        byte[] body = new System.Text.UTF8Encoding().GetBytes(json.ToString());
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    }

    protected void SetRequestHeaders(UnityWebRequest www)
    {
        SetRequestHeaders(www, new Dictionary<string, string>() { });
    }

    protected void SetRequestHeaders(UnityWebRequest www, Dictionary<string, string> headers)
    {
        www.SetRequestHeader("Content-Type", "application/json");
        
        // if an access token has already been found on the device use it
        string accessToken = (string)DataStore.Instance["access_token"];
        if(string.IsNullOrEmpty(accessToken) == false)
        {
            www.SetRequestHeader("Authorization", "Bearer " + accessToken);
        }

        //if a refresh token has already been found on the device, use it
        string refreshToken = (string)DataStore.Instance["refresh_token"];
        if (string.IsNullOrEmpty(accessToken) == false)
        {
            www.SetRequestHeader("Refresh-Token", refreshToken);
        }


        foreach (KeyValuePair<string, string> header in headers)
        {
            www.SetRequestHeader((string)header.Key, (string)header.Value);
            Debug.Log((string)header.Key + " " + (string)header.Value);
        }
    }

    protected void CheckResponseHeaders(UnityWebRequest www)
    {
        Dictionary<string, string> headers = www.GetResponseHeaders();
        if(headers.ContainsKey("Set-Authorization"))
        {
            DataStore.Instance["access_key"] = headers["Set-Authorization"];
        }
        if (headers.ContainsKey("Set-Refresh"))
        {
            DataStore.Instance["refresh_token"] = headers["Set-Refresh"];
        }

        DataStore.Instance.Save();
    }

    public REST Get(Action<string> callback)
    {
        info.Add(GET(callback));
        return this;
    }

    protected IEnumerator GET(Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(path))
        {
            SetHandlers(www);
            SetRequestHeaders(www);
            yield return www.Send();
            CheckResponseHeaders(www);

            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }
    }


    public REST Post(JSONObject json, Action<string> callback)
    {
        
        info.Add(POST(json, callback));
        return this;
    }

    protected IEnumerator POST (JSONObject json, Action<string> callback) {
        using (UnityWebRequest www = UnityWebRequest.Post(path, json.ToString()))
        { 
            SetHandlers(www, json);
            SetRequestHeaders(www);
            yield return www.Send ();
            CheckResponseHeaders(www);
            
            if (www.isError) {
                Debug.Log(www.error);
			} else {
                callback(www.downloadHandler.text);
			}
		}
	}

    public REST Delete(Action<string> callback = null)
    {
        info.Add(DELETE(callback));
        return this;
    }

    protected IEnumerator DELETE (Action<string> callback = null)
    {
        using(UnityWebRequest www = UnityWebRequest.Delete(path))
        {
            SetHandlers(www);
            SetRequestHeaders(www);
            yield return www.Send();
            CheckResponseHeaders(www);

            if (www.isError)
            {
                if (callback != null)
                {
                    Debug.Log(www.error);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(www.downloadHandler.text);
                }
            }
        }
    }

    public REST Put(JSONObject json, Action<string> callback)
    {
        info.Add(PUT(json, callback));
        return this;
    }

    protected IEnumerator PUT(JSONObject json, Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(path, json.ToString()))
        {
            SetHandlers(www, json);
            SetRequestHeaders(www);
            yield return www.Send();
            CheckResponseHeaders(www);

            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }
    }
    
}
