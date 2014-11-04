using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public Team owner;
	public Vector3 velocity;
	public bool isNeutral;


	void OnTriggerEnter2D(Collider2D other){
		//decrement health
	}
	bool IsFree() {
		return isNeutral;
	}

	void SetColor(Team owner){
		//if owner is neutral, turn ball white

	}

	void MoveBall(){
		//if magnitude of velocity < .1, turn white
		//make it stop
	}

	// Use this for initialization
	void Start () {
		isNeutral = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
