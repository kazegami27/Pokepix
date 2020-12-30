using Firebase;
using Firebase.Messaging;
using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitFireBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(continuationAction: task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
           FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;
        });

       
    }

    public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
}
