﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour {
	private static PlayManager instance = null; //for singleton design

    public GameManager gameManager;
    public Pikachu pikachu;
    public GameObject ball;

	public GameObject player1;
	public GameObject player2;
    
	private static playState playState;
	private int score1;
	private int score2;
    public Image Score1Image;
    public Image Score2Image;
    public Image GameSetImage;
    public Image ReadyImage;

    public float readyCounter; // Ready? 메세지 표시를 조절하기 위한 변수

    public static Sprite[] scoreImageList = new Sprite[16]; // 점수 스프라이트를 불러오기 위한 배열
    
    /// <summary>
    ///  //0: map width, 1: map height, 2: net width, 3: net height, 4:topnet width 5: topnet height
    /// </summary>
    public static float[] mapInfo = new float[6] { 20f, 8f, 0.2f, 2f, 0.4f, 0.2f };
    public EdgeCollider2D[] WallNNet = null; // 0: Top, 1: Bottom, 2:Left, 3:Right, 4:Net

    public static float ballRadius = 0.5f;
	public static float pikaBelly = 0.5f;
	public static float pikaBot = 0.5f;


	/***** Getters and Setters *****/
    public static PlayManager Instance
    {
        get
        {
            return instance;
        }
    }

	public int Score1 {
		get {
			return score1;
		}
		set {
			score1 = value;
            UpdateScore();
            StartCoroutine("ResetPlayScene1");
            Ball.ballVelocity= new Vector3(0, 0.1f, 0);
        }
	}

	public int Score2 {
		get {
			return score2;
		}
		set {
			score2 = value;
            UpdateScore();
            StartCoroutine("ResetPlayScene2");
            Ball.ballVelocity = new Vector3(0, 0.1f, 0);
        }
	}

	/***** MonoBehaviour *****/
	void Awake() { //for singleton design
		if (instance == null)
			instance = this;
        else
        	Destroy (this);

        //의미가 없는데? 딱히 쓸 이유가 없음.
        //DontDestroyOnLoad(Score1Image);
        //DontDestroyOnLoad(Score2Image);
    }

	void Start() {
        
		player1.AddComponent<Pikachu> ().PlayerNum = 1; //adding scripts to both players
		player2.AddComponent<Pikachu> ().PlayerNum = 2;

        GenerateScene(); // 맵 콜라이더 생성 _ 병희(1012)

		score1 = 0;
		score2 = 0;

        Score1Image = GameObject.Find("Score1").GetComponent<Image>();
        Score2Image = GameObject.Find("Score2").GetComponent<Image>();
        GameSetImage = GameObject.Find("GameSet").GetComponent<Image>();
        ReadyImage = GameObject.Find("Ready?").GetComponent<Image>();

        ball.transform.position = new Vector3(-0.4f * mapInfo[0], 0.8f * mapInfo[1], 0);

        ResetPlayer();
        ResetBall(1);            

        for (int i = 0; i < 16; i++)
        {
            scoreImageList[i] = Resources.Load<Sprite>("Score/ScoreNumbers_" + i);
        }
        Score1Image.sprite = scoreImageList[score1];
        Score2Image.sprite = scoreImageList[score2];

        GameSetImage.sprite = Resources.Load<Sprite>("게임화면_GameSet");
        GameSetImage.transform.gameObject.SetActive(false);

        ReadyImage.sprite = Resources.Load<Sprite>("게임화면_Ready");
        ReadyImage.transform.gameObject.SetActive(true);
    }

    void Update()
    {
<<<<<<< HEAD
        CallReady();
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine("ResetPlayScene1");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine("ResetPlayScene2");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            score1++;
            UpdateScore();
            print("1p scoreup");
        }
        
        if(Input.GetKeyDown(KeyCode.J))
        {
            score2++;
            UpdateScore();
            print("2p scoreup");
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            readyCounter = 0f;
        }

=======
        DebugCheck();
>>>>>>> origin/midterm
        GameSet();
    }

    /***** Method and Variables *****/
    void UpdateScore()
    {
        Score1Image.sprite = scoreImageList[score1];
        Score2Image.sprite = scoreImageList[score2];
    }

    public void GameSet()
    {
        if (score1 == 15 || score2 == 15)
        {
            print("GameSet");
            GameSetImage.transform.gameObject.SetActive(true); // Print Game Set Message

            if (pikachu.PlayerState == pikachuState.Ground)
            {
                player1.GetComponent<Pikachu>().enabled = false; // 
                player2.GetComponent<Pikachu>().enabled = false; //1P와 2P 의 피카츄 움직임 스크립트 Disable
            }
            
            StartCoroutine("Wait");
        }
    }

    /// <summary>
    /// Debug용 함수 씬 리로드, 점수 증가
    /// </summary>
    private void DebugCheck()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine("ResetPlayScene1");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine("ResetPlayScene2");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            score1++;
            UpdateScore();
            print("1p scoreup");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            score2++;
            UpdateScore();
            print("2p scoreup");
        }
    }

    // 맵에 2D콜라이더 생성
    void GenerateScene() 
    {
        //Generate Collider without Pikachu
        Vector2[] pointsVar = new Vector2[12];
        pointsVar[0] = new Vector2(-mapInfo[0] / 2f, mapInfo[1]);
        pointsVar[1] = new Vector2(-mapInfo[0] / 2, 0);
        pointsVar[2] = new Vector2(-mapInfo[2] / 2, 0);
        pointsVar[3] = new Vector2(-mapInfo[2] / 2, mapInfo[3]);
        pointsVar[4] = new Vector2(-mapInfo[4] / 2, mapInfo[3]);
        pointsVar[5] = new Vector2(-mapInfo[4] / 2, mapInfo[3] + mapInfo[5]);
        pointsVar[6] = new Vector2(mapInfo[4] / 2, mapInfo[3] + mapInfo[5]);
        pointsVar[1] = new Vector2(-mapInfo[0] / 2, 0);
        pointsVar[7] = new Vector2(mapInfo[4] / 2, mapInfo[3]);
        pointsVar[8] = new Vector2(mapInfo[2] / 2, mapInfo[3]);
        pointsVar[9] = new Vector2(mapInfo[2] / 2, 0);
        pointsVar[10] = new Vector2(mapInfo[0] / 2, 0);
        pointsVar[11] = new Vector2(mapInfo[0] / 2, mapInfo[1]);

        Vector2[] top = new Vector2[2];
        top[0] = new Vector2(-mapInfo[0] / 2f, mapInfo[1]);
        top[1] = new Vector2(mapInfo[0] / 2f, mapInfo[1]);
        WallNNet[0].points = top;

        Vector2[] bottom = new Vector2[2];
        bottom[0] = new Vector2(-mapInfo[0] / 2f, 0);
        bottom[1] = new Vector2(mapInfo[0] / 2f, 0);
        WallNNet[1].points = bottom;

        Vector2[] left = new Vector2[2];
        left[0] = new Vector2(-mapInfo[0] / 2f, mapInfo[1]);
        left[1] = new Vector2(-mapInfo[0] / 2f, 0);
        WallNNet[2].points = left;

        Vector2[] right = new Vector2[2];
        right[0] = new Vector2(mapInfo[0] / 2f, mapInfo[1]);
        right[1] = new Vector2(mapInfo[0] / 2f, 0);
        WallNNet[3].points = right;

        Vector2[] net = new Vector2[8];
        net[0] = new Vector2(-mapInfo[2] / 2, 0);
        net[1] = new Vector2(-mapInfo[2] / 2, mapInfo[3]);
        net[2] = new Vector2(-mapInfo[4] / 2, mapInfo[3]);
        net[3] = new Vector2(-mapInfo[4] / 2, mapInfo[3] + mapInfo[5]);
        net[4] = new Vector2(mapInfo[4] / 2, mapInfo[3] + mapInfo[5]);
        net[5] = new Vector2(mapInfo[4] / 2, mapInfo[3]);
        net[6] = new Vector2(mapInfo[2] / 2, mapInfo[3]);
        net[7] = new Vector2(mapInfo[2] / 2, 0);
        WallNNet[4].points = net;

    }

    IEnumerator ResetPlayScene1()
    {
        float fadetime = GameObject.Find("FadeControl").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        ResetPlayer();
        ResetBall(1);
        float fadetime2 = GameObject.Find("FadeControl").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadetime2);
        readyCounter = 0f;
        
    }

    IEnumerator ResetPlayScene2()
    {
        float fadetime = GameObject.Find("FadeControl").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        ResetPlayer();
        ResetBall(2);
        float fadetime2 = GameObject.Find("FadeControl").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadetime2);
