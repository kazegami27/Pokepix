using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TestingReCap : MonoBehaviour
{
    public Image image;
    public static TestingReCap instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
       
    }

    public void ComPletePicture()
    {
        var fileName = Path.Combine(Application.persistentDataPath, GameController.instance.nameBT + ".png");
        int size = GameState.colorImage.texture.width;
        var bytes = File.ReadAllBytes(fileName);
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.LoadImage(bytes);
        texture.filterMode = FilterMode.Point;

        Rect rec = new Rect(0, 0, texture.width, texture.height);
        var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
        image.sprite = sprite;
    }
}
