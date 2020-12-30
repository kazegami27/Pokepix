using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewContentShowingComplete : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        if (gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (ModelControl.isComplete)
                {
                    if (gameObject.transform.GetChild(i).transform.GetComponent<LibraryItem>().itemStatus == "complete")
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (gameObject.transform.GetChild(i).transform.GetComponent<LibraryItem>().itemStatus == "complete")
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }

                if (ModelControl.isIncomplete)
                {
                    if (gameObject.transform.GetChild(i).transform.GetComponent<LibraryItem>().itemStatus == "painting")
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (gameObject.transform.GetChild(i).transform.GetComponent<LibraryItem>().itemStatus == "painting")
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }

        }
    }

    public void OnEnable()
    {
        StartCoroutine(Start());
    }

    public void UpdateUI()
    {
        StartCoroutine(Start());
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
