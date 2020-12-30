using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public float timeCount;

    private void OnEnable()
    {
        timeCount = 5;
        StopCoroutine(Off());
        StartCoroutine(Off());
    }

    IEnumerator Off()
    {
        while(true)
        {         
            if(timeCount>0)
            {
                timeCount--;
                yield return new WaitForSeconds(1);
            }
            else
            {
                gameObject.SetActive(false);
                break;
            }
        }
    }
}
