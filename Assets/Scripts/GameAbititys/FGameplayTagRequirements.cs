using RailShootGame;

namespace GameplayAbilitySystem
{
    public class FGameplayTagRequirements
    {
        FGameplayTagContainer RequireTags;
        FGameplayTagContainer IgnoreTags;
        public bool RequirementsMet(FGameplayTagContainer Container)
        {

            bool HasRequired = Container.HasAll(RequireTags);
            bool HasIgnored = Container.HasAny(IgnoreTags);

            return HasRequired && !HasIgnored;
        }
    }
}
