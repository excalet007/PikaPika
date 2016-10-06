using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour {

    // SceneManager.LoadScene(1)은 시작화면, (2)는 게임화면(1P서브), (3)은 게임화면(2P서브)


   
}

public class InGameScene
{
    
    public GameObject gameObject;
    public Image[] scoreImages;
    Score score1 = 0;
    Score score2 = 0;
    public int winner;

    public void UpdateScore(int winner)
    {
        if (winner == 1)
            score1++;
        else score2++;
    }
    
   /* public IEnumerator ResetScene(int winner)
    {
        if(winner == 1)// 승자가 누구인지 받아 승자가 서브를 할 수 있도록 공의 위치를 초기화
        {
            UpdateScore(1);
            Time.timeScale = 0.5f;
            float fadeTime = GameObject.Find("Game Control").GetComponent<Fading>().BeginFade(1); //Game Control 게임오브젝트에서 Fading 스크립트를 찾아서 BeginFade() 실행
            yield return new WaitForSeconds(fadeTime);
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(1); // 
        }

        else
        {
            UpdateScore(2);
            Time.timeScale = 0.5f;
            float fadeTime = GameObject.Find("Game Control").GetComponent<Fading>().BeginFade(1); //Game Control 게임오브젝트에서 Fading 스크립트를 찾아서 BeginFade() 실행
            yield return new WaitForSeconds(fadeTime);
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(1); // 
        }
    }
    */
    

    void EndGameAnimation() // 15점이 나고 게임이 끝났을 때 호출
    {
        //피카츄의 승리, 패배 애니메이션 호출
        // Game Set! 메세지 호출

    }

    IEnumerator Endgame()
    {
        yield return new WaitForSeconds(5);
        float fadeTime = GameObject.Find("Game Control").GetComponent<Fading>().BeginFade(1); //Game Control 게임오브젝트에서 Fading 스크립트를 찾아서 BeginFade() 실행
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(0); // 시작화면 호출
    }
}

