using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PikaAnimation : MonoBehaviour {

    /***** MonoBehaviour *****/
    void Start()
    {
        pikaSprite = this.gameObject.GetComponent<SpriteRenderer>();
        spriteNumber = 0;
        coolDownTime = 0f;
    }

    void Update () {
        //switch case 에서 coolDownTime 초기화할 필요 없나?
        checkAnimationState();
        playAnimation();

    }

    /***** Method & Variables *****/

    private pikachuAnimationState pikaAniState;

    private void checkAnimationState()
    {
        refPikachuScript = GetComponentInParent<Pikachu>();

        switch (refPikachuScript.PlayerState)
        {
            case pikachuState.Ground:
                pikaAniState = pikachuAnimationState.Walk;
                if (refPikachuScript.PikaVelocity == new Vector3(0, 0, 0))
                    pikaAniState = pikachuAnimationState.Stay;
                break;

            case pikachuState.Jump:
            case pikachuState.AirDrop:
                pikaAniState = pikachuAnimationState.Jump;
                // 점프상태일때 스매쉬 입력버튼 하면 (normal제외하고 모두에 대하여 스매쉬 에니메이션)
                if (refPikachuScript.HitState != pikachuHitState.Normal)
                {
                    pikaAniState = pikachuAnimationState.Smash;
                }
                break;

            case pikachuState.Receive_Left:
            case pikachuState.Receive_Right:
                pikaAniState = pikachuAnimationState.Receive;
                break;

            default:
                print("checkAnimation State 에서 디폴트가 뜨네요?");
                break;

        }
    }

    private void playAnimation()
    {
        //초기화
        switch (pikaAniState)
        {
            case pikachuAnimationState.Stay:
                stayAnimation();
                break;

            case pikachuAnimationState.Walk:
                walkAnimation();
                break;

            case pikachuAnimationState.Jump:
                jumpAnimation();
                break;

            case pikachuAnimationState.Receive:
                receiveAnimation();
                break;

            case pikachuAnimationState.Smash:
                smashAnimation();
                break;
        }
    }

    public Sprite[] walk;
    public Sprite[] jump;
    public Sprite[] receive;
    public Sprite[] smash;
    public Sprite[] win;
    public Sprite[] lose;

    private int spriteNumber;

    /// <summary>
    /// [0] = walk, [1] = jump, [2] = receive, [3] = smash
    /// </summary>
    public float[] cycleInOneSecond = { 2f, 1f, 3f, 1.5f };
    private float spriteChangeTime;
    private float coolDownTime;

    private SpriteRenderer pikaSprite;
    private Pikachu refPikachuScript;



    private void stayAnimation()
    {
        pikaSprite.sprite = walk[0];
    }

    private void walkAnimation()
    {
        spriteChangeTime = (1 / cycleInOneSecond[0]) / walk.Length;
        //print("sprite Change Time in walk is" + spriteChangeTime);

        if (spriteNumber >= walk.Length)
            spriteNumber = 0;
        pikaSprite.sprite = walk[spriteNumber];

        coolDownTime += Time.deltaTime;
        if (coolDownTime >= spriteChangeTime)
        {
            spriteNumber++;
            coolDownTime = 0;
        }
    }

    private void jumpAnimation()
    {
        spriteChangeTime = (1 / cycleInOneSecond[1]) / jump.Length;
        //print("sprite Change Time in jump is" + spriteChangeTime);

        if (spriteNumber >= jump.Length)
            spriteNumber = 0;
        pikaSprite.sprite = jump[spriteNumber];

        coolDownTime += Time.deltaTime;
        if (coolDownTime >= spriteChangeTime)
        {
            spriteNumber++;
            coolDownTime = 0;
        }
    }


    private void receiveAnimation()
    {
        if(refPikachuScript.PikaVelocity.y > 0)
        {
            pikaSprite.sprite = receive[0];
        }
        else if(refPikachuScript.PikaVelocity.y <= 0)
        {
            pikaSprite.sprite = receive[2];
        }
    }

    private void smashAnimation()
    {
        spriteChangeTime = (1f / 1.5f) / smash.Length;
        //print("sprite Change Time in smash is" + spriteChangeTime);

        if (spriteNumber >= smash.Length)
            spriteNumber = 0;
        pikaSprite.sprite = smash[spriteNumber];

        coolDownTime += Time.deltaTime;
        if (coolDownTime >= spriteChangeTime)
        {
            spriteNumber++;
            coolDownTime = 0;
        }

    }

    private void winAnimation()
    {

    }

    private void loseAnimation()
    {

    }

}
