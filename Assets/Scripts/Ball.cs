using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public Player owner;
	public Vector3 moveDirection;
	public bool isNeutral = true;
	private float speed = 10;
	GameObject arrow;
	TrailRenderer trail;
	public float playerInfluence;

	bool isHeld = false;

	public AudioClip hit;
	public AudioClip wallDeflect;

	Vector3 homePos;
	protected LineCircle ring;
	bool isHome = true;


	/*
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
			moveDirection = new Vector3(moveDirection.x, -moveDirection.y);
		}

		if(other.tag == "Bottom"){

		}

		if(other.tag == "Left"){

		}

		if(other.tag == "Right"){

		}

	}
	*/

	void OnCollisionEnter2D(Collision2D col){
//		Debug.Log (col.collider.gameObject);
		if (LayerMask.NameToLayer("Wall") == col.collider.gameObject.layer) {

			if(col.gameObject.tag == "Top"){
				Deflect(Vector3.down);
			}
			
			if(col.gameObject.tag == "Bottom"){
				Deflect(Vector3.up);

			}
			
			if(col.gameObject.tag == "Left"){
				Deflect(Vector3.right);
				SetNeutral();


			}
			
			if(col.gameObject.tag == "Right"){
				Deflect(Vector3.left);
				SetNeutral();


			}
			Camera.main.audio.PlayOneShot(wallDeflect);

		}
		if (LayerMask.NameToLayer("Player") == col.collider.gameObject.layer) {
			Player other = col.gameObject.GetComponent<Player>();
			Vector3 normal = (this.transform.position - other.transform.position).normalized;

			if (isNeutral){
				Deflect(normal);
				Camera.main.audio.PlayOneShot(wallDeflect);

			}
			else if (other.team == owner.team){
				Deflect(normal);
				Camera.main.audio.PlayOneShot(wallDeflect);

			}
			else if (other.team != owner.team){
				other.DecrementHealth();
					Deflect(normal);
					SetNeutral();
				Camera.main.audio.PlayOneShot(hit);
			}
		}

		if (LayerMask.NameToLayer("Ball") == col.collider.gameObject.layer) {
			Ball other = col.gameObject.GetComponent<Ball>();
			Vector3 normal = (this.transform.position - other.transform.position).normalized;
			
			if (other.isNeutral){
				//Physics2D.IgnoreCollision();
				Deflect(normal);
				Camera.main.audio.PlayOneShot(wallDeflect);
				
			}
			else if (!this.isNeutral){
			if (other.owner.team == owner.team){
				Deflect(normal);
				Camera.main.audio.PlayOneShot(wallDeflect);
				
			}
			else if (other.owner.team != owner.team){
				Deflect(normal);
				Camera.main.audio.PlayOneShot(wallDeflect);
			}
			}
		}

			
	}


	void SetColor(Color color){
		renderer.material.color = color;
		arrow.renderer.material.color = color;
		trail.renderer.material.color = color;
		particleSystem.startColor = color;
	}

	public void SetOwner(Player p){
		isHome = false;
		this.gameObject.layer = LayerMask.NameToLayer ("Ball");
		owner = p;
		switch (p.team) {
		case Team.ONE:
			SetColor(Color.cyan);
			break;
		case Team.TWO:
			SetColor(Color.magenta);
			break;
		}

		isNeutral = false;
		isHeld = true;
		collider2D.enabled = false;
		trail.enabled = false;
		transform.parent = owner.transform;
		transform.localPosition = Vector3.right;
		transform.localRotation = Quaternion.Euler(0,0,-90);
		rigidbody2D.isKinematic = true;
	}

	void MoveBall(){

		if (owner != null) {
						moveDirection += owner.moveVector * playerInfluence;
			if (moveDirection.magnitude < .5f){
				moveDirection = moveDirection.normalized * .5f;
			}
				} else
						moveDirection *= (1-Time.fixedDeltaTime * .5f);
		Vector3 move = moveDirection * Time.fixedDeltaTime * speed;
		

		this.rigidbody2D.MovePosition (this.transform.position + move);
		if (moveDirection.magnitude > .01f) {
						this.transform.LookAt (this.transform.position + moveDirection);	
						this.transform.Rotate (0, 90, 90);
				} 
		if (moveDirection.magnitude < .1f && isNeutral && !isHome) {
			StartCoroutine("ReturnHome");
		}
	}

	public void SetNeutral(){
		isNeutral = true;
		owner = null;
		SetColor(Color.white);
		this.gameObject.layer = LayerMask.NameToLayer ("DeadBall");
	}

	// Use this for initialization
	void Start () {
		ring = transform.FindChild ("Ring").GetComponent<LineCircle>();
		ring.SetThickness (.075f);
		//ring.SetRadius (this.transform.localScale.x);

		homePos = this.transform.position;
		//moveDirection = Vector3.one - Vector3.forward;
		arrow = transform.FindChild ("Arrow").gameObject;
		trail = transform.FindChild ("Trail").gameObject.GetComponent<TrailRenderer> ();
		SetNeutral ();

	}
	void Update(){

	}
	// Update is called once per frame
	void FixedUpdate () {
		if (!isHeld) {
						MoveBall ();
				}
	}

	void ResetPosition(){
		transform.position = Vector3.zero;
		moveDirection = Vector3.zero;
	}

	public void Deflect(Vector3 normal){
		//Vector3 deflectionPoint3 = new Vector3 (deflectionPoint.x, deflectionPoint.y, 0);
		Vector3 reflection = Vector3.Reflect (moveDirection, normal);
		moveDirection = reflection;
//		Debug.Log (reflection);
	}
	/*
	public void Deflect(GameObject other){
		//Vector3 deflectionPoint3 = new Vector3 (deflectionPoint.x, deflectionPoint.y, 0);
		Vector3 reflection = Vector3.Reflect (moveDirection, normal);
		moveDirection = reflection;
		//		Debug.Log (reflection);
	}
	*/

	public void Release(Vector3 direction){
		isHeld = false;
		collider2D.enabled = true;
		trail.enabled = true;
		transform.parent = null;
		transform.localRotation = Quaternion.Euler(0,0,-90);
		rigidbody2D.isKinematic = false;
		moveDirection = direction;
	}

	IEnumerator ReturnHome(){
		isHome = true;

		float effectSpeed = 1;
		collider2D.enabled = false;
		ring.SetRadius (1);


		float t = this.transform.localScale.x;
		while (t > 0) {
			t-=Time.deltaTime * effectSpeed;
			this.transform.localScale = Vector3.one * t;
			trail.startWidth = t;
			yield return null;
		}
		t = 0;
		trail.enabled = false;

		this.transform.position = homePos;
		moveDirection = Vector3.zero;
		transform.rotation = Quaternion.identity;
		while (t < .5f) {
			t += Time.deltaTime * effectSpeed;	
			this.transform.localScale = Vector3.one * t;
			yield return null;
		}
		this.transform.localScale = Vector3.one * .5f;
		trail.startWidth = .5f;
		Debug.Log ("here");
		ring.SetRadius (0);
		collider2D.enabled = true;
		trail.enabled = true;
	}
}