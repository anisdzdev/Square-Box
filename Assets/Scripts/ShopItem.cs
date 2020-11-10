using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopItem : MonoBehaviour {


	public string ItemName = "Name";
	public int Id = 0;
	public Sprite image;
	public int cost;
	public Image _myImg;
	public Image Background;
	public Sprite PrefferedSprite;
	public Sprite[] SpriteList;
	public Text _myText;
	public bool bought;
	public bool usingitem;
	public GameObject buyButton;
	public GameObject useButton;
	public GameObject usingButton;
	int money;


	void Start(){
		PlayerPrefs.SetInt ("RedBox",1);
		if (PlayerPrefs.GetInt (ItemName) == 1) {
			bought = true;
		} else {
			bought = false;
		}

		if (bought) {
			if (PlayerPrefs.GetInt ("box") == Id) {
				usingitem = true;
				buyButton.SetActive (false);
				useButton.SetActive (false);
				usingButton.SetActive (true);
			} else {
				buyButton.SetActive (false);
				useButton.SetActive (true);
				usingButton.SetActive (false);
			}
		}

		if (PrefferedSprite != null) {
			Background.sprite = PrefferedSprite;
		} else {
			float randomvalue = Random.Range (0, 4);
			if (randomvalue >= 0 && randomvalue < 1) {
				Background.sprite = SpriteList [0];
			}else if (randomvalue >= 1 && randomvalue < 2) {
				Background.sprite = SpriteList [1];
			}else if (randomvalue >= 2 && randomvalue < 3) {
				Background.sprite = SpriteList [2];
			}else if (randomvalue >= 3 && randomvalue <= 4) {
				Background.sprite = SpriteList [3];
			}
		}
	}

	void Update(){
		money = FindObjectOfType<MoveOnTrack> ().money;
		if (bought) {

			if (Id != 15000) {
				if (PlayerPrefs.GetInt ("box") == Id) {
					usingitem = true;
					buyButton.SetActive (false);
					useButton.SetActive (false);
					usingButton.SetActive (true);
				} else {
					usingitem = false;
					buyButton.SetActive (false);
					useButton.SetActive (true);
					usingButton.SetActive (false);
				}
			} else {
				if (PlayerPrefs.GetInt ("nMode") == 1) {
					usingitem = true;
					buyButton.SetActive (false);
					useButton.SetActive (false);
					usingButton.SetActive (true);
				} else {
					usingitem = false;
					buyButton.SetActive (false);
					useButton.SetActive (true);
					usingButton.SetActive (false);
				}
			}
		}

		if (money < cost) {
			buyButton.GetComponent<Button> ().interactable = false;
		} else {
			buyButton.GetComponent<Button> ().interactable = true;
		}

		if (cost != 0){
			_myText.text = cost.ToString ();
		}
		_myImg.sprite = image;

	}

	public void BuyItem(){
		
		FindObjectOfType<MoveOnTrack> ().money = money - cost;
		bought = true;
		PlayerPrefs.SetInt (ItemName, 1);
	}

	public void UseItem(){
		usingitem= true;
		if(Id == 15000){
			PlayerPrefs.SetInt ("nMode", 1);
		}else{
			PlayerPrefs.SetInt ("box", Id);
		}
		/*ShopItem[] items = FindObjectsOfType<ShopItem> ();
		foreach (ShopItem item in items) {
			
			item.usingitem = false;
		}*/
	}

	public void unuse(){
		usingitem = false;
		if(Id == 15000){
			PlayerPrefs.SetInt ("nMode", 0);
		}
	}

}
