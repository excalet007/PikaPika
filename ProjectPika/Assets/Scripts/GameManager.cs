using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    


    public void SinglePlay()
    {
        SceneManager.LoadScene(1);
    }

    public void MultiPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
    
}
