using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pikachu : MonoBehaviour {

	public float offset = 0.01f;
	public Vector3 gravity = new Vector3(0f, 1f, 0f);
	public Vector3 jump = new Vector3(0f, 0.35f, 0f);
	public Vector3 right = new Vector3(4f, 0f, 0f);

	private int playerNum;
	private pikachuState playerState;
	private pikachuHitState hitState;
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode> ();
	private Vector3 updownAccel = new Vector3();
	private Move move;

	/***** Getters and Setters *****/
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

	public int PlayerNum {
		get {
			return playerNum;
		}
		set {
			playerNum = value;
		}
	}

	/***** MonoBehaviour *****/
	void Start () {
		PlayerState = pikachuState.AirDrop; //first drop at game start
		hitState = pikachuHitState.Normal;

		switch (playerNum) {
		case 1:
			keys.Add ("UP", KeyCode.W);
			keys.Add ("DOWN", KeyCode.S);
			keys.Add ("LEFT", KeyCode.A);
			keys.Add ("RIGHT", KeyCode.D);
			keys.Add ("SMASH", KeyCode.F);
			move = new Move (Move_1p);
			break;
		case 2:
			keys.Add ("UP", KeyCode.UpArrow);
			keys.Add ("DOWN", KeyCode.DownArrow);
			keys.Add ("LEFT", KeyCode.LeftArrow);
			keys.Add ("RIGHT", KeyCode.RightArrow);
			keys.Add ("SMASH", KeyCode.Return);
			move = new Move (Move_2p);
			break;
		}
	}

	void Update () {
		CheckGround();
		move();
		Debug.Log (playerState);
	}

	/***** Methods *****/
	delegate void Move();
	delegate void HitBall();

	void CheckGround() {
		if (transform.position.y - PlayManager.pikaBot + updownAccel.y * Time.deltaTime < 0f + offset) {
			PlayerState = pikachuState.Ground;
			transform.position = new Vector3 (transform.position.x, PlayManager.pikaBot, transform.position.z);
		}
	}

	void Move_1p() {
		Vector3 dir = new Vector3 ();
		if (Input.anyKey) { //move when input is given
			if (Input.GetKey (keys ["UP"]) && playerState == pikachuState.Ground) {
				PlayerState = pikachuState.Jump;
			}
			if (Input.GetKey (keys ["LEFT"])) {
				dir -= right * Time.deltaTime;
				if (transform.position.x - PlayManager.pikaBelly + dir.x < -PlayManager.mapInfo [0]/ 2) {
					transform.position = new Vector3 (-PlayManager.mapInfo [0] / 2 + PlayManager.pikaBelly, transform.position.y, transform.position.z);
					dir = new Vector3();
				}
			}
			if (Input.GetKey (keys ["RIGHT"])) {
				dir += right * Time.deltaTime;
				if (transform.position.x + PlayManager.pikaBelly + dir.x > -PlayManager.mapInfo [2] / 2) {
					transform.position = new Vector3 (-PlayManager.mapInfo [2] / 2 - PlayManager.pikaBelly, transform.position.y, transform.position.z);
					dir = new Vector3();
				}
			}
		}
		switch (playerState) {
		case pikachuState.Ground:
			transform.position += dir;
			break;
		case pikachuState.Jump:
			updownAccel += jump;
			dir += updownAccel;
			transform.position += dir;
			Debug.Log ("accel:" + updownAccel);
			PlayerState = pikachuState.AirDrop;
			break;
		case pikachuState.AirDrop:
			updownAccel -= gravity * Time.deltaTime;
			dir += updownAccel;
			transform.position += dir;
			break;
		}
	}

	void Move_2p() {
		Vector3 dir = new Vector3 ();
		if (Input.anyKey) { //move when input is given
			if (Input.GetKey (keys ["UP"]) && playerState == pikachuState.Ground) {
				PlayerState = pikachuState.Jump;
			}
			if (Input.GetKey (keys ["LEFT"])) {
				dir -= right * Time.deltaTime;
				if (transform.position.x - PlayManager.pikaBelly + dir.x < PlayManager.mapInfo [2] / 2) {
					transform.position = new Vector3 (PlayManager.mapInfo [2] / 2 + PlayManager.pikaBelly, transform.position.y, transform.position.z);
					dir = new Vector3();
				}
			}
			if (Input.GetKey (keys ["RIGHT"])) {
				dir += right * Time.deltaTime;
				if (transform.position.x + PlayManager.pikaBelly + dir.x > PlayManager.mapInfo [0] / 2) {
					transform.position = new Vector3 (PlayManager.mapInfo [0] / 2 - PlayManager.pikaBelly, transform.position.y, transform.position.z);
					dir = new Vector3();
				}
			}
		}
		switch (playerState) {
		case pikachuState.Ground:
			transform.position += dir;
			break;
		case pikachuState.Jump:
			updownAccel += jump;
			dir += updownAccel;
			transform.position += dir;
			Debug.Log ("accel:" + updownAccel);
			PlayerState = pikachuState.AirDrop;
			break;
		case pikachuState.AirDrop:
			updownAccel -= gravity * Time.deltaTime;
			dir += updownAccel;
			transform.position += dir;
			break;
		}
	}

	void Smash_1p() {
		
	}

	void Smash_2p() {
		
	}
}
