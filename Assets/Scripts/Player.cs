using UnityEngine;
using System.Collections;


public enum Team{ONE, TWO};

public class Player : MonoBehaviour {
	public Team team;
	public int health;
	public float speed;

	void OnTriggerEnter2D(){
		health--;
		Debug.Log("health--");
	}

	// Use this for initialization
	void Start () {
		SetColor (team);
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
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

	void Move(){
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		this.transform.Translate (move * Time.deltaTime * speed);
	}
}
