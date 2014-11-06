using UnityEngine;
using System.Collections;
using XInputDotNetPure;


public enum Team{ONE, TWO};

public class Player : MonoBehaviour {
	public int playerNum;

	public Team team;

	private int health = 3;
	public float speed;
	public Ball heldBall;
	public bool isThrowing;
	public float scaleFactor;
	public bool againstWall;
	public Vector3 moveVector;

	float defenseRadius;
	protected PlayerIndex gamepadNum;
	protected GamePadState gamepad;
	protected LineCircle ring;

	bool defenseAvailable = true;

	public GameObject[] healthBars;
	public AudioClip throwSound;
	public AudioClip pickupSound;

	
	// Use this for initialization
	void Start () {
		Init ();

		healthBars = new GameObject[health];
		DisplayHealth();
	}
	
	// Update is called once per frame
	void Update () {
		gamepad = GamePad.GetState(gamepadNum);

		if (playerNum == 0){
			if (Input.GetButtonDown("Fire1")){
				Throw ();
			}
			if (Input.GetButtonDown("Fire2") && defenseAvailable){
				//Debug.Log("Fire2 down");
				StartCoroutine ("ActiveDefense");
			}
			if (Input.GetButtonUp("Fire2") && !defenseAvailable && defenseRadius >0){
				//Debug.Log("Fire2 up");

				Catch ();
				StopCoroutine("ActiveDefense");
				StartCoroutine("DefenseCooldown");
			}
		}
		else{
			if (gamepad.Buttons.A == ButtonState.Pressed){
				Throw ();
			}
			if (gamepad.Buttons.B == ButtonState.Pressed && defenseAvailable){
				StartCoroutine ("ActiveDefense");
			}
			if (gamepad.Buttons.B == ButtonState.Released && !defenseAvailable  && defenseRadius >0){
				Catch ();
				StopCoroutine("ActiveDefense");
				StartCoroutine("DefenseCooldown");
			}
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
		if(heldBall != null){
			heldBall.Release (this.transform.right);
			heldBall = null;
			PlayThrowSound();
		}
	}

	void Block(Ball ball){

	}

	void Catch(){
		Collider2D[] objectsInRing = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), defenseRadius);
		foreach (Collider2D obj in objectsInRing) {
			if (obj.gameObject.tag == "Ball"){
				//StartCoroutine("SlowMotion", .1f);
				Ball ball  = obj.GetComponent<Ball>();
				if (heldBall == null){
					Pickup(ball);
				}
				else{ ball.Deflect(ball.transform.position - this.transform.position);
					ball.SetNeutral();
				}
			}
		}
	}

	void Pickup(Ball b){
//		Debug.Log("Pickup Ball");
		PlayPickupSound ();
		heldBall = b;
		heldBall.SetOwner (this);
	}

	void Move(){
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
		if (this.transform.position.x > 0) {
			transform.Rotate(0,0,180);
		} 
		else {
		}

		SetColor (team);
		ring = transform.FindChild ("Ring").GetComponent<LineCircle>();
		ring.SetRadius (.55f);
		
	}

	void PlayThrowSound(){
		Camera.main.audio.PlayOneShot (throwSound);
		//AudioSource.PlayClipAtPoint(throwSound, transform.position);
		Debug.Log("throw sound");
	}
	
	void PlayPickupSound(){
		Camera.main.audio.PlayOneShot (pickupSound);
		//AudioSource.PlayClipAtPoint(pickupSound, transform.position);
		Debug.Log("pickup sound");
	}
	
	void DisplayHealth(){
		for(int i = 0; i < health; i++){
			GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Quad);
			if(team == Team.ONE){
				bar.transform.position = new Vector3(10f,1f - i);
				bar.renderer.material.color = Color.cyan;
				
			}else if(team == Team.TWO){
				bar.transform.position = new Vector3(-10f,1f - i);
				bar.renderer.material.color = Color.magenta;
				
			}
			bar.transform.localScale += new Vector3(-0f, -0.6f);
			healthBars[i] = bar;
		}
	}
	
	public void DecrementHealth(){
		healthBars [health - 1].renderer.enabled = false;
		health--;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Ball"){
			Ball ball = other.GetComponent<Ball>();
			if (ball.isNeutral && heldBall == null){
				Pickup(ball);
			}
			
		}
	}

	IEnumerator ActiveDefense(){
		float maxRadius = 1.25f;
		//Debug.Log ("Activate Defense");
		defenseAvailable = false;
		defenseRadius = .55f;
		ring.SetRadius(defenseRadius);
		while (defenseRadius < maxRadius) {
			defenseRadius += Time.deltaTime;
			ring.SetRadius(defenseRadius);
			yield return null;
		}
		if (defenseRadius >= maxRadius) {
						defenseRadius = maxRadius;
						ring.SetRadius (defenseRadius);
						yield return new WaitForSeconds (.3f);
		}
		//Debug.Log("after");
		defenseRadius = 0;
		ring.SetRadius (defenseRadius);

		StartCoroutine ("DefenseCooldown");
	}

	IEnumerator DefenseCooldown(){
		//Debug.Log("DefenseCooldown");
		defenseRadius = 0;
		yield return new WaitForSeconds (.1f);
		ring.SetRadius (defenseRadius);
		yield return new WaitForSeconds (1);
		defenseRadius = .55f;
		ring.SetRadius (defenseRadius);
		defenseAvailable = true;
		//Debug.Log ("Defense Available");
	}

	IEnumerator SlowMotion(float scale){
		Time.timeScale = 0;
		while (Time.timeScale < 1) {
			Time.timeScale += Time.unscaledDeltaTime * scale;
			yield return null;
		}

		Time.timeScale = 1;
	}
	
}
