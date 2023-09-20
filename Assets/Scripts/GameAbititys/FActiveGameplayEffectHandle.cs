using System;

namespace GameplayAbilitySystem
{
    public class FActiveGameplayEffectHandle
    {
        public static int GHandleID = 0;
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
            GHandleID++;
            FActiveGameplayEffectHandle NewHandle = new FActiveGameplayEffectHandle(GHandleID);
            GlobalActiveGameplayEffectHandles.Map.Add(NewHandle, OwningComponent);
            return NewHandle;
        }
        public void RemoveFromGlobalMap()
        {
            GlobalActiveGameplayEffectHandles.Map.Remove(this);
        }
    }
}
