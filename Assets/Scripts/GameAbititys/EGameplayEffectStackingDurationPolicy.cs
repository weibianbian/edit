namespace GameplayAbilitySystem
{
    public enum EGameplayEffectStackingDurationPolicy
    {
        /** 该效果的持续时间将从任何成功的堆栈应用程序中刷新 */
        RefreshOnSuccessfulApplication,

        /**特效的持续时间永远不会刷新 */
        NeverRefresh,
    }
}

