using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pikachu : MonoBehaviour {

	public float offset;
	public float jumpLimit;
	public Vector3 gravity;
	public Vector3 jump;
	public Vector3 right;
	public float height = 0.5f;

    private int playerNum;
	private pikachuState playerState;
	private pikachuHitState hitState;
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode> ();
//	private Vector3 dir = new Vector3();
	private Vector3 updownAccel = new Vector3();

	public pikachuState PlayerState {
		get {
			return playerState;
		}
		set {
			playerState = value;
			if (playerState == pikachuState.Ground) {
				updownAccel = new Vector3 ();
			}
		}
	}

	/***** Constructors *****/
	/*
	Pikachu (int player) {
		switch (player) {
		case 1:
			playerNum = 1;
			keys.Add ("UP", KeyCode.W);
			keys.Add ("DOWN", KeyCode.S);
			keys.Add ("LEFT", KeyCode.A);
			keys.Add ("RIGHT", KeyCode.D);
			keys.Add ("SMASH", KeyCode.F);
			break;
		case 2:
			playerNum = 2;
			keys.Add ("UP", KeyCode.UpArrow);
			keys.Add ("DOWN", KeyCode.DownArrow);
			keys.Add ("LEFT", KeyCode.LeftArrow);
			keys.Add ("RIGHT", KeyCode.RightArrow);
			keys.Add ("SMASH", KeyCode.Return);
			break;
		}
	}
	*/

	/***** MonoBehaviour *****/
	void Start () {
		playerState = pikachuState.AirDrop;
		hitState = pikachuHitState.Normal;

		playerNum = 1;
		keys.Add ("UP", KeyCode.W);
		keys.Add ("DOWN", KeyCode.S);
		keys.Add ("LEFT", KeyCode.A);
		keys.Add ("RIGHT", KeyCode.D);
		keys.Add ("SMASH", KeyCode.F);
	}

	void Update () {
		if (transform.position.y - height + updownAccel.y * Time.deltaTime < 0f + offset) {
			PlayerState = pikachuState.Ground;
			transform.position = new Vector3 (transform.position.x, height, transform.position.z);
		}
		Move ();
		Debug.Log (playerState);
	}


//	void OnCollisionEnter2D(Collision2D col) {
//		if (col.gameObject.tag == "Ground")
//			playerState = pikachuState.Ground;
//	}

	/***** Methods *****/

	void Move() {
		Vector3 dir = new Vector3 ();
		if (Input.anyKey) { //move when input is given
			if (Input.GetKey (keys["UP"]) && playerState == pikachuState.Ground) {
				PlayerState = pikachuState.Jump;
			}
			if (Input.GetKey (keys["LEFT"])) {
				dir -= right * Time.deltaTime;
			}
			if (Input.GetKey (keys["RIGHT"])) {
				dir += right * Time.deltaTime;
			}
		}
		/*
		else if (playerState == pikachuState.Ground) { //else don't do anything(gravity not affected)
			return;
		}
		if (playerState == pikachuState.Ground) { //else don't do anything(gravity not affected)
			transform.Translate (dir);
			return;
		}

		if (playerState == pikachuState.Jump) {
			if (transform.position.y < jumpLimit)
				updownAccel += jump;
			else {
				playerState = pikachuState.AirDrop;
			}
		}
		updownAccel -= gravity;
		dir += updownAccel;
		transform.Translate (dir);
		*/
//		if (playerState == pikachuState.Jump) {
//			if (transform.position.y > jumpLimit)
//				PlayerState = pikachuState.AirDrop;
//		}

		switch (playerState) {
		case pikachuState.Ground:
			transform.position += dir;
			break;
		case pikachuState.Jump:
			updownAccel += jump * Time.deltaTime;
			dir += updownAccel;
			transform.position += dir;
			PlayerState = pikachuState.AirDrop;
			break;
		case pikachuState.AirDrop:
			updownAccel -= gravity * Time.deltaTime;
			dir += updownAccel;
			transform.position += dir;
			break;
		}
	}
}
