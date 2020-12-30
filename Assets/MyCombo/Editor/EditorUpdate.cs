using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
class EditorUpdate
{
    static EditorUpdate()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        EditorApplication.update -= Update;

        if (!Directory.Exists("Assets/Plugins/UnityPurchasing/Bin"))
        {
            IAPChecker.CheckItNow();
            return;
        }

        IAPChecker.CheckItNow();
    }
}