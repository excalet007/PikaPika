using UnityEngine;
using System.Collections;

public enum playState
{
    Load, SetStart, SetReady, Playing, RallyEnd, SetEnd
}

public enum ballState
{
    Normal, Smash, Slow, GameOver
}

// bounce State 예측을 위한 변경사항
public enum bouncePoint
{
    None, LeftWall, RightWall, Top, LeftGround, RightGround, LeftNet, RightNet, TopNet
}

public enum pikachuState
{
    Ground, Jump, AirDrop, Receive, GameOver
}

public enum pikachuHitState
{
    Normal, HitSmash, HitShow
}

