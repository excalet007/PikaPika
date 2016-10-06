using UnityEngine;
using System.Collections;

public class FileManager : MonoBehaviour {

	private Sprite[] scoreList = new Sprite[16];
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 16; i++) {
			scoreList [i] = Resources.Load ("ScoreNumbers_" + i) as Sprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
