namespace Player
{
    public enum PlayerState
    {
        Idle, // 0
        Moving, // 1
        Attacking1Closed, // 2  
        Attacking1Open, // 3
        Attacking2Closed, // 4
        Attacking2Open, // 5
        Attacking3Closed, // 6
        Attacking3Open, // 7
        RollingInvincible, // 8
        RollingHittable, // 9
        Stunned, // 10
        JumpingDelay,//11
        Jumping, //12
    }
}