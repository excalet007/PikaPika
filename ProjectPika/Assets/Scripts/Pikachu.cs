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
	private Move move; //delegate function to choose player1/player2
	private bool smashCounter = false; //used to limit smash time, true:smash cooldown

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
			keys.Add ("SMASH", KeyCode.Space);
			move = new Move (Move_1p);
			move += Smash_1p;
			break;
		case 2:
			keys.Add ("UP", KeyCode.UpArrow);
			keys.Add ("DOWN", KeyCode.DownArrow);
			keys.Add ("LEFT", KeyCode.LeftArrow);
			keys.Add ("RIGHT", KeyCode.RightArrow);
			keys.Add ("SMASH", KeyCode.Return);
			move = new Move (Move_2p);
			move += Smash_2p;
			break;
		}
	}

	void Update () {
		CheckGround();
		move();
	}

	/***** Methods *****/
	delegate void Move();
	delegate void HitBall();

	void CheckGround() {
		if (transform.position.y - PlayManager.pikaBot + updownAccel.y * Time.deltaTime < 0f + offset) {
			PlayerState = pikachuState.Ground;
			if(hitState == pikachuHitState.Normal)
				smashCounter = false; //cooldown over
			transform.position = new Vector3 (transform.position.x, PlayManager.pikaBot, transform.position.z);
		}
	}

	private void Move_1p() {
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
			PlayerState = pikachuState.AirDrop;
			break;
		case pikachuState.AirDrop:
			updownAccel -= gravity * Time.deltaTime;
			dir += updownAccel;
			transform.position += dir;
			break;
		}
	}

	private void Move_2p() {
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

	private void ReturnNormalHitState() {
		hitState = pikachuHitState.Normal;
	}

	private void Smash_1p() {
		if (Input.GetKeyDown (keys ["SMASH"]) && !smashCounter && PlayerState == pikachuState.AirDrop) { //airborn smash
			if (Input.GetKey (keys ["DOWN"])) {
				if (Input.GetKey (keys ["RIGHT"]) || Input.GetKey(keys["LEFT"]))
					hitState = pikachuHitState.HitSmash_DownRight;
				else
					hitState = pikachuHitState.HitSmash_Down;
			}
			else if (Input.GetKey (keys ["UP"])) {
				if (Input.GetKey (keys ["RIGHT"]) || Input.GetKey(keys["LEFT"]))
					hitState = pikachuHitState.HitSmash_UpRight;
				else
					hitState = pikachuHitState.HitSmash_Up;
			}
			else if (Input.GetKey (keys["RIGHT"]) || Input.GetKey(keys["LEFT"])) {
				hitState = pikachuHitState.HitSmash_Right;
			} else
				hitState = pikachuHitState.HitSlow;
			smashCounter = true;//cooldown until lands ground
			Debug.Log("1p hitstate:"+hitState+"counter"+smashCounter);
		}
		else if (Input.GetKeyDown (keys ["SMASH"]) && !smashCounter && PlayerState == pikachuState.Ground) {
			if (Input.GetKey (keys ["LEFT"]))
				PlayerState = pikachuState.Receive_Left;
			if (Input.GetKey (keys ["RIGHT"]))
				PlayerState = pikachuState.Receive_Right;
			Debug.Log("1p state:"+playerState+"counter"+smashCounter);
		}
		Invoke ("ReturnNormalHitState", 0.5f);
	}

	private void Smash_2p() {
		if (Input.GetKeyDown (keys ["SMASH"]) && !smashCounter && PlayerState == pikachuState.AirDrop) { //airborn smash
			if (Input.GetKey (keys ["DOWN"])) {
				if (Input.GetKey (keys ["RIGHT"]) || Input.GetKey(keys["LEFT"]))
					hitState = pikachuHitState.HitSmash_DownLeft;
				else
					hitState = pikachuHitState.HitSmash_Down;
			}
			else if (Input.GetKey (keys ["UP"])) {
				if (Input.GetKey (keys ["RIGHT"]) || Input.GetKey(keys["LEFT"]))
					hitState = pikachuHitState.HitSmash_UpLeft;
				else
					hitState = pikachuHitState.HitSmash_Up;
			}
			else if (Input.GetKey (keys["RIGHT"]) || Input.GetKey(keys["LEFT"])) {
				hitState = pikachuHitState.HitSmash_Left;
			} else
				hitState = pikachuHitState.HitSlow;
			smashCounter = true; //cooldown until lands ground
			Debug.Log("2p hitstate:"+hitState+"counter"+smashCounter);
		}
		else if (Input.GetKeyDown (keys ["SMASH"]) && !smashCounter && PlayerState == pikachuState.Ground) {
			if (Input.GetKey (keys ["LEFT"]))
				PlayerState = pikachuState.Receive_Left;
			if (Input.GetKey (keys ["RIGHT"]))
				PlayerState = pikachuState.Receive_Right;
		}
		Invoke ("ReturnNormalHitState", 0.5f);
	}
}
