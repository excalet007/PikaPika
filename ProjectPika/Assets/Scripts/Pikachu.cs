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
	private Vector3 pikaVelocity = new Vector3 ();
	private Vector3 motionEnergy = new Vector3(); //the current motion energy(jumps, gravity)
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
				motionEnergy = new Vector3 ();
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

	public Vector3 PikaVelocity {
		get {
			return pikaVelocity;
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

	//checks if player position is touching ground and resets smachCounter
	void CheckGround() {
		if (transform.position.y - PlayManager.pikaBot + motionEnergy.y * Time.deltaTime < 0f + offset) {
			PlayerState = pikachuState.Ground;
			if(hitState == pikachuHitState.Normal)
				smashCounter = false; //cooldown over
			transform.position = new Vector3 (transform.position.x, PlayManager.pikaBot, transform.position.z);
		}
	}

	//move controls for player1
	private void Move_1p() {
		pikaVelocity = new Vector3 (); //amount of next frame position change
		if (Input.anyKey) { //move when input is given
			if (Input.GetKey (keys ["UP"]) && playerState == pikachuState.Ground) {
				PlayerState = pikachuState.Jump;
			}
			if (Input.GetKey (keys ["LEFT"])) {
				pikaVelocity -= right * Time.deltaTime;
				//if player goes beyond boundary, return fixed vector and reset dir
				if (transform.position.x - PlayManager.pikaBelly + pikaVelocity.x < -PlayManager.mapInfo [0]/ 2) { 
					//transform.position = new Vector3 (-PlayManager.mapInfo [0] / 2 + PlayManager.pikaBelly, transform.position.y, transform.position.z);
					pikaVelocity = new Vector3();
				}
			}
			if (Input.GetKey (keys ["RIGHT"])) {
				pikaVelocity += right * Time.deltaTime;
				//if player goes beyond boundary, return fixed vector and reset dir
				if (transform.position.x + PlayManager.pikaBelly + pikaVelocity.x > -PlayManager.mapInfo [2] / 2) {
					//transform.position = new Vector3 (-PlayManager.mapInfo [2] / 2 - PlayManager.pikaBelly, transform.position.y, transform.position.z);
					pikaVelocity = new Vector3();
				}
			}
		}
		switch (playerState) {
		case pikachuState.Ground:
			transform.position += pikaVelocity;
			break;
		case pikachuState.Jump:
			motionEnergy += jump;
			pikaVelocity += motionEnergy;
			transform.position += pikaVelocity;
			PlayerState = pikachuState.AirDrop;
			break;
		case pikachuState.AirDrop:
			motionEnergy -= gravity * Time.deltaTime;
			pikaVelocity += motionEnergy;
			transform.position += pikaVelocity;
			break;
		}
	}

	//move controls for player2
	private void Move_2p() {
		pikaVelocity = new Vector3 (); //amount of next frame position change
		if (Input.anyKey) { //move when input is given
			if (Input.GetKey (keys ["UP"]) && playerState == pikachuState.Ground) {
				PlayerState = pikachuState.Jump;
			}
			if (Input.GetKey (keys ["LEFT"])) {
				pikaVelocity -= right * Time.deltaTime;
				//if player goes beyond boundary, return fixed vector and reset dir
				if (transform.position.x - PlayManager.pikaBelly + pikaVelocity.x < PlayManager.mapInfo [2] / 2) {
					//transform.position = new Vector3 (PlayManager.mapInfo [2] / 2 + PlayManager.pikaBelly, transform.position.y, transform.position.z);
					pikaVelocity = new Vector3();
				}
			}
			if (Input.GetKey (keys ["RIGHT"])) {
				pikaVelocity += right * Time.deltaTime;
				//if player goes beyond boundary, return fixed vector and reset dir
				if (transform.position.x + PlayManager.pikaBelly + pikaVelocity.x > PlayManager.mapInfo [0] / 2) {
					//transform.position = new Vector3 (PlayManager.mapInfo [0] / 2 - PlayManager.pikaBelly, transform.position.y, transform.position.z);
					pikaVelocity = new Vector3();
				}
			}
		}
		switch (playerState) {
		case pikachuState.Ground:
			transform.position += pikaVelocity;
			break;
		case pikachuState.Jump:
			motionEnergy += jump;
			pikaVelocity += motionEnergy;
			transform.position += pikaVelocity;
			Debug.Log ("accel:" + motionEnergy);
			PlayerState = pikachuState.AirDrop;
			break;
		case pikachuState.AirDrop:
			motionEnergy -= gravity * Time.deltaTime;
			pikaVelocity += motionEnergy;
			transform.position += pikaVelocity;
			break;
		}
	}

	//turn hitstate to normal(need for Invoke)
	private void ReturnNormalHitState() {
		hitState = pikachuHitState.Normal;
	}

	//smash controls for player 1
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

	//smash controls for player 2
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
