using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelControl : MonoBehaviour
{
    public enum GameTag { All, a, b, c, d, e, f };
    public static bool isComplete = true, isIncomplete = true;

    public static int countPaint
    {
        get { return PlayerPrefs.GetInt("Paintcount", 3); }
        set { PlayerPrefs.SetInt("Paintcount", value); }
    }
}
