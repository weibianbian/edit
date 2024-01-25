namespace UEngine.GameplayAbilities
{
    public enum EGameplayEffectDurationType
    {
        /** 此效果即刻生效 */
        Instant,
        /** 这种效果会持续到永远 */
        Infinite,
        /** 这种效果的持续时间将由一个量级来指定 */
        HasDuration
    }
}

