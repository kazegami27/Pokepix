using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using System.Linq;
using System.IO;
#pragma warning disable 0618

public class GameController : BaseController {

    [Header("Pixel images settings")]
    public Sprite[] colorImages;
    public Sprite[] grayImages;
    public bool[] videoToUnlocks;

    [Header("")]
    public RectTransform canvasTr;
    public FooterButton[] footerbuttons;
    public GridLayoutGroup gridLayout, myWorkGridLayout, createGridLayout;
    public Board board;
    public Slider slider;
    public LibraryItem libraryItemPrefab;
    public GameObject[] tabContents;
    public GameObject nowork;
    public GameObject shareButton, shareAndReplayGroup;
    public Image createImage;
    public Text PaintNumb;
    public Image EndGameImg;
    public GameObject popUploading;

    public GameObject canvasHome, canvasMain, mainScreenObjects;
    public GameObject completeDialog, dialogOverlay;
    public Transform completeInTr, completeOutTr;
    public GameObject createListItems, createScreen;
    public CreateManager createManager;
    public Transform downloadingItemsTr;
    public Text removeAdText;
    public GameObject removeAdSection;

    [HideInInspector]
    public float cellSize;

    private string KeyPref = "library_items";
    private List<string> allItems = new List<string>();
    List<GameObject> allitemBT = new List<GameObject>();
    private int currentTab;

    public static GameController instance;

    protected override void Awake()
    {
        //base.Awake();
        //Application.targetFrameRate = 60;
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        SetFooterActive(0);

        int column = (int)Mathf.Ceil(canvasTr.rect.width / 1550f);
        column = Mathf.Max(column, 2);

        gridLayout.constraintCount = column;
        cellSize = (canvasTr.rect.width - (column - 1) * gridLayout.spacing.x) / (float)column;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        myWorkGridLayout.constraintCount = column;
        myWorkGridLayout.cellSize = new Vector2(cellSize, cellSize);

        createGridLayout.constraintCount = column;
        createGridLayout.cellSize = new Vector2(cellSize, cellSize);

        removeAdSection.SetActive(Purchaser.instance.isEnabled && !CUtils.IsAdsRemoved());
        removeAdText.text = string.Format(removeAdText.text, Purchaser.instance.iapItems[0].price);

        int count = colorImages.Length;
        for (int i = 0; i < count; i++)
        {
            if (colorImages[i] == null || grayImages[i] == null) continue;

            LibraryItem item = Instantiate(libraryItemPrefab);
            item.transform.SetParent(gridLayout.transform);
            item.transform.localScale = Vector3.one;
            item.colorImage = colorImages[i];
            item.grayImage = grayImages[i];
            item.itemName = colorImages[i].name;
            item.videoToUnlock = videoToUnlocks.Length > i ? videoToUnlocks[i] : false;
            allitemBT.Add(item.gameObject);
            allItems.Add(item.itemName);
        }

        var downloadedItems = GetDownloadedItems();
        foreach (var lItem in downloadedItems)
        {
            AddItemToHome(lItem);
            allItems.Add(lItem.itemName);
        }

        ChangeGameMusic();

        StartCoroutine(GetDataFromServer(GameConfig.instance.jsonUrl));
    }

