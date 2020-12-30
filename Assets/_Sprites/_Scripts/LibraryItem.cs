using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LibraryItem : MonoBehaviour {
    public Image image;
    public Sprite colorImage, grayImage;
    public string itemName = null;
    public LItem lItem;
    public bool isItemCreated;
    public bool videoToUnlock;
    public GameObject adLocked;
    IUSoulMate UIMain;

    [HideInInspector]
    public Transform parentTr;

    public string itemStatus;
    private bool initialized;
    private string colorImageFileName, grayImageFileName;

    private void Start()
    {
        if (colorImage == null || grayImage == null)
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                string colorPath = Path.Combine(Application.persistentDataPath, "colorImage_" + itemName + ".png");
                string grayPath = Path.Combine(Application.persistentDataPath, "grayImage_" + itemName + ".png");
                CUtils.LoadFromLocal(ApplyColorImage, colorPath);
                CUtils.LoadFromLocal(ApplyGrayImage, grayPath);
            }
            else
            {
                itemName = lItem.itemName;
                videoToUnlock = lItem.videoToUnlock;

                colorImageFileName = "colorImage_" + itemName + ".png";
                grayImageFileName = "grayImage_" + itemName + ".png";
                StartCoroutine(CUtils.LoadPicture(lItem.colorImageUrl, colorImageFileName, ApplyColorImage));
                StartCoroutine(CUtils.LoadPicture(lItem.grayImageUrl, grayImageFileName, ApplyGrayImage));
            }
        }
        else
        {
            UpdateStatus();
            UpdateUI();
        }
        initialized = true;
    }

    private void ApplyColorImage(Texture2D texture)
    {
        texture.filterMode = FilterMode.Point;
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        colorImage = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

        if (grayImage != null)
        {
            MyUpdate();
        }
    }

    private void ApplyGrayImage(Texture2D texture)
    {
        texture.filterMode = FilterMode.Point;
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        grayImage = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

        if (colorImage != null)
        {
            MyUpdate();
        }
    }

    public void UpDateOneTime()
    {
        if (initialized)
        {
            Start();
        }
    }
    //private void OnEnable()
    //{
    //    if (initialized)
    //    {
    //        Start();
    //    }
    //}

    private void MyUpdate()
    {
        UpdateStatus();
        UpdateUI();

        if (parentTr != null)
        {
            transform.SetParent(parentTr);
            if (GameConfig.instance.addItemsToTop)
                transform.SetAsFirstSibling();
            else
                transform.SetAsLastSibling();
            parentTr = null;
        }
    }

    private void UpdateStatus()
    {
        string prefKey = "item_status_" + itemName;

        UIMain = FindObjectOfType<IUSoulMate>();
        if (!PlayerPrefs.HasKey(prefKey))
        {
            itemStatus = "none";
        }
        else
        {
            itemStatus = PlayerPrefs.GetString(prefKey);
        }
        gameObject.name = itemName;

        //if(itemName.Contains("cartoon"))
        //{
        //    UIMain.HandleMyLove[0].ShowContent.Add(gameObject);
        //}

        //if (itemName.Contains("christmas"))
        //{
        //    UIMain.HandleMyLove[1].ShowContent.Add(gameObject);
        //}
    }

    private void UpdateUI()
    {
        bool isAdUnlocked = CUtils.GetBool("adUnlocked_" + itemName);
        videoToUnlock = videoToUnlock && !isAdUnlocked;
        adLocked.SetActive(videoToUnlock);

        float cellSize = GameController.instance.cellSize * 0.9f;

        if (itemStatus == "none")
        {
            image.sprite = grayImage;
        }
        else if (itemStatus == "complete")
        {
            image.sprite = colorImage;
            //if (ModelControl.isComplete)
            //    gameObject.SetActive(true);
            //else
            //    gameObject.SetActive(false);
        }
        else if (itemStatus == "painting")
        {
            var fileName = Path.Combine(Application.persistentDataPath, itemName + ".png");
            //Debug.LogError(fileName);
            int size = colorImage.texture.width;
            var bytes = File.ReadAllBytes(fileName);
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.LoadImage(bytes);
            texture.filterMode = FilterMode.Point;

            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            image.sprite = sprite;

            //if (ModelControl.isIncomplete)
            //    gameObject.SetActive(true);
            //else
            //    gameObject.SetActive(false);
        }
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);
    }

    public void OnClick()
    {
        if (!videoToUnlock)
            GameController.instance.OnLibraryItemClicked(this);
        else if (IsAdAvailable())
        {
            GameState.itemWatchingAd = this;
#if UNITY_EDITOR
            HandleRewardBasedVideoRewarded();
#elif UNITY_ANDROID || UNITY_IOS
            AdmobController.instance.ShowRewardBasedVideo();
#endif
        }
        else
        {
            Toast.instance.ShowMessage("Ad is not available at the moment");
        }
        GameController.instance.nameBT = itemName;
    }

    [ContextMenu ("Mark this image painted")]
    public void MarkPainted()
    {
        PlayerPrefs.SetString("item_status_" + itemName, "complete");
        image.sprite = colorImage;

        var myWorkItem = CUtils.BuildListFromString<string>(PlayerPrefs.GetString("mywork_items"));
        if (!myWorkItem.Contains(itemName))
        {
            myWorkItem.Add(itemName);
            PlayerPrefs.SetString("mywork_items", CUtils.BuildStringFromCollection(myWorkItem));
        }
    }

    [ContextMenu("Mark this image unpainted")]
    public void MarkUnPainted()
    {
        PlayerPrefs.DeleteKey("item_status_" + itemName);
        image.sprite = grayImage;

        var myWorkItem = CUtils.BuildListFromString<string>(PlayerPrefs.GetString("mywork_items"));
        myWorkItem.Remove(itemName);
        PlayerPrefs.SetString("mywork_items", CUtils.BuildStringFromCollection(myWorkItem));
    }

    public void HandleRewardBasedVideoRewarded()
    {
        videoToUnlock = false;
        adLocked.SetActive(videoToUnlock);
        CUtils.SetBool("adUnlocked_" + itemName, true);
        GameController.instance.OnLibraryItemClicked(this);
    }

    private bool IsAdAvailable()
    {
        if (AdmobController.instance.rewardBasedVideo == null) return false;
        bool isLoaded = AdmobController.instance.rewardBasedVideo.IsLoaded();
        return isLoaded;
    }
}
