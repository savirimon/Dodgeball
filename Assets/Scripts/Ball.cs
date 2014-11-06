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
			particleSystem.Emit(5);
			if(col.gameObject.tag == "Top"){
				Deflect(Vector3.down);
			}
			
			if(col.gameObject.tag == "Bottom"){
				Deflect(Vector3.up);

			}
			
			if(col.gameObject.tag == "Left"){
				Deflect(Vector3.right);

			}
			
			if(col.gameObject.tag == "Right"){
				Deflect(Vector3.left);

			}
			Camera.main.audio.PlayOneShot(wallDeflect);
			owner = null;
			SetNeutral();
		}
		if (LayerMask.NameToLayer("Player") == col.collider.gameObject.layer) {
			Player other = col.gameObject.GetComponent<Player>();
			Vector3 normal = (this.transform.position - new Vector3(col.contacts[0].point.x, col.contacts[0].point.y)).normalized;

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
			
	}


	void SetColor(Color color){
		renderer.material.color = color;
		arrow.renderer.material.color = color;
		trail.renderer.material.color = color;
	}

	public void SetOwner(Player p){
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
		}
		Vector3 move = moveDirection * Time.fixedDeltaTime * speed;

		this.rigidbody2D.MovePosition (this.transform.position + move);
		if (moveDirection.magnitude > .1f) {
						this.transform.LookAt (this.transform.position + moveDirection);	
						this.transform.Rotate (0, 90, 90);
				}
	}

	public void SetNeutral(){
		isNeutral = true;
		owner = null;
		SetColor(Color.white);
	}

	// Use this for initialization
	void Start () {
		isNeutral = true;
		//moveDirection = Vector3.one - Vector3.forward;
		arrow = transform.FindChild ("Arrow").gameObject;
		trail = transform.FindChild ("Trail").gameObject.GetComponent<TrailRenderer> ();
	}
	void Update(){

	}
	// Update is called once per frame
	void FixedUpdate () {
		if (!isHeld) {
						MoveBall ();
				}
	}

	public void Deflect(Vector3 normal){
		//Vector3 deflectionPoint3 = new Vector3 (deflectionPoint.x, deflectionPoint.y, 0);
		Vector3 reflection = Vector3.Reflect (moveDirection, normal);
		moveDirection = reflection;
//		Debug.Log (reflection);
	}

	public void Release(Vector3 direction){
		isHeld = false;
		collider2D.enabled = true;
		trail.enabled = true;
		transform.parent = null;
		transform.localRotation = Quaternion.Euler(0,0,-90);
		rigidbody2D.isKinematic = false;
		moveDirection = direction;
	}
}