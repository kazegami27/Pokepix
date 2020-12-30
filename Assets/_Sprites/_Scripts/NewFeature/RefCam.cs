using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefCam : MonoBehaviour
{
    public Camera cammini, fixcam;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        //cammini.orthographicSize = fixcam.orthographicSize;
        //obj.transform.localScale = new Vector2(cammini.orthographicSize / 2, cammini.orthographicSize / 2);
    }

    private void OnEnable()
    {
        StartCoroutine(setup());
    }

    IEnumerator setup()
    {
        yield return new WaitForSeconds(.1f);
        cammini.orthographicSize = fixcam.orthographicSize /1.5f;
        obj.transform.localScale = new Vector2(cammini.orthographicSize / 8, cammini.orthographicSize / 8);
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
}
