namespace GameplayAbilitySystem
{
    public class FActiveGameplayEffectHandle
    {
        public int Handle;
        private bool bPassedFiltersAndWasExecuted;
        public FActiveGameplayEffectHandle(int InHandle)
        {
            Handle = InHandle;
            bPassedFiltersAndWasExecuted = true;
        }
        public FActiveGameplayEffectHandle()
        {
            Handle = -1;
            bPassedFiltersAndWasExecuted = false;
        }
        public static FActiveGameplayEffectHandle GenerateNewHandle(UAbilitySystemComponent OwningComponent)
        {
            FActiveGameplayEffectHandle NewHandle = new FActiveGameplayEffectHandle();
            GlobalActiveGameplayEffectHandles.Map.Add(NewHandle, OwningComponent);
            return NewHandle;
        }
        public void RemoveFromGlobalMap()
        {
            GlobalActiveGameplayEffectHandles.Map.Remove(this);
        }
    }
}
