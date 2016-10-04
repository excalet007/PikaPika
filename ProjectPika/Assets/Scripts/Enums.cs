using System;

public enum GameState {
	Menu, In_Play
};

public enum PlayState {
	Load, SetStart, SetReady, Playing, RallyEnd, SetEnd	
};

public enum BallState {
	Normal, Smash, Slow, GameOver
};

public enum PikachuState {
	Ground, Jump, AirDrop, Receive, GameOver
};

public enum PikachuHitState {
	Normal, HitSmash, HitSlow
};