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
    Ground, Jump, AirDrop, Receive_Left, Receive_Right, GameOver
}

public enum pikachuHitState
{
    Normal, HitSmash_Left, HitSmash_Right, HitSmash_UpLeft, HitSmash_UpRight, HitSmash_DownRight, 
	HitSmash_DownLeft, HitSmash_Down, HitSmash_Up, HitSlow
}
	
