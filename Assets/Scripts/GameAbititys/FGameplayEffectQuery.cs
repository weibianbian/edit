namespace GameplayAbilitySystem
{
    public class FGameplayEffectQuery
    {
        public FGameplayTagQuery OwningTagQuery;
        public FGameplayTagQuery EffectTagQuery;
        public FGameplayTagQuery SourceTagQuery;
        public bool Matches(GameplayEffectSpec Spec)
        {
            if (Spec == null)
            {
                return false;
            }
            return true;
        }

    }

}

