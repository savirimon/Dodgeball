using UnityEngine;
using System.Collections;
using XInputDotNetPure;


public enum Team{ONE, TWO};

public class Player : MonoBehaviour {
	public int playerNum;

	public Team team;

	public int health;
	public float speed;
	public Ball b;
	public bool isThrowing;
	public float scaleFactor;
	public bool againstWall;

	protected PlayerIndex gamepadNum;
	protected GamePadState gamepad;


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
		gamepad = GamePad.GetState(gamepadNum);
		/*
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
		*/
	}

	void FixedUpdate(){
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

	void Throw(){
		Vector3 ballMove = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		Debug.Log(ballMove);

		if(b != null){
			b.velocity = (ballMove * Time.deltaTime * scaleFactor);
		}
	}

	void Move(){
		//Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		//this.transform.Translate (move * Time.deltaTime * speed);

		float leftX, leftY;

		if (playerNum == 0){
			leftX = Input.GetAxis ("Horizontal");
			leftY = Input.GetAxis ("Vertical");
		}

		else{
			leftX = gamepad.ThumbSticks.Left.X;
			leftY = gamepad.ThumbSticks.Left.Y;
		}
		Vector3 move = new Vector3 (leftX, leftY, 0);

		this.rigidbody2D.MovePosition (this.transform.position + move * Time.fixedDeltaTime * speed);

	}

	public void Init () {
		switch (playerNum){
		case 1: gamepadNum = PlayerIndex.One;
			Debug.Log("Player 1 Registered");
			break;
		case 2: gamepadNum = PlayerIndex.Two;
			Debug.Log("Player 2 Registered");
			break;
		case 3: gamepadNum = PlayerIndex.Three;
			Debug.Log("Player 3 Registered");
			break;
		case 4 : gamepadNum = PlayerIndex.Four;
			Debug.Log("Player 4 Registered");
			break;
		default :
			playerNum = 0;
			break;
		}
		
		name = ("Player " + playerNum);
		
	}
	
}
