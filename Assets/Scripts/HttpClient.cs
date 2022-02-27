using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class HttpClient
{

    IEnumerator Query(string query)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://localhost:5000/query?query=" + query))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                var artWorks = webRequest.downloadHandler.data;
                Debug.Log(artWorks);
            }
        }
    }
}