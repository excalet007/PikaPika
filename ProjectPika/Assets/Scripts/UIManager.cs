using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public GameObject score1;
	public GameObject score2;

	private Text score1_txt;
	private Text score2_txt;

	void Start () {
		score1_txt = score1.GetComponent<Text> ();
		score2_txt = score2.GetComponent<Text> ();
	}

	void Update () {
		score1_txt.text = PlayManager.Score1.ToString();
		score2_txt.text = PlayManager.Score2.ToString();
	}
}
