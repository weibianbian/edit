namespace UEngine.GameplayAbilities
{
    public enum EGameplayEffectStackingPeriodPolicy
    {
        /**在任何成功的堆栈应用程序中，朝向周期性效果的下一个时钟周期的任何进展都会被丢弃 */
        ResetOnSuccessfulApplication,

        /** 无论堆栈应用如何，周期性效果的下一个时钟周期的进度永远不会重置 */
        NeverReset,
    };
}

