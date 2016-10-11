using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour {
	private PlayManager instance = null; //for singleton design
    public GameManager gameManager;
    public Pikachu pikachu;

	public GameObject player1;
	public GameObject player2;

	private static playState playState;
	private static int score1;
	private static int score2;
    public Image Score1Image;
    public Image Score2Image;
    public Image GameSetImage;

    public static Sprite[] scoreImageList = new Sprite[16]; // 점수 스프라이트를 불러오기 위한 배열
    

    public static float[] mapInfo = new float[5]{20f, 5f, 0.2f, 2f, 0.2f}; //0: map width, 1: map height, 2: net width, 3: net height, 4: topnet height
	public static float ballRadius = 0.5f;
	public static float pikaBelly = 0.5f;
	public static float pikaBot = 0.5f;


	/***** Getters and Setters *****/
	public static int Score1 {
		get {
			return score1;
		}
		set {
			score1 = value;
		}
	}

	public static int Score2 {
		get {
			return score2;
		}
		set {
			score2 = value;
		}
	}

	/***** MonoBehaviour *****/
	void Awake() { //for singleton design
		if (instance == null)
			instance = this;
		else
			Destroy (this);

        
    }

	void Start() {
		player1.AddComponent<Pikachu> ().PlayerNum = 1; //adding scripts to both players
		player2.AddComponent<Pikachu> ().PlayerNum = 2;

		score1 = 0;
		score2 = 0;

        Score1Image = GameObject.Find("Score1").GetComponent<Image>();
        Score2Image = GameObject.Find("Score2").GetComponent<Image>();
        GameSetImage = GameObject.Find("GameSet").GetComponent<Image>();
        
        

        for (int i = 0; i < 16; i++)
        {
            scoreImageList[i] = Resources.Load<Sprite>("Score/ScoreNumbers_" + i);

        }
        Score1Image.sprite = scoreImageList[score1];
        Score2Image.sprite = scoreImageList[score2];

        GameSetImage.sprite = Resources.Load<Sprite>("게임화면_GameSet");
        GameSetImage.transform.gameObject.SetActive(false);
    }

    void Update()
    {



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

        GameSet();
    }

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
    

    IEnumerator Wait()
    {
        const float waitTime = 3f;
        float counter = 0f;
        
        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            yield return null; //Don't freeze Unity
        }
        gameManager.EndGame();
    }

}
