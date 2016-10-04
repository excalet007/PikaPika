using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    // fxied updated -> trigger -> collosion -> update 순으로 처리가 일어난다.
    public float gravity = -1f;
    public float totalSpeedMultiplier = 1f;
    public float ySpeedMultiplier = 1f;
    public float xSpeedMultiplier = 1f;
    public float keyTestSpeed = 0.25f;

    public Vector3 velocity = new Vector3(0, 0, 0);


   void CalculateVelocity()
    {
        // 중력가속도를 더한다.
        velocity += Vector3.up * gravity * Time.deltaTime; 
 
        // 나중에 확장을 위해 각 방향속도, 전체속도 Multiplier를 곱한다
        velocity.x *= xSpeedMultiplier;
        velocity.y *= ySpeedMultiplier;
        velocity *= totalSpeedMultiplier;
    }
 
    void MoveBall()
    {
        // 속력에 따라 초당 velocity만큼 이동한다(프레임 단위로 찢어서 이동)
        this.transform.position += velocity * Time.deltaTime;
    }

    void SceneReset()
    {
        SceneManager.LoadScene("GamePlay");
    }

    void KeyControl()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneReset();
        }

        // 상하좌우
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity.y += keyTestSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity.y -= keyTestSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.x -= keyTestSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x += keyTestSpeed;
        }
    }

   void Update()
    {
        KeyControl();
        CalculateVelocity();
        MoveBall();
    }

    void OnTriggerEnter(Collider other)
    {
        // elseif로 하면 에러 생기지 않을까? 실험해보자 나중에

        if (other.CompareTag("LeftWall")) // x값이 양수로
        {
            velocity.x = Mathf.Abs(velocity.x);
        }
        if (other.CompareTag("RightWall")) // x값이 음수로
        {
            velocity.x = -Mathf.Abs(velocity.x);
        }
        if (other.CompareTag("Top"))
        {
           // ballState.Spike
        }
    }

    
}
