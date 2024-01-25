using UEngine.GameplayTags;

namespace UEngine.GameplayAbilities
{
    public class FGameplayTagRequirements
    {
        FGameplayTagContainer RequireTags;
        FGameplayTagContainer IgnoreTags;
        public FGameplayTagRequirements()
        {
            RequireTags = new FGameplayTagContainer();
            IgnoreTags = new FGameplayTagContainer();
        }
        public bool RequirementsMet(FGameplayTagContainer Container)
        {

            bool HasRequired = Container.HasAll(RequireTags);
            bool HasIgnored = Container.HasAny(IgnoreTags);

            return HasRequired && !HasIgnored;
        }
        public bool IsEmpty()
        {
            return (RequireTags.Num() == 0 && IgnoreTags.Num() == 0);
        }
    }
}