    private IEnumerator GetDataFromServer(string url)
    {
        if (string.IsNullOrEmpty(url)) yield break;

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "secret-key", GameConfig.instance.jsonBinSecretKey }
        };

        WWW www = new WWW(url, null, headers);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Error: GetDataFromServer - " + www.error);
            yield break;
        }

        if (string.IsNullOrEmpty(www.text)) yield break;

        var latestItems = GetLibraryItems(www.text);
        if (latestItems != null && latestItems.Count > 0)
        {
            PlayerPrefs.SetString(KeyPref, www.text);
        }

        foreach (var lItem in latestItems)
        {
            if (!allItems.Contains(lItem.itemName))
            {
                AddItemToHome(lItem, true);
                allItems.Add(lItem.itemName);
            }
        }
    }

    private void AddItemToHome(LItem lItem, bool isDownloading = false)
    {
        LibraryItem item = Instantiate(libraryItemPrefab);

        if (isDownloading)
        {
            item.transform.SetParent(downloadingItemsTr);
            item.parentTr = gridLayout.transform;
        }
        else
        {
            item.transform.SetParent(gridLayout.transform);

            if (GameConfig.instance.addItemsToTop)
                item.transform.SetAsFirstSibling();
            else
                item.transform.SetAsLastSibling();
        }

        item.transform.localScale = Vector3.one;
        item.lItem = lItem;
    }

    private List<LItem> GetDownloadedItems()
    {
        if (!PlayerPrefs.HasKey(KeyPref))
        {
            return new List<LItem>();
        }

        string data = PlayerPrefs.GetString(KeyPref);
        return GetLibraryItems(data);
    }

    private List<LItem> GetLibraryItems(string data)
    {
        return JsonUtility.FromJson<LItems>(data).items;
    }

    private void OnMyWorkClick()
    {
        for (int i = myWorkGridLayout.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(myWorkGridLayout.transform.GetChild(i).gameObject);
        }

        var myWorkItems = CUtils.BuildListFromString<string>(PlayerPrefs.GetString("mywork_items"));
        if (myWorkItems.Count == 0)
        {
            nowork.SetActive(true);
            return;
        }

        nowork.SetActive(false);

        foreach (var itemName in myWorkItems)
        {
            LibraryItem item = Instantiate(libraryItemPrefab);
            item.transform.SetParent(myWorkGridLayout.transform);
            item.transform.localScale = Vector3.one;
            item.itemName = itemName;


            for (int i = 0; i < colorImages.Length; i++)
            {
                if (colorImages[i].name == itemName)
                {
                    item.colorImage = colorImages[i];
                    item.grayImage = grayImages[i];
                    break;
                }
            }
        }

        if (myWorkItems.Count == 1)
        {
            GameObject empty = new GameObject("Empty");
            empty.AddComponent<RectTransform>();
            empty.transform.SetParent(myWorkGridLayout.transform);
        }
    }

    private void OnCreateTabClick()
    {
        createListItems.SetActive(true);
        createScreen.SetActive(false);

        for (int i = createGridLayout.transform.childCount - 1; i > 0; i--)
        {
            DestroyImmediate(createGridLayout.transform.GetChild(i).gameObject);
        }

        var myCreateItems = CUtils.BuildListFromString<string>(PlayerPrefs.GetString("mycreate_items"));

        foreach (var itemName in myCreateItems)
        {
            LibraryItem item = Instantiate(libraryItemPrefab);
            item.transform.SetParent(createGridLayout.transform);
            item.transform.localScale = Vector3.one;
            item.itemName = itemName;
        }

        if (myCreateItems.Count == 0)
        {
            GameObject empty = new GameObject("Empty");
            empty.AddComponent<RectTransform>();
            empty.transform.SetParent(createGridLayout.transform);
        }
    }

    public void OnCreateButtonClick()
    {
        Sound.instance.PlayButton();
        Timer.Schedule(this, 0, () =>
        {
#if UNITY_EDITOR
            createListItems.SetActive(false);
            createScreen.SetActive(true);
            createManager.Load();
#elif UNITY_ANDROID || UNITY_IOS
            //PickImage(-1);
#endif
        });
    }

    public void OnBackFromCreateScreen()
    {
        createListItems.SetActive(true);
        createScreen.SetActive(false);
    }

    //private void PickImage(int maxSize)
    //{
    //    NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
    //    {
    //        Debug.Log("Image path: " + path);
    //        if (path != null)
    //        {
    //            // Create Texture from selected image
    //            Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false, false);
    //            if (texture == null)
    //            {
    //                Debug.Log("Couldn't load texture from " + path);
    //                Toast.instance.ShowMessage("Couldn't load texture");
    //                return;
    //            }

    //            createListItems.SetActive(false);
    //            createScreen.SetActive(true);
    //            createManager.LoadImage(texture);
    //        }
    //    }, "Select a PNG image", "image/png", maxSize);

    //    Debug.Log("Permission result: " + permission);
    //}

    public void OnTabClick(int tabIndex)
    {
        SetFooterActive(tabIndex);
        DoTab(tabIndex);

        Sound.instance.PlayButton();
        //AdmobController.instance.ShowInterstitial();
    }

    private void DoTab(int tabIndex)
    {
        if (tabIndex == 2)
        {
            OnMyWorkClick();
            var ShowOff = FindObjectOfType<ViewContentShowingComplete>();
            ShowOff.OnEnable();
            ModelControl.isComplete = false;
            ModelControl.isIncomplete = true;
        }
        else if (tabIndex == 1)
        {
            OnCreateTabClick();

        }
        else if (tabIndex == 0)
        {
            StartCoroutine(GetDataFromServer(GameConfig.instance.jsonUrl));
        }
    }

    public void ShowingDone()
    {
        //OnMyWorkClick();
        ModelControl.isComplete = true;
        ModelControl.isIncomplete = false;
        var view = FindObjectOfType<ViewContentShowingComplete>();
        view.UpdateUI();
    }

    public void ShowingUndone()
    {
        //OnMyWorkClick();
        ModelControl.isComplete = false;
        ModelControl.isIncomplete = true;
        var view = FindObjectOfType<ViewContentShowingComplete>();
        view.UpdateUI();
    }

    public void SetFooterActive(int footerIndex)
    {
        for (int i = 0; i < footerbuttons.Length; i++)
        {
            footerbuttons[i].SetActive(i == footerIndex);
            tabContents[i].GetComponent<CanvasGroup>().alpha = i == footerIndex ? 1 : 0;
            tabContents[i].GetComponent<CanvasGroup>().blocksRaycasts = i == footerIndex;
        }
        currentTab = footerIndex;
    }

    public void OnLibraryItemClicked(LibraryItem item)
    {
        AdmobController.instance.ID_AdInter = "ca-app-pub-4973559944609228/3155501636";
        popUploading.SetActive(true);
        Timer.Schedule(this, .5f, () =>
          {
              AdmobController.instance.ShowInterstitial();
          });
        EndGameImg.sprite = item.colorImage;
        var a = (ModelControl.countPaint > 0) ? PaintNumb.text = ModelControl.countPaint.ToString() : PaintNumb.text = "AD";
        GameState.colorImage = item.colorImage;
        GameState.grayImage = item.grayImage;
        GameState.libraryItemName = item.itemName;

        GotoMainScreen();

    }

    public void GotoMainScreen()
    {
        Timer.Schedule(this, 0, () =>
        {
            canvasHome.GetComponent<CanvasGroup>().LeanAlpha(0, .5f);
            canvasHome.GetComponent<CanvasGroup>().blocksRaycasts = false;
            canvasMain.GetComponent<CanvasGroup>().LeanAlpha(1, .5f);
            canvasMain.GetComponent<CanvasGroup>().blocksRaycasts = true;
            mainScreenObjects.SetActive(true);
        });

        Sound.instance.PlayButton();
        ChangeGameMusic();
    }

    public void OnMainBackClick()
    {
        //AdmobController.instance.ShowInterstitial();
        Sound.instance.PlayButton();

        board.SaveProgress();
        dialogOverlay.SetActive(false);
        completeDialog.GetComponent<CanvasGroup>().LeanAlpha(0, .6f);
        completeDialog.GetComponent<CanvasGroup>().blocksRaycasts = false;

        canvasHome.GetComponent<CanvasGroup>().alpha = 1;
        canvasHome.GetComponent<CanvasGroup>().blocksRaycasts = true;
        canvasMain.GetComponent<CanvasGroup>().alpha = 0;
        canvasMain.GetComponent<CanvasGroup>().blocksRaycasts = false;
        mainScreenObjects.SetActive(false);
        board.ResetVariables();

        ChangeGameMusic();

        DoTab(currentTab);

        updateBT();
    }

    public void updateBT()
    {
        var tempList = allitemBT.FindAll(x => x.name.Contains(nameBT));
        foreach (var item in tempList)
        {
            item.GetComponent<LibraryItem>().UpDateOneTime();
        }

        gridLayout.GetComponent<IUSoulMate>().UpdateChild();
    }

    public string nameBT;

    public void OnComplete(float delay, bool canReplay)
    {
        AdmobController.instance.ID_AdInter = "ca-app-pub-4973559944609228/8216256627";
        Timer.Schedule(this, 0.4f, () =>
        {
            AdmobController.instance.ShowInterstitial();
        });

        Timer.Schedule(this, delay, () =>
        {
            slider.gameObject.SetActive(false);
            dialogOverlay.SetActive(true);
            //completeDialog.SetActive(true);
            shareButton.SetActive(!canReplay);
            shareAndReplayGroup.SetActive(canReplay);
            //TestingReCap.instance.ComPletePicture();

            //completeDialog.transform.position = completeOutTr.position;
            completeDialog.GetComponent<CanvasGroup>().LeanAlpha(1, .6f);
            completeDialog.GetComponent<CanvasGroup>().blocksRaycasts = true;
        });

    }

    public void showAss()
    {
        AdmobController.instance.ID_AdInter = "ca-app-pub-4973559944609228/7342158657";
        AdmobController.instance.ShowInterstitial();
    }
    public void CloseCompleteDialog()
    {
        dialogOverlay.SetActive(false);
        //iTween.MoveTo(completeDialog, completeOutTr.position, 0.3f);

        Timer.Schedule(this, 0.3f, () =>
        {
            //completeDialog.SetActive(false);
        });
        Sound.instance.PlayButton();
    }

    public void ShareCompleteGame()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //CloseCompleteDialog();
        StartCoroutine(ShareScreenshot());
