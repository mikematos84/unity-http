using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class REST {
	protected List<IEnumerator> info;
	protected readonly string path;
	protected readonly MonoBehaviour mono;

    public REST (MonoBehaviour mono, string path) {
        this.path = path;
		this.mono = mono;
		info = new List<IEnumerator> ();
	}
    
	public REST Start () {
		if (info.Count > 0) mono.StartCoroutine (Iterate(new List<IEnumerator>(info)));
		info.Clear();
		return this;
	}

    protected IEnumerator Iterate(List<IEnumerator> info)
    {
        while (info.Count > 0)
        {
            yield return mono.StartCoroutine(info[0]);
            info.RemoveAt(0);
        }
    }

    
    public REST Post(string key, string value)
    {
        info.Add(POST(key, value));
        return this;
    }

    protected IEnumerator POST (string key, string value) {
		WWWForm form = new WWWForm();
		form.AddField(key, value);

		using (UnityWebRequest www = UnityWebRequest.Post (path, form)) {
			yield return www.Send ();

			if (www.isError) {
				Debug.Log (www.error);
			}
            else
            {
				Debug.Log ("Done!");
			}
		}
	}

    public REST Get(Action<string> callback)
    {
        info.Add(GET(callback));
        return this;
    }

    protected IEnumerator GET (Action<string> callback) {
		using (UnityWebRequest www = UnityWebRequest.Get(path)) {
			yield return www.Send();

			if (www.isError) {
				Debug.Log (www.error);
			}
			else
            {
				callback (www.downloadHandler.text);
			}
		}
	}
}
