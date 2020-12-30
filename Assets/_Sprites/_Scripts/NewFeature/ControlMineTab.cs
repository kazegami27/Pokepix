using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlMineTab : MonoBehaviour
{
    [SerializeField]
    CanvasGroup MyWork, Create;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnWorkClick()
    {
        MyWork.LeanAlpha(1, .5f);
        MyWork.blocksRaycasts = true;
        Create.LeanAlpha(0, .5f);
        Create.blocksRaycasts = false;
    }

    public void OnCreateClick()
    {
        Create.LeanAlpha(1, .5f);
        Create.blocksRaycasts = true;
        MyWork.LeanAlpha(0, .5f);
        MyWork.blocksRaycasts = false;
    }

    public void OnMineClick()
    {
        MyWork.alpha = 1;
        MyWork.blocksRaycasts = true;
        Create.alpha = 0;
        Create.blocksRaycasts = false;
    }
}
