namespace UEngine.GameplayAbilities
{
    public enum EGameplayEffectStackingExpirationPolicy
    {
        /**当激活的游戏效果到期时，整个堆栈将被清除  */
        ClearEntireStack,

        /** 当前堆栈计数会减1，并且持续时间会刷新。GE并不是“重新应用”，只是在少了一个堆栈的情况下继续存在 */
        RemoveSingleStackAndRefreshDuration,

        /** 刷新游戏效果的持续时间。这使得效果的持续时间是无限的。这可以通过OnStackCountChange回调手动处理堆栈递减 */
        RefreshDuration,
    }
}
