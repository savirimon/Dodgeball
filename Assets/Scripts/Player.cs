using UnityEngine;
using System.Collections;
using XInputDotNetPure;


public enum Team{ONE, TWO};

public class Player : MonoBehaviour {
	public int playerNum;

	public Team team;

	private int health;
	public float speed;
	public Ball heldBall;
	public bool isThrowing;
	public float scaleFactor;
	public bool againstWall;
	public Vector3 moveVector;
	public GameObject[] healthBars;
	public AudioClip throwSound;
	public AudioClip pickupSound;

	protected PlayerIndex gamepadNum;
	protected GamePadState gamepad;

	void PlayThrowSound(){
		AudioSource.PlayClipAtPoint(throwSound, transform.position);
		Debug.Log("throw sound");
	}

	void PlayPickupSound(){
		AudioSource.PlayClipAtPoint(pickupSound, transform.position);
		Debug.Log("pickup sound");
	}

	void DisplayHealth(){
		for(int i = 0; i < health; i++){
			GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Quad);
			if(team == Team.ONE){
				bar.transform.position = new Vector3(10f,1f - i);
			}else if(team == Team.TWO){
				bar.transform.position = new Vector3(-10f,1f - i);
			}
			bar.transform.localScale += new Vector3(-0f, -0.6f);
			bar.renderer.material.color = Color.green;
			healthBars[i] = bar;
		}
	}

	void DecrementHealth(){
		healthBars[health-1].renderer.material.color = Color.black;
		health--;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Ball"){
			Ball ball = other.GetComponent<Ball>();
			if (ball.isNeutral && heldBall == null){
				Pickup(ball);
				PlayPickupSound();
			}else{
				if(!(ball.owner == this)){
					DecrementHealth();
				}
			}
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
		Init ();

		health = 3;
		healthBars = new GameObject[health];
		DisplayHealth();

	}
	
	// Update is called once per frame
	void Update () {
		gamepad = GamePad.GetState(gamepadNum);

		if (playerNum == 0 && Input.GetButtonDown("Fire1")){
			Throw ();
		}
		else if (gamepad.Buttons.A == ButtonState.Pressed){
			Throw ();
		}

	}

	void FixedUpdate(){
		Move ();
	}

	void SetColor(Team t){
		switch (t) {
		case Team.ONE:
			renderer.material.color = Color.cyan;
			//gameObject.layer = LayerMask.NameToLayer("TeamOne");
			break;
		case Team.TWO:
			renderer.material.color = Color.magenta;
			//gameObject.layer = LayerMask.NameToLayer("TeamTwo");

			break;
		}
	}

	void Throw(){
		PlayThrowSound();
		if(heldBall != null){
			heldBall.Release (this.transform.right);
			heldBall = null;
		}
	}

	void Block(Ball ball){

	}

	void Catch(Ball ball){
	
	}

	void Pickup(Ball b){
		Debug.Log("Pickup Ball");
		
		heldBall = b;
		heldBall.SetOwner (this);
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

		moveVector = new Vector3 (leftX, leftY, 0);

		this.rigidbody2D.MovePosition (this.transform.position + moveVector * Time.fixedDeltaTime * speed);

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

		// on the right side of the court
		if (this.transform.position.x > 0) {
			transform.Rotate(0,0,180);
			
				} 
		//on the left side of the court
		else {
				}

		SetColor (team);
		
	}
	
}
