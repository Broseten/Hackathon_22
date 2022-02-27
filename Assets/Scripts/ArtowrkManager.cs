using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtowrkManager : MonoBehaviour
{
    public GameObject[] arts;
    private GameObject[] activeArts = new GameObject[3];

    private Dictionary<string, GameObject> hashtable = new Dictionary<string, GameObject>();
    private Vector3 pos1 = new Vector3(2, 0, -4);
    private Vector3 pos2 = new Vector3(-3, 0, -4);
    private Vector3 pos3 = new Vector3(0, 0, 4);
    private Vector3[] positions;

    private void Awake()
    {
        positions = new Vector3[] { pos1, pos2, pos3 };
        foreach (GameObject art in arts)
        {
            hashtable.Add(art.name, art);
        }
    }
    //public void ShowArtworks(string[] ids)
    //{
    //    int count = 0;
    //    foreach (var id in ids)
    //    {
    //        GameObject art;
    //        GameObject activeArt;
    //        hashtable.TryGetValue(id, out art);

    //        if (art != null)
    //        {
    //            // deactivate previously active art
    //            hashtable.TryGetValue(activeArts[count], out activeArt);
    //            if (activeArt != null)
    //            {
    //                activeArt.SetActive(false);
    //            }

    //            activeArts[count] = id;
    //            art.SetActive(true);
    //            art.gameObject.transform.position = positions[count];
    //        }
    //    }
    //}
        public void ShowArtworksRandom()
        {
            int count = 0;
            foreach (var a in activeArts)
        {
            a?.SetActive(false);
        }
        for (int i = 0; i<3; i++)
            {
                int random = Random.RandomRange(0, arts.Length);

                GameObject art = arts[random];
                
                    activeArts[count] =art;
                    art.SetActive(true);
                    art.gameObject.transform.position = positions[count];
            count++;
 
            }
        }
}
