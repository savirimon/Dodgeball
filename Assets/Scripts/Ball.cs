using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public Team owner;
	public Vector3 velocity;
	public bool isNeutral;
	public Player p;

	void OnTriggerEnter2D(Collider2D other){
		p = other.GetComponent<Player>();
		if(isNeutral){
			owner = p.team;
			Debug.Log("owner: " + owner);
			SetColor(owner);
			p.b = this;
			//p.b = this.GetComponent<Ball>();
		}else{
			p.health--;
			Debug.Log("decrement health");
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
	}
}
