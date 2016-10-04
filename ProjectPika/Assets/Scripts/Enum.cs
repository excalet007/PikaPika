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

public enum pikachuState
{
    Ground, Jump, AirDrop, Receive, GameOver
}

public enum pikachuHitState
{
    Normal, HitSmash, HitShow
}

