using UnityEngine;
using System.Collections;

public class SlowMotionScript : MonoBehaviour {

    private float slowTimeScale;
    private float factor = 4f;

	// Use this for initialization
	void Start () {
        slowTimeScale = Time.timeScale / factor;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.P))
            slowMotion();
	}

    void slowMotion()
    {
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
    }
}
