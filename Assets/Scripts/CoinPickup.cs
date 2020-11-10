using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {



    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, 200 * Time.deltaTime); 
		
	}


    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "PlayerSubObject") {

			other.transform.GetComponentInParent<MoveOnTrack>().money++;
            other.transform.GetComponentInParent<AudioSource>().PlayOneShot(other.transform.GetComponentInParent<MoveOnTrack>().CoinPickSound);
            //Add 1 point to Coins
            Destroy(this.gameObject); // Destroy things.

		}
		if (other.gameObject.name == "Player") {

			other.GetComponent<MoveOnTrack>().money++;
            other.GetComponent<AudioSource>().PlayOneShot(other.GetComponent<MoveOnTrack>().CoinPickSound);
            //Add 1 point to Coins
            Destroy(this.gameObject); // Destroy things.

		}

	}



}
