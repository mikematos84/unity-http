using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using uPromise;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DCIL
{
    public class HTTP : Singleton<HTTP>
    {
        protected HTTP() { }

        public Promise<T> Get<T>(string url, Dictionary<string, string> headers = null, string body = null)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            return WebRequest<T>(www, headers, body);
        }

        public Promise<T> Post<T>(string url, Dictionary<string, string> headers = null, string body = null)
        {
            UnityWebRequest www = UnityWebRequest.Post(url, body);
            return WebRequest<T>(www, headers, body);
        }

        public Promise<T> Put<T>(string url, Dictionary<string, string> headers = null, string body = null)
        {
            UnityWebRequest www = UnityWebRequest.Put(url, body);
            return WebRequest<T>(www, headers, body);
        }

        public Promise<T> Delete<T>(string url, Dictionary<string, string> headers = null, string body = null)
        {
            UnityWebRequest www = UnityWebRequest.Delete(url);
            return WebRequest<T>(www, headers, body);
        }

        public Promise<T> WebRequest<T>(UnityWebRequest www, Dictionary<string, string> headers = null, string body = null)
        {
            Deferred<T> deferred = new Deferred<T>();

            // headers
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }
            headers["Content-Type"] = "application/json";
            
            foreach (KeyValuePair<string, string> header in headers)
            {
                if (header.Key != null && header.Value != null)
                {
                    www.SetRequestHeader(header.Key, header.Value);
                }
            }

            //upload handler
            if (string.IsNullOrEmpty(body) == false)
            {
                byte[] bytes = new System.Text.UTF8Encoding().GetBytes(body);
                www.uploadHandler = new UploadHandlerRaw(bytes);
            }

            //download handler
            www.downloadHandler = new DownloadHandlerBuffer();

            StartCoroutine(_WebRequest(deferred, www));
            return deferred.Promise;
        }

        public IEnumerator _WebRequest<T>(Deferred<T> deferred, UnityWebRequest www)
        {
            using (www)
            {
                yield return www.Send();

                //get response headers
                var response = new Response();
                response.headers = www.GetResponseHeaders();
                response.www = www;
                response.body = www.downloadHandler.text;
                response.error = www.error;

                if (www.isError)
                {
                    deferred.Reject(response);
                }
                else
                {
                    if (www.isDone)
                    {
                        deferred.Resolve(response);
                    }
                }
            }
        }
        
        public class Response
        {
            public string error { get; set; }
            public string body { get; set; }
            public Dictionary<string, string> headers { get; set; }
            public UnityWebRequest www;
        }
    }    
}
