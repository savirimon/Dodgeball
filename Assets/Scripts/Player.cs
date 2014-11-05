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

	// Use this for initialization
	void Start () {
		SetColor (team);
		isThrowing = false;
		scaleFactor = 2.75f;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isThrowing){
			Move ();
			if(Input.GetKeyDown("space")){
				isThrowing = true;
				Debug.Log("throwing");
			}
		}else{
			Throw();
			if(Input.GetKeyUp("space")){
				isThrowing = false;
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
			b.transform.Translate(ballMove * Time.deltaTime * scaleFactor);
		}
	}

	void Move(){
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		this.transform.Translate (move * Time.deltaTime * speed);
	}
}
