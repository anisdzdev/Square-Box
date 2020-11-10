using UnityEngine;
using System.Collections;

public class TouchedEnemy : MonoBehaviour {

    // Use this for initialization
    GameObject Char;
    bool dead;

	void Start () {
        Char = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        
        Char.GetComponent<MoveOnTrack>().dead = dead;
    }
    void OnTriggerEnter(Collider player)
    {
        if(player.gameObject.name == "Player")
        {
            dead = true;
            Char.GetComponent<MoveOnTrack>().dead = true;
            Destroy(this.gameObject);
        }
    }
}
