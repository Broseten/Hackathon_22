using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class SearchResponse
{
    [SerializeField]
    public List<string> artworks = new List<string>();
}

public class ArtworkSearch : MonoBehaviour
{
    public string url;

    public void SearchArts(string search)
    {
        StartCoroutine(SearchArtsCoroutine(search));
    }

    IEnumerator SearchArtsCoroutine(string search)
    {
        WWWForm form = new WWWForm();

        string query = url + "?query=" + search;
        Debug.Log(query);

        using (UnityWebRequest www = UnityWebRequest.Get(query))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                SearchResponse arts = JsonUtility.FromJson<SearchResponse>(www.downloadHandler.text);
                Debug.Log(arts.artworks);
                // TODO call art manager.showarts
            }
        }
    }
}
