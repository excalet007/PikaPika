using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour {
	private PlayManager instance = null; //for singleton design

	public GameObject player1;
	public GameObject player2;

	private static playState playState;
	private static int score1;
	private static int score2;

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
	}
}
