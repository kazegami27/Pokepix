using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingLobby : MonoBehaviour
{
    public GameObject content, loading, tutorial;
  
    // Start is called before the first frame update
    void Start()
    {
        //if (PlayerPrefs.GetInt("Tutorial", 1) == 1)
        //{
        //    tutorial.SetActive(true);
        //    PlayerPrefs.SetInt("Tutorial", 0);
        //}
        //else
        //{
        //    tutorial.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(content.transform.childCount>0)
        {
            loading.SetActive(false);
        }
    }
}
