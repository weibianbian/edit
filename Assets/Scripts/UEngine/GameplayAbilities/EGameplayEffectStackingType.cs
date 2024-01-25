namespace UEngine.GameplayAbilities
{
    public enum EGameplayEffectStackingType
    {
        /** 没有堆积。这种游戏效果的多个应用被视为单独的实例. */
        None,
        /** 每个施法者都有自己的栈. */
        AggregateBySource,
        /** 每个目标都有自己的栈. */
        AggregateByTarget,
    };
}

