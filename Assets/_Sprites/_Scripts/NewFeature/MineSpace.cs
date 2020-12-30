using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineSpace : MonoBehaviour
{
    //public static int isComplete;
    [SerializeField]
    Text[] Text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClcick(int ID)
    {
        if (ID==1)
        {
            Text[0].color = Color.red;
            Text[1].color = Color.gray;
            Text[2].color = Color.gray;
        }
        else if (ID == 2)
        {         
            Text[0].color = Color.gray;
            Text[1].color = Color.red;
            Text[2].color = Color.gray;
        }
        else if (ID == 3)
        {
            Text[0].color = Color.gray;
            Text[1].color = Color.gray;
            Text[2].color = Color.red;
        }

    }

    public void SetColor()
    {
        Text[0].color = Color.red;
        Text[1].color = Color.gray;
        Text[2].color = Color.gray;
    }
}
