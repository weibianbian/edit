namespace GameplayAbilitySystem
{
    public enum GameplayCueEvent
    {
        /** 当具有持续时间的GameplayCue第一次被激活时调用，只有当客户端目睹激活时才会调用 */
        OnActive,

        /** 当具有持续时间的游戏提示第一次被视为活动时调用，即使它实际上没有被应用(加入进程等) */
        WhileActive,

        /** 当执行GameplayCue时调用，它用于即时特效或周期时钟 */
        Executed,

        /** 当带有持续时间的游戏提示被移除时调用 */
        Removed
    }
}

