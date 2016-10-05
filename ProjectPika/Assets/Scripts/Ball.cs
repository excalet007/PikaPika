using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    // 다른 곳과 공유하는 정보
    public ballState bS = ballState.Normal;
    public bouncePoint bP = bouncePoint.None;

    // 벽이외에 부딪히는 사물(피카츄)
    public GameObject pikaObject;
    public GameObject pikaHead;
    public GameObject pikaBody;
    public GameObject pikaTail;

    // Object 크기 정보
    private float width = 20;
    private float height = 5;
    private float netWidth = 0.2f;
    private float netTopHeight = 0.2f;
    private float netHeight = 2f;
    private float ballRadius = 0.5f;

    // fxied updated -> trigger -> collosion -> update 순으로 처리가 일어난다.
    public float gravity = -1f;
    public float totalSpeedMultiplier = 1f;
    public float ySpeedMultiplier = 1f;
    public float xSpeedMultiplier = 1f;
    public float keyTestSpeed = 0.1f;
    public float reflectionCoefficient = 1.2f;

    public Vector3 velocity = new Vector3(0, 0, 0);

    // 1. 공 디버깅용 함수 + 씬리셋
    void KeyControl()
    {
        // R키= 리셋
        if (Input.GetKey(KeyCode.R))
        {
            SceneReset();
        }

        // q,w,e 시스템 (노말, 스메쉬, 슬로우)
        if (Input.GetKey(KeyCode.Q))
        {
            bS = ballState.Normal;
        }
        if (Input.GetKey(KeyCode.W))
        {
            bS = ballState.Smash;
        }
        if (Input.GetKey(KeyCode.E))
        {
            bS = ballState.Slow;
        }

        // 공 상하좌우 (방향키 상하좌우)
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

    void SceneReset()
    {
        SceneManager.LoadScene("GamePlay");
    }


    // 2. 충돌 예측함수과 필요한 변수
    private float nextPositionX, nextPositionY;
    private float currentPositionX, currentPositionY;
    
    void Bounce()
    {
        // 초기화(아무것도 안부딪혔다고 가정한다)
        bP = bouncePoint.None;

        // 예측을 위해  현재 위치값, 다음프레임 위치값을 변수로 설정
        currentPositionX = this.transform.position.x;
        currentPositionY = this.transform.position.y;
        nextPositionX = this.transform.position.x + velocity.x*Time.deltaTime;
        nextPositionY = this.transform.position.y + velocity.y*Time.deltaTime;

        //1p범위(x<0) //
        if (currentPositionX < 0)
        {
            // 코너링은 잠시만 대기하자 머리터지겄다 슈발
            // 좌측벽
            if (nextPositionX <= -(width / 2 - ballRadius))
            {
                bP = bouncePoint.LeftWall;
            }
            // 좌측천장
            if (nextPositionY >= (height - ballRadius))
            {
                bP = bouncePoint.Top;
            }
            // 좌네트벽
            if (nextPositionX >= -(netWidth/2 +ballRadius) && (nextPositionY >= (ballRadius) && nextPositionY <= (netHeight-ballRadius)))
            {
                bP = bouncePoint.LeftNet;
            }
            // 네트윗부분
            if (nextPositionX >= -(netWidth/2 + ballRadius) && (nextPositionY > (netHeight - ballRadius) && nextPositionY <= (netHeight + ballRadius+netTopHeight)))
            {
                bP = bouncePoint.TopNet;
            }
            // 바닥(좌바닥으로 나누어도 됨)
            if (nextPositionY <= ballRadius)
            {
                bP = bouncePoint.LeftGround;
            }
             
        }

        //2p범위(x>=0)
        else if (currentPositionX >= 0)
        {
            //우측벽
            if (nextPositionX >= (width / 2 - ballRadius))
            {
                bP = bouncePoint.RightWall;
            }
            // 우측천장
            if (nextPositionY >= (height - ballRadius))
            {
                bP = bouncePoint.Top;
            }
            // 우측네트벽
            if (nextPositionX <= (netWidth / 2 + ballRadius) && (nextPositionY >= (ballRadius) && nextPositionY <= (netHeight - ballRadius)))
            {
                bP = bouncePoint.RightNet;
            }
            // 네트윗부분
            if (nextPositionX <= -(netWidth / 2 + ballRadius) && (nextPositionY > (netHeight - ballRadius) && nextPositionY <= (netHeight + ballRadius + netTopHeight)))
            {
                bP = bouncePoint.TopNet;
            }
            // 바닥(좌바닥으로 나누어도 됨)
            if (nextPositionY <= ballRadius)
            {
                bP = bouncePoint.RightGround;
            }
        }

        // 만약 탈주할경우
        if (Mathf.Abs(this.transform.position.x) >= 2*width)
        {
            Debug.Log("탈주닌자닷!!!");
        }
    }

    // 3. 그리고 나서 속도계산
    void CalculateVelocity()
    {
        // 중력가속도를 더한다.(모든것
        velocity += Vector3.up * gravity * Time.deltaTime;

        switch (bP)
        {
            case bouncePoint.None:
                // 지금 이대로만 가다오
                break;

            case bouncePoint.Top:
                if (bS == ballState.Normal)
                {
                    velocity.y = -Mathf.Abs(velocity.y);
                }
                else if (bS == ballState.Smash)
                {
                    velocity.x *= reflectionCoefficient;
                    velocity.y = -Mathf.Abs(velocity.y);
                }
                else if (bS == ballState.Slow)
                {
                    velocity.y = -Mathf.Abs(velocity.y);
                }
                else if (bS == ballState.GameOver)
                {
                    velocity.y = -Mathf.Abs(velocity.y);
                }
                break;

            case bouncePoint.LeftGround:
                velocity.x *= 0.9f;
                velocity.y = 0.9f * Mathf.Abs(velocity.y);
                break;

            case bouncePoint.RightGround:
                velocity.x *= 0.9f;
                velocity.y = 0.9f * Mathf.Abs(velocity.y);
                break;

            case bouncePoint.LeftWall:
                velocity.x = Mathf.Abs(velocity.x);
                break;

            case bouncePoint.RightWall:
                velocity.x = -Mathf.Abs(velocity.x);
                break;

            case bouncePoint.TopNet:
                velocity.y = Mathf.Abs(velocity.y);
                break;

            case bouncePoint.LeftNet:
                velocity.x = -Mathf.Abs(velocity.x);
                break;

            case bouncePoint.RightNet:
                velocity.x = Mathf.Abs(velocity.x);
                break;

            default:
                print("Default State!");
                break;
        }

        // 나중에 확장을 위해 각 방향속도, 전체속도 Multiplier를 곱한다
        velocity.x *= xSpeedMultiplier;
        velocity.y *= ySpeedMultiplier;
        velocity *= totalSpeedMultiplier;
    }

    // 4. 속도에 따른 이동
    void MoveBall()
    {
        // 속력에 따라 초당 velocity만큼 이동한다(프레임 단위로 찢어서 이동)
        // 다만 부딪힌 경우에는 그 벽에 딱 달라붙는다 그 프레임에서

        switch (bP)
        {
            case bouncePoint.None:
                this.transform.position += velocity * Time.deltaTime;
                break;

            case bouncePoint.Top:
                this.transform.position = new Vector3(this.transform.position.x, (height-ballRadius), this.transform.position.z);
                break;

            case bouncePoint.LeftGround:
                this.transform.position = new Vector3(this.transform.position.x, (ballRadius), this.transform.position.z);
                break;

            case bouncePoint.RightGround:
                this.transform.position = new Vector3(this.transform.position.x, (ballRadius), this.transform.position.z);
                break;

            case bouncePoint.LeftWall:
                this.transform.position = new Vector3(-(width / 2 - ballRadius), this.transform.position.y, this.transform.position.z);
                break;

            case bouncePoint.RightWall:
                this.transform.position = new Vector3((width / 2 - ballRadius), this.transform.position.y, this.transform.position.z);
                break;

            case bouncePoint.TopNet:
                this.transform.position = new Vector3(this.transform.position.x, (netHeight + netTopHeight + ballRadius), this.transform.position.z);
                break;

            case bouncePoint.LeftNet:
                this.transform.position = new Vector3((-netWidth/2 - ballRadius), this.transform.position.y, this.transform.position.z);
                break;

            case bouncePoint.RightNet:
                this.transform.position = new Vector3((netWidth / 2 + ballRadius), this.transform.position.y, this.transform.position.z);
                break;

            default:
                print("Default State!");
                break;
        }

    }

    void Update()
    {
        KeyControl();
        Bounce();
        CalculateVelocity();
        MoveBall();
    }

} 