<<<<<<< HEAD
        readyCounter = 0f;
        
=======
>>>>>>> origin/midterm
    }

    IEnumerator Wait()
    {
        const float waitTime = 3f;
        float counter = 0f;
        
        while (counter < waitTime)
        {
            counter += Time.fixedDeltaTime;
            yield return null; //Don't freeze Unity
        }
        gameManager.EndGame();
    }

    void CallReady()
    {

        
        if (readyCounter >= 0f && readyCounter < 3.5f)
            {
            readyCounter += Time.deltaTime;
            ReadyImage.transform.gameObject.SetActive(true);
                Debug.Log(readyCounter);
            }
            if (readyCounter >= 1f)
            {
            ReadyImage.transform.gameObject.SetActive(false);
            }
            if (readyCounter >= 2f)
            {
            ReadyImage.transform.gameObject.SetActive(true);
            }
            if (readyCounter >= 3f)
            {
            ReadyImage.transform.gameObject.SetActive(false);
        }
        

        
    }


    public void ResetPlayer()
    {
        player1.transform.position = new Vector3(-0.4f * mapInfo[0], 0.01f * mapInfo[1], 0); // 
        player2.transform.position = new Vector3(0.4f * mapInfo[0], 0.01f * mapInfo[1], 0);  // position.x 값은 0.35~0.4 * mapInfo[0] 정도로 할것
    }

    public void ResetBall(int winner)
    {
        switch (winner)
        {
            case 1:
                ball.transform.position = new Vector3(-0.4f * mapInfo[0], 0.8f * mapInfo[1], 0);
                break;

            case 2:
                ball.transform.position = new Vector3(0.4f * mapInfo[0], 0.8f * mapInfo[1], 0);
                break;
        }
    }

}
