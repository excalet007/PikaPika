using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{

    /***** MonoBehaviour *****/
    void Start()
    {
        ballVelocity = new Vector3(0, 0, 0);
        ballState = ballState.Normal;
    }

    void FixedUpdate()
    {
        KeyControl();
        Bounce();
        CalculateVelocity();
        MoveBall();
        ErrorCheck();
    }
    
    void Update()
    {
        BallAnimation();
    }

    /***** Getters and Setters *****/
    public ballState BallState
    {
        get
        {
            return ballState;
        }

        set
        {
            ballState = value;
        }
    }

    /***** Methods and Variables *****/

    // 1. 키콘트롤 & 씬 리셋
    void KeyControl()
    {
        // R키= 리셋
        if (Input.GetKey(KeyCode.R))
        {
            SceneReset();
        }
    }
    void SceneReset()
    {
        SceneManager.LoadScene("GamePlay");
    }


    // 2. 충돌계산
    private RaycastHit2D hitCollider;
    private Vector2 distanceVector;
    private float ballRadius = 0.5f;
    public GameObject player1;
    public GameObject player2;
    private Vector2 pika1Velocity;
    private Vector2 pika2Velocity;

    void Bounce()
    {
        // 피카츄 콜라이더 오프셋
        pika1Velocity = player1.GetComponent<Pikachu>().PikaVelocity;
        pika2Velocity = player2.GetComponent<Pikachu>().PikaVelocity;

        //time.deltatime 생각해보기`
        player1.GetComponent<PolygonCollider2D>().offset = pika1Velocity;  
        player2.GetComponent<PolygonCollider2D>().offset = pika2Velocity;

        //충돌판정
        hitCollider = Physics2D.CircleCast(this.transform.position, ballRadius, ballVelocity, ballVelocity.magnitude * Time.fixedDeltaTime * 1.1f);
    }
    

    // 3. 속도계산 
    private ballState ballState;
    public float gravity = -10f;
    public float reflectionCoefficient = 1.3f;
    public static float maxSpeed = 25f;
    public static float minSpeed = 8f;
    public static Vector3 ballVelocity = new Vector3(0, 0, 0);

    void CalculateVelocity()
    {
        // 3.1 중력가속도 계산
        ballVelocity = new Vector3(ballVelocity.x, ballVelocity.y + gravity * Time.fixedDeltaTime, ballVelocity.z);

        // 3.2 충돌시 계산
        if (hitCollider.collider != null)
        {
            //3.2.1 일반충돌
            ballVelocity = Vector3.Reflect(ballVelocity, hitCollider.normal);
            
            //3.2.2 스매쉬 상태에서 천장충돌(x값 배수곱셈)
            if (hitCollider.collider.gameObject.CompareTag("top") == true && ballState == ballState.Smash)
                ballVelocity.x = reflectionCoefficient;

            //3.2.3 바닥충돌 _ 점수증가
            if (hitCollider.collider.gameObject.CompareTag("bottom") == true)
            {
                ballVelocity *= 0.8f;
                if (this.transform.position.x <= 0)
                {
                    PlayManager.Instance.Score2++;
                }
                else if (this.transform.position.x > 0)
                {
                    PlayManager.Instance.Score1++;
                }
            }

            if (hitCollider.collider.gameObject.CompareTag("player1") == true)
            {
                pikachuHitState pika1HitState = hitCollider.collider.gameObject.GetComponent<Pikachu>().HitState;
                //print("지금 힛스테이트는요... " +pika1HitState.ToString());

                if(ballVelocity.y < 0 && pika1Velocity.y >=0)
                {
                    ballVelocity = new Vector3(ballVelocity.x, Mathf.Abs(ballVelocity.y), ballVelocity.z);
                    //print("아래로 꺽이는 것을 방지하였습니다!");
                }

                switch (pika1HitState)
                {
                    case pikachuHitState.HitSlow:

                        ballState = ballState.Smash;
                        break;
                    case pikachuHitState.HitSmash_Down:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3((ballVelocity.x / ballVelocity.x) * 0.15f, -1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_DownLeft:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(-1, -1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_DownRight:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(1, -1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_Left:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(-1, -0.15f, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_Right:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(1, -0.15f, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_Up:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3((ballVelocity.x / ballVelocity.x) * 0.15f, 1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_UpLeft:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(-1, 1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_UpRight:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(1, 1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    default:
                        break;

                }

                if (ballVelocity.magnitude * Time.fixedDeltaTime <= (pika1Velocity.magnitude))
                {
                    //print("ball velocity is slow then pika velocity " + ballVelocity.magnitude);
                    ballVelocity *= (pika1Velocity.magnitude / (ballVelocity.magnitude * Time.fixedDeltaTime));
                    //print("now ball velocity is " + ballVelocity.magnitude);
                }

            }

            if (hitCollider.collider.gameObject.CompareTag("player2") == true)
            {
                pikachuHitState pika2HitState = hitCollider.collider.gameObject.GetComponent<Pikachu>().HitState;
              
                if (ballVelocity.y < 0 && pika1Velocity.y >= 0)
                {
                    ballVelocity = new Vector3(ballVelocity.x, Mathf.Abs(ballVelocity.y), ballVelocity.z);
                }

                switch (pika2HitState)
                {
                    case pikachuHitState.HitSlow:

                        ballState = ballState.Smash;
                        break;
                    case pikachuHitState.HitSmash_Down:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3((ballVelocity.x / ballVelocity.x) * 0.15f, -1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_DownLeft:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(-1, -1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_DownRight:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(1, -1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_Left:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(-1, -0.15f, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_Right:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(1, -0.15f, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_Up:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3((ballVelocity.x / ballVelocity.x) * 0.15f, 1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_UpLeft:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(-1, 1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    case pikachuHitState.HitSmash_UpRight:

                        ballState = ballState.Smash;
                        ballVelocity = new Vector3(1, 1, 0);
                        ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed;
                        break;
                    default:
                        break;

                }

                if (ballVelocity.magnitude * Time.fixedDeltaTime <= (pika2Velocity.magnitude))
                {
                    //print("ball velocity is slow then pika velocity " + ballVelocity.magnitude);
                    ballVelocity *= (pika2Velocity.magnitude / (ballVelocity.magnitude * Time.fixedDeltaTime));
                    //print("now ball velocity is " + ballVelocity.magnitude);
                }

            }

        }

        // 3.3 최대/ 최소속도 제한
        if (ballVelocity.magnitude >= maxSpeed)
        {
            ballVelocity = ballVelocity / ballVelocity.magnitude * maxSpeed * 0.95f; ;
        }
        else if (ballVelocity.magnitude < minSpeed)
        {
            ballVelocity = ballVelocity / ballVelocity.magnitude * minSpeed * 1.05f;
        }

    }


    // 4. 이동
    void MoveBall()
    {
        //일반이동
        this.transform.position += ballVelocity * Time.fixedDeltaTime;

        //피카충돌시 이동 보정
        if (hitCollider.collider != null)
        {
<<<<<<< HEAD
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
=======
            if (hitCollider.collider.CompareTag("player1") == true)
            {
                float pika1TempX = pika1Velocity.x;
                float pika1TempY = pika1Velocity.y; 
                                
                Vector3 pika1TempVelocity = new Vector3(pika1TempX, pika1TempY, 0);
                this.transform.position += pika1TempVelocity*1.5f;
                //this.transform.position += pika1TempVelocity*-1f;
            }

            if (hitCollider.collider.CompareTag("player2") == true)
            {
                float pika2TempX = pika2Velocity.x;
                float pika2TempY = pika2Velocity.y;

                Vector3 pika2TempVelocity = new Vector3(pika2TempX, pika2TempY, 0);
                this.transform.position += pika2TempVelocity*1.5f;
            }
>>>>>>> origin/midterm
        }
    }

    // 5. 디버그용 함수
    void ErrorCheck()
    {
        float smallOffSet = 0.05f;
        // 탈주체크
        if (this.transform.position.x < -(PlayManager.mapInfo[0]/2 - ballRadius)){
            print("왼쪽 탈주");
            this.transform.position = new Vector3(-(PlayManager.mapInfo[0] / 2 - ballRadius) +smallOffSet, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.x > (PlayManager.mapInfo[0] / 2 - ballRadius)){
            print("오른쪽 탈주");
            this.transform.position = new Vector3((PlayManager.mapInfo[0] / 2 - ballRadius) - smallOffSet, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.y > (PlayManager.mapInfo[1]- ballRadius)){
            print("위쪽 탈주");
            this.transform.position = new Vector3(this.transform.position.x , PlayManager.mapInfo[1] -ballRadius- smallOffSet, this.transform.position.z);
        }
        if (this.transform.position.y < (ballRadius)){
            print("아래쪽 탈주");
            this.transform.position = new Vector3(this.transform.position.x, ballRadius + smallOffSet, this.transform.position.z);
        }


        // 콜라이더 체크
        if (hitCollider.collider != null)
        {
            Debug.DrawLine(hitCollider.point, hitCollider.point + Vector2.left * ballRadius, Color.blue);
            Debug.DrawLine(hitCollider.point, hitCollider.point + Vector2.up * ballRadius, Color.blue);
            Debug.DrawLine(hitCollider.point, hitCollider.point + Vector2.down * ballRadius, Color.blue);
            Debug.DrawLine(hitCollider.point, hitCollider.point + Vector2.right * ballRadius, Color.blue);
            //distanceVector = hitCollider.point - (Vector2)this.transform.position;
            //Debug.Log("부딪힌 점과 중점에서의 거리는 " + distanceVector.magnitude);
            //Debug.Log("이동 거리는 " + ballVelocity.magnitude * Time.fixedDeltaTime);
            //Debug.Log("distance는 " + hitCollider.distance);
            //Debug.Log("fraction은 " + hitCollider.fraction);
        }
        Debug.DrawRay(this.transform.position, ballVelocity / ballVelocity.magnitude, Color.green);
    }

    // 6. 볼 에니메이션
    private float tunningSpeed;     // 양수 = 시계 반대방향  음수 = 시계방향
    public Sprite[] afterImages;

    void BallAnimation()
    {
        if(ballState == ballState.Normal)
        {
            tunningSpeed = ballVelocity.magnitude * 10;
            this.transform.Rotate(Vector3.forward, tunningSpeed*Time.deltaTime);
        }
        else if(ballState == ballState.Smash)
        {

        }
    }

}
