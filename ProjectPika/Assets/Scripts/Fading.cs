using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

    public Texture2D fadeOutTexture; // 씬 전환 사이에 들어올 검은 화면
    public float fadeSpeed = 0.8f; // 페이드 속도

    private int drawDepth = -1000; // 검은 화면이 맨 위에 렌더링되도록
    private float alpha = 1.0f; // 텍스쳐의 알파값, 0에서 1 사이
    private int fadeDir = -1; // 페이드 방향 --> 페이드인 = -1, 페이드아웃 = 1

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.fixedDeltaTime;

        alpha = Mathf.Clamp01(alpha); // alpha값이 0과 1 사이를 벗어나지 못하도록 설정

        //GUI의 색을 설정하는데, 색은 변하지 않아야 하고 알파값이 alpha로 설정되도록
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha); // 알파값 설정
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture); // 텍스쳐가 화면 전부를 가리도록
    }

    // fadeDir를 설정, -1이면 페이드인 1이면 페이드아웃
    
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed); // 속도를 반환해, Application.LoadLevel 시간을 재기 쉽도록 함
    }

    // OnLevelWasLoaded는 씬이 로드될때마다 호출되며, 
    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }
}
