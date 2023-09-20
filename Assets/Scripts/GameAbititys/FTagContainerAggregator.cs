using RailShootGame;

namespace GameplayAbilitySystem
{
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
    }

}

