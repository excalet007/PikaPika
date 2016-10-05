using UnityEngine;
using System.Collections;

public class PlayManager : MonoBehaviour {

	private static playState playState;
	private static int score1;
	private static int score2;

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
	void Start() {
		score1 = 0;
		score2 = 0;
	}
}
