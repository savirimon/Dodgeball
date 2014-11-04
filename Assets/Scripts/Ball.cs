using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public Team owner;
	public Vector3 velocity;
	public bool isNeutral;


	void OnTriggerEnter2D(Collider2D other){
		if(isNeutral){
			//owner = other.GetComponent("Player").team;
		}
	}
	bool IsFree() {
		return isNeutral;
	}

	void SetWhite(){
		renderer.material.color = Color.white;
	}

	void SetColor(Team owner){
		switch (owner) {
		case Team.ONE:
			renderer.material.color = Color.cyan;
			break;
		case Team.TWO:
			renderer.material.color = Color.yellow;
			break;
		}
	}

	void MoveBall(){
	}

	void SetNeutral(){
		SetWhite();
	}

	// Use this for initialization
	void Start () {
		isNeutral = true;
	}
	
	// Update is called once per frame
	void Update () {
		//if v <.1, turn white
	}
}
