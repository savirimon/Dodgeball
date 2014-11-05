using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public Team owner;
	public Vector3 velocity;
	public bool isNeutral;
	public Player p;
	private float scaleFactor;

	void OnTriggerEnter2D(Collider2D other){
		p = other.GetComponent<Player>();
		if(other.tag == "Player"){
			if(isNeutral){
				owner = p.team;
				Debug.Log("owner: " + owner);
				SetColor(owner);
				p.b = this;
				p.b.transform.parent  = p.transform;
			}else{
				p.health--;
				Debug.Log("decrement health");
			}
		}

		if(other.tag == "Top"){
			velocity = new Vector3(velocity.x, -velocity.y);
		}

		if(other.tag == "Bottom"){

		}

		if(other.tag == "Left"){

		}

		if(other.tag == "Right"){

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
		this.transform.Translate (velocity * Time.deltaTime * scaleFactor);
	}

	void SetNeutral(){
		SetWhite();
	}

	// Use this for initialization
	void Start () {
		isNeutral = true;
		velocity = new Vector3(0,0);
		scaleFactor = 4f;
	}
	
	// Update is called once per frame
	void Update () {
		MoveBall();
	}
}