#else
        Toast.instance.ShowMessage("This function only works on Android and iOS");
#endif
        Sound.instance.PlayButton();
    }

    private IEnumerator ShareScreenshot()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForEndOfFrame();
        Sharing.ShareScreenshot("screenshot", "");
    }

    public void ReplayGame()
    {
        CloseCompleteDialog();

        Timer.Schedule(this, 0.3f, () =>
        {
            board.DeleteProgress();
            board.ResetVariables();
            board.OnEnable();
            ChangeGameMusic();
        });
    }

    float lastChangeTime = int.MinValue;
    int selectMusicIndex;
    private void ChangeGameMusic()
    {
        if (Time.time - lastChangeTime > 60)
        {
            Music.Type[] types = { Music.Type.MainMusic1, Music.Type.MainMusic2, Music.Type.MainMusic3 };
            Music.instance.Play(types[selectMusicIndex]);
            selectMusicIndex = (selectMusicIndex + 1) % 3;

            lastChangeTime = Time.time;
        }
    }

    public void OpenStore(string url)
    {
        Application.OpenURL(url);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && canvasHome.GetComponent<CanvasGroup>().alpha==0)
        {
#if !UNITY_IOS
            Application.Quit();
#endif
        }
    }

    public void OnRemoveAdClick()
    {
#if IAP && UNITY_PURCHASING
        Sound.instance.PlayButton();
        Purchaser.instance.BuyProduct(0);
#else
        Debug.LogError("Please enable, import and install Unity IAP to use this function");
#endif
    }

    public void OnRestorePurchase()
    {
#if IAP && UNITY_PURCHASING
        Sound.instance.PlayButton();
        Purchaser.instance.RestorePurchases();
#else
        Debug.LogError("Please enable, import and install Unity IAP to use this function");
#endif
    }
}
