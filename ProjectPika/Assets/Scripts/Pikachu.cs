using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pikachu : MonoBehaviour {

	public float jumpLimit;
	public Vector3 gravity;
	public Vector3 jump;
	public Vector3 right;

    private int playerNum;
	private PikachuState playerState;
	private PikachuHitState hitState;
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode> ();
	private Vector3 dir = new Vector3();

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
		playerState = PikachuState.AirDrop;
		hitState = PikachuHitState.Normal;

		playerNum = 2;
		keys.Add ("UP", KeyCode.UpArrow);
		keys.Add ("DOWN", KeyCode.DownArrow);
		keys.Add ("LEFT", KeyCode.LeftArrow);
		keys.Add ("RIGHT", KeyCode.RightArrow);
		keys.Add ("SMASH", KeyCode.Return);
	}

	void Update () {
		Move ();
		Debug.Log (playerState);
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Ground")
			playerState = PikachuState.Ground;
	}

	/***** Methods *****/

	void Move() {
		if (Input.anyKey) { //move when input is given
			if (Input.GetKeyDown (keys["UP"]) && playerState == PikachuState.Ground) {
				playerState = PikachuState.Jump;
			}
			if (Input.GetKey (keys["LEFT"])) {
				dir -= right;
			}
			if (Input.GetKey (keys["RIGHT"])) {
				dir += right;
			}
		}
		if (playerState == PikachuState.Ground) { //else don't do anything(gravity not affected)
			transform.Translate (dir);
			return;
		}

		if (playerState == PikachuState.Jump) {
			if (transform.position.y < jumpLimit)
				dir += jump;
			else {
				playerState = PikachuState.AirDrop;
				dir = new Vector3 ();
			}
		}
		dir -= gravity;
		transform.Translate (dir);
	}
}
