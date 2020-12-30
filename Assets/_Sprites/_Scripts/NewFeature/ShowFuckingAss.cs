using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFuckingAss : BaseController
{
    public GameObject gamecontrol, content, libra, main;
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        StartCoroutine(showAss());
    }

   
    IEnumerator showAss()
    {
        yield return new WaitForSeconds(.1f);
        gamecontrol.SetActive(true);

        yield return new WaitForSeconds(.1f);
        libra.SetActive(true);

        yield return new WaitForSeconds(.1f);
        content.SetActive(true);

        yield return new WaitForSeconds(.1f);
        main.SetActive(true);

        yield return new WaitForSeconds(.5f);

        AdmobController.instance.ShowInterstitial();

        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}
