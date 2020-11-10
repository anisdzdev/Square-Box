using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerSplash : MonoBehaviour {

	public InputField inputField;
	public Button btn;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerPrefs.GetString ("user") != "") {
			SceneManager.LoadScene ("Main");
		} else {

			if (inputField.text == "") {
				btn.interactable = false;
			} else {
				btn.interactable = true;
			}
		}
	}

	public void LoadScene(){
		PlayerPrefs.SetString ("user", inputField.text);
		SceneManager.LoadScene ("Main"); 
	}
		
}
