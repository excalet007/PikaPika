using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{

    /***** MonoBehaviour *****/
    void FixedUpdate()
    {
        KeyControl();
        Bounce();
        CalculateVelocity();
        MoveBall();
        ErrorCheck();
    }


    /***** Methods and Variables *****/
    // 1. 키콘트롤 & 씬 리셋
    public float keyTestSpeed = 0.1f;

    void KeyControl()
    {
        // R키= 리셋
        if (Input.GetKey(KeyCode.R))
        {
            SceneReset();
        }

        // 1,2,3 시스템 (노말, 스메쉬, 슬로우)
        if (Input.GetKey(KeyCode.Alpha1))
        {
            bS = ballState.Normal;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            bS = ballState.Smash;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            bS = ballState.Slow;
        }

        //// 공 상하좌우 (방향키 상하좌우)
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    velocity.y += keyTestSpeed;
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    velocity.y -= keyTestSpeed;
        //}
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    velocity.x -= keyTestSpeed;
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    velocity.x += keyTestSpeed;
        //}
    }
    void SceneReset()
    {
        SceneManager.LoadScene("GamePlay");
    }

    // 2. 충돌계산
    private RaycastHit2D wallHit;
    private Vector2 distanceVector;
    private float ballRadius = 0.5f;
    public GameObject player1;
    public GameObject player2;
    private Vector2 pika1Velocity;
    private Vector2 pika2Velocity;


    void Bounce()
    {
        // 피카츄 콜라이더 오프셋
        pika1Velocity = player1.GetComponent<Pikachu>().PikaVelocity / Time.deltaTime;
        pika2Velocity = player2.GetComponent<Pikachu>().PikaVelocity / Time.deltaTime;

        //time.deltatime 생각해보기
        player1.GetComponent<PolygonCollider2D>().offset = pika1Velocity * Time.deltaTime;  
        player2.GetComponent<PolygonCollider2D>().offset = pika2Velocity * Time.deltaTime;

        //벽과 튕김
        wallHit = Physics2D.CircleCast(this.transform.position, ballRadius, velocity, velocity.magnitude * Time.deltaTime);
    }


    // 3. 속도계산 
    public ballState bS = ballState.Normal;
    public float gravity = -1f;
    public float reflectionCoefficient = 1.2f;
    public float maxSpeed = 7f;
    public float minSpeed = 4.5f;
    public Vector3 velocity = new Vector3(0, 0, 0);

    void CalculateVelocity()
    {
        // 3.1 중력가속도 계산
        velocity = new Vector3(velocity.x, velocity.y + gravity * Time.deltaTime, velocity.z);

        // 3.2 충돌시 계산
        if (wallHit.collider != null)
        {
            Debug.Log("There's Collision at " + wallHit.point.ToString());

            //3.2.1 일반충돌
            velocity = Vector3.Reflect(velocity, wallHit.normal);

            //3.2.2 스매쉬 상태에서 천장충돌(x값 배수곱셈)
            if (wallHit.collider.gameObject.CompareTag("top") == true && bS == ballState.Smash)
                velocity.x = reflectionCoefficient;

            //3.2.3 바닥충돌
            if (wallHit.collider.gameObject.CompareTag("bottom"))
                velocity *= 0.85f;

        }

        // 3.3 최대/ 최소속도 제한
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity / velocity.magnitude * maxSpeed;
        }
        else if (velocity.magnitude < minSpeed)
        {
            velocity = velocity / velocity.magnitude * minSpeed;
        }
    }


    // 4. 이동
    void MoveBall()
    {
        //일반이동
        this.transform.position += velocity * Time.deltaTime;

        //피카충돌(탈주방지)
        if (wallHit.collider.CompareTag("player1") == true)
        {
            Debug.Log("It Activates!");

            float pika1TempX = pika1Velocity.x;
            float pika1TempY = pika1Velocity.y;

            Vector3 pika1TempVelocity = new Vector3(pika1TempX, pika1TempY, 0);

            this.transform.position += pika1TempVelocity * Time.fixedDeltaTime;
        }

        if (wallHit.collider.CompareTag("player2") == true)
        {
            float pika2TempX = pika2Velocity.x;
            float pika2TempY = pika2Velocity.y;

            Vector3 pika2TempVelocity = new Vector3(pika2TempX, pika2TempY, 0);

            this.transform.position += pika2TempVelocity * Time.fixedDeltaTime;
        }
    }

    // 5. 디버그용 함수
    public PlayManager playManager;

    void ErrorCheck()
    {
        // 탈주체크

        // 콜라이더 체크
        if (wallHit.collider != null)
        {
            Debug.DrawLine(wallHit.point, wallHit.point + Vector2.left * ballRadius, Color.blue);
            Debug.DrawLine(wallHit.point, wallHit.point + Vector2.up * ballRadius, Color.blue);
            Debug.DrawLine(wallHit.point, wallHit.point + Vector2.down * ballRadius, Color.blue);
            Debug.DrawLine(wallHit.point, wallHit.point + Vector2.right * ballRadius, Color.blue);
            distanceVector = wallHit.point - (Vector2)this.transform.position;
            Debug.Log("부딪힌 점과 중점에서의 거리는 " + distanceVector.magnitude);
            Debug.Log("이동 거리는 " + velocity.magnitude * Time.deltaTime);
            Debug.Log("distance는 " + wallHit.distance);
            Debug.Log("fraction은 " + wallHit.fraction);

        }
        Debug.DrawLine(this.transform.position, wallHit.point, Color.red);
        Debug.DrawRay(this.transform.position, velocity / velocity.magnitude, Color.green);
    }



}
