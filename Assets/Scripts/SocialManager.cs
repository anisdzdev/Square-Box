using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;

public class SocialManager : MonoBehaviour
{
	
	void Start () {
        // Authenticate and register a ProcessAuthentication callback
        // This call needs to be made before we can proceed to other calls in the Social API
#if UNITY_ANDROID
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesClientConfiguration config = new
            PlayGamesClientConfiguration.Builder()
            .Build();

        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;

        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(SignInCallback, true);
#endif

        Social.localUser.Authenticate (ProcessAuthentication);
	}

	// This function gets called when Authenticate completes
	// Note that if the operation is successful, Social.localUser will contain data from the server. 
	void ProcessAuthentication (bool success) {
		
		if (success){ 
			GetComponent<MoveOnTrack>().authenticated = true;
			Debug.Log ("Authenticated, checking achievements");
		}
		
		else{ 
			GetComponent<MoveOnTrack>().authenticated = false;
			Debug.Log ("Failed to authenticate");
		}
		
	}

    public void SignInCallback(bool success)
    {
        if (success)
        {
            GetComponent<MoveOnTrack>().authenticated = true;
            Debug.Log("Signed in!");
        }
        else
        {
            Debug.Log("Sign-in failed...");
        }
    }

    // This function gets called when the LoadAchievement call completes

}
