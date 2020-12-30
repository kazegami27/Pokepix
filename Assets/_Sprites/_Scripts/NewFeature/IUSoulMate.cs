using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IUSoulMate : MonoBehaviour
{
    //[System.Serializable]
    //public class IULover
    //{
    //    public List<GameObject> ShowContent;
    //}

    //public List<IULover> HandleMyLove;
    public List<GameObject> AllItem;
    public List<GridLayoutGroup> grit;
    public LibraryItem libraryItemPrefab;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        var gamecontrol = GameController.instance;
        for (int i = 0; i < grit.Count; i++)
        {
            int column = (int)Mathf.Ceil(gamecontrol.canvasTr.rect.width / 450f);
            column = Mathf.Max(column, 2);

            grit[i].constraintCount = column;
            var cellSize = (gamecontrol.canvasTr.rect.width - (column - 1) * grit[i].spacing.x) / (float)column;
            grit[i].cellSize = new Vector2(cellSize, cellSize);
        }
        foreach (Transform child in transform)
        {
            //yield return new WaitForSeconds(.01f);
            AllItem.Add(child.gameObject);
            if(child.name.Contains("christmas"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[0].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if(child.name.Contains("cartoon"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[1].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Scenery"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[2].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Fashion"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[3].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Game"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[4].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Food"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[5].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Animal"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[6].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Character"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[7].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Vehicle"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[8].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Painting"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[9].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
            else if (child.name.Contains("Pokepix"))
            {
                GameObject item = Instantiate(child.gameObject);
                item.transform.SetParent(grit[10].transform);
                item.transform.localScale = Vector3.one;
                item.GetComponent<Image>().sprite = child.GetComponent<Image>().sprite;
                item.GetComponent<LibraryItem>().itemName = child.GetComponent<LibraryItem>().itemName;
            }
        }
    }

    public void UpdateChild()
    {
        var templist = AllItem.FindAll(x => x.name.Contains(GameController.instance.nameBT));
        foreach (var item in templist)
        {
            if (item.name.Contains("christmas"))
            {
                foreach (Transform child in grit[0].transform)
                {
                    if(child.GetComponent<LibraryItem>().itemName== GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("cartoon"))
            {
                foreach (Transform child in grit[1].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Scenery"))
            {
                foreach (Transform child in grit[2].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Fashion"))
            {
                foreach (Transform child in grit[3].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Game"))
            {
                foreach (Transform child in grit[4].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Food"))
            {
                foreach (Transform child in grit[5].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Animal"))
            {
                foreach (Transform child in grit[6].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Character"))
            {
                foreach (Transform child in grit[7].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Vehicle"))
            {
                foreach (Transform child in grit[8].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Painting"))
            {
                foreach (Transform child in grit[9].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
            else if (item.name.Contains("Pokepix"))
            {
                foreach (Transform child in grit[10].transform)
                {
                    if (child.GetComponent<LibraryItem>().itemName == GameController.instance.nameBT)
                    {
                        child.GetComponent<LibraryItem>().UpDateOneTime();
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowContent(string theme)
    {
        foreach (var item in AllItem)
        {
            if (theme == ("All"))
            {
                item.SetActive(true);
            }
            else if (item.name.Contains(theme))
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }
        
}
