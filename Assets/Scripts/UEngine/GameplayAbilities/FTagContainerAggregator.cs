using UEngine.GameplayTags;

namespace UEngine.GameplayAbilities
{
    //结构，用于在效果执行过程中组合来自不同来源的标签
    public class FTagContainerAggregator
    {
        public bool CacheIsValid = false;
        public FGameplayTagContainer CachedAggregator = new FGameplayTagContainer();
        public FGameplayTagContainer CapturedActorTags = new FGameplayTagContainer();
        public FGameplayTagContainer CapturedSpecTags = new FGameplayTagContainer();
        public FGameplayTagContainer ScopedTags = new FGameplayTagContainer();
        public FGameplayTagContainer GetAggregatedTags()
        {
            if (CacheIsValid == false)
            {
                CacheIsValid = true;
                CachedAggregator.Reset();
                CachedAggregator.AppendTags(CapturedActorTags);
                CachedAggregator.AppendTags(CapturedSpecTags);
                CachedAggregator.AppendTags(ScopedTags);
            }

            return CachedAggregator;
        }
        public FGameplayTagContainer GetSpecTags()
        {
            return CapturedSpecTags;
        }
    }
}

