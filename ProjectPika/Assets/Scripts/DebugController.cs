using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour {

    // reset 함수
    public  void SceneReset()
    {
        SceneManager.LoadScene("GamePlay");
    }

	// 상하좌우 컨트롤러 , R키는 씬 재시작
	void Update ()
    {

        if(Input.GetKey(KeyCode.R))
        {
            SceneReset();
        }

        // 상하좌우
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += new Vector3(0, 2, 0)*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position += new Vector3(0, -2, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += new Vector3(-2, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(2, 0, 0) * Time.deltaTime;
        }
    }
}
