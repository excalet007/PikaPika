using UnityEngine;
using System.Collections;

public class PikaBottom : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Ground")
			GetComponentInParent<Pikachu> ().PlayerState = pikachuState.Ground;
	}
}
