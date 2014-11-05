using UnityEngine;
using System.Collections;


public enum Team{ONE, TWO};

public class Player : MonoBehaviour {
	public Team team;
	public int health;
	public float speed;
	public Ball b;
	public bool isThrowing;
	public float scaleFactor;
	public bool againstWall;

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Wall"){
			//reflect x and y
			Debug.Log("wall");
			againstWall = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Wall"){
			againstWall = false;
			Debug.Log("no longer against wall");
		}
	}

	// Use this for initialization
	void Start () {
		SetColor (team);
		isThrowing = false;
		scaleFactor = 12.75f;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isThrowing){
			Move();
			if(Input.GetKeyDown("space")){
				isThrowing = true;
				Debug.Log("throwing");
			}
		}else{
			Throw();
			if(Input.GetKeyUp("space")){
				isThrowing = false;
				b.transform.parent  = null;
				b = null;
				Debug.Log("not throwing");
			}
		}
	}

	void SetColor(Team t){
		switch (t) {
		case Team.ONE:
			renderer.material.color = Color.cyan;
			break;
		case Team.TWO:
			renderer.material.color = Color.yellow;
			break;
		}
	}

	void Throw(){
		Vector3 ballMove = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		Debug.Log(ballMove);

		if(b != null){
			b.velocity = (ballMove * Time.deltaTime * scaleFactor);
		}
	}

	void Move(){
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		this.transform.Translate (move * Time.deltaTime * speed);
	}
}
