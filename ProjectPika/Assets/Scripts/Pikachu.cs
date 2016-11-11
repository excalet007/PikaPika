using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Pikachu : MonoBehaviour {

    private int playerNum;
	private pikachuState playerState;
	private pikachuHitState hitState;
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode> ();

	private Vector3 pikaVelocity = new Vector3 ();
	private Vector3 motionEnergy = new Vector3(); //the current motion energy(jumps, gravity)
    private Vector3 slideEnergy = new Vector3(); //the current slide energy(jumps * n, gravity * n, right * n)
    public Vector3 gravity = new Vector3(0f, 1f, 0f);
    public Vector3 jump = new Vector3(0f, 0.35f, 0f);
    public Vector3 right = new Vector3(6f, 0f, 0f);

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

    public pikachuHitState HitState
    {
        get
        {
            return hitState;
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

	void FixedUpdate () {
		CheckGround();
        if (GameObject.Find("PlayManager").GetComponent<PlayManager>().keyLock == false)
        {
            move();
        }
        ErrorCheck();
	}
    
	/***** Methods *****/
	delegate void Move();
	delegate void HitBall();
    
    private void ErrorCheck()
    {

        if ((Mathf.Abs(this.transform.position.x)+PlayManager.pikaBelly) > Mathf.Abs(PlayManager.mapInfo[0]/2))
        {
            switch (playerNum) { 
                case 1:
                    this.transform.position = new Vector3(-(PlayManager.mapInfo[0] / 2)+PlayManager.pikaBelly, this.transform.position.y, transform.position.z);
                    pikaVelocity = new Vector3(0, pikaVelocity.y, pikaVelocity.z);
                    break;

                case 2:
                    this.transform.position = new Vector3((PlayManager.mapInfo[0] / 2) - PlayManager.pikaBelly, this.transform.position.y, transform.position.z);
                    pikaVelocity = new Vector3(0, pikaVelocity.y, pikaVelocity.z);
                    break;
            }
        }

        if((Mathf.Abs(this.transform.position.x) - PlayManager.pikaBelly) < Mathf.Abs(PlayManager.mapInfo[2]/2))
        {
            switch (playerNum)
            {
                case 1:
                    this.transform.position = new Vector3(-(PlayManager.mapInfo[2] / 2) - PlayManager.pikaBelly, this.transform.position.y, transform.position.z);
                    pikaVelocity = new Vector3(0, pikaVelocity.y, pikaVelocity.z);
                    break;
                case 2:
                    this.transform.position = new Vector3((PlayManager.mapInfo[2] / 2) + PlayManager.pikaBelly, this.transform.position.y, transform.position.z);
                    pikaVelocity = new Vector3(0, pikaVelocity.y, pikaVelocity.z);
                    break;
            }
        }

    }

    //checks if player position is touching ground and resets smachCounter
    public float offset = 0.01f;

    void CheckGround() {
        
        if (this.transform.position.y <= PlayManager.pikaHeight)
        {
            //print("땅이에욧");
			PlayerState = pikachuState.Ground;
			if(hitState == pikachuHitState.Normal)
				smashCounter = false; //cooldown over
			this.transform.position = new Vector3 (transform.position.x, PlayManager.pikaHeight, transform.position.z);
            pikaVelocity = new Vector3(0, 0, 0);
		}
	}

    //move controls for player1

    public Quaternion pikaRotation;

    private void Move_1p() {
        // FSM 에 따르면 선상태 구분후 입력판단이 좋다고 합니다. 
        //1. 이 게임의 경우 상태는 한정되어있지만 입력방식은 다양하게 할 수도 있으므로.
        pikaVelocity = new Vector3 (); //amount of next frame position change

        if (playerState == pikachuState.Ground)
        {
            if (Input.GetKey(keys["UP"]))
                PlayerState = pikachuState.Jump;

            if (Input.GetKey(keys["LEFT"]) == true && Input.GetKey(keys["SMASH"]) == true)
            {
                print("좌슬라이드 시작!");
                motionEnergy += 1.5f * jump;
                pikaVelocity += motionEnergy;
                //transform.position += pikaVelocity*0.5f;
                playerState = pikachuState.Receive_Left;
            }

            if (Input.GetKey(keys["RIGHT"]) == true && Input.GetKey(keys["SMASH"]) == true)
            {
                print("우슬라이드 시작!");
                motionEnergy += 1.5f*jump;
                pikaVelocity += motionEnergy;
                //transform.position += pikaVelocity*0.5f;
                playerState = pikachuState.Receive_Right;
            }
        }

        if (playerState == pikachuState.Ground || playerState == pikachuState.Jump || playerState == pikachuState.AirDrop)
        {
            if (Input.GetKey(keys["LEFT"]))
            {
                pikaVelocity -= right * Time.fixedDeltaTime;
            }
            if (Input.GetKey(keys["RIGHT"]))
            {
                pikaVelocity += right * Time.fixedDeltaTime;
            }
        }

        switch (playerState) {
		case pikachuState.Ground:
                pikaRotation.eulerAngles = new Vector3(0, 0, 0);
                this.transform.rotation = pikaRotation;
                transform.position += pikaVelocity;
			break;
		case pikachuState.Jump:
			motionEnergy += jump;
			pikaVelocity += motionEnergy;
			transform.position += pikaVelocity;
			PlayerState = pikachuState.AirDrop;
			break;
		case pikachuState.AirDrop:
			motionEnergy -= gravity * Time.fixedDeltaTime;
			pikaVelocity += motionEnergy;
			transform.position += pikaVelocity;
			break;
        case pikachuState.Receive_Left:
                pikaRotation.eulerAngles = new Vector3(0, 180, 0);
                this.transform.rotation = pikaRotation;
                pikaVelocity += (-2f* gravity*Time.fixedDeltaTime + -1.5f*right*Time.fixedDeltaTime);
                transform.position += pikaVelocity;
                //print("좌측 리시브 작동");
            break;
        case pikachuState.Receive_Right:
                //print("우측 리시브 작동");
                pikaRotation.eulerAngles = new Vector3(0, 0, 0);
                this.transform.rotation = pikaRotation;
                pikaVelocity += (-2f*gravity * Time.fixedDeltaTime + 1.5f*right * Time.fixedDeltaTime);
                transform.position += pikaVelocity;
                break;
        case pikachuState.GameOver:
            print ("게임오버!");
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
				pikaVelocity -= right * Time.fixedDeltaTime;
				//if player goes beyond boundary, return fixed vector and reset dir
				if (transform.position.x - PlayManager.pikaBelly + pikaVelocity.x < PlayManager.mapInfo [2] / 2) {
					//transform.position = new Vector3 (PlayManager.mapInfo [2] / 2 + PlayManager.pikaBelly, transform.position.y, transform.position.z);
					pikaVelocity = new Vector3();
				}
			}
			if (Input.GetKey (keys ["RIGHT"])) {
				pikaVelocity += right * Time.fixedDeltaTime;
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
			PlayerState = pikachuState.AirDrop;
			break;
		case pikachuState.AirDrop:
			motionEnergy -= gravity * Time.fixedDeltaTime;
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
            Invoke("ReturnNormalHitState", 0.3f);
        }
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
		Invoke ("ReturnNormalHitState", 0.3f);
	}
}
