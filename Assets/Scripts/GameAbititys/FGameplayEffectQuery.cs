using RailShootGame;

namespace GameplayAbilitySystem
{
    public class FGameplayEffectQuery
    {
        public FGameplayTagQuery OwningTagQuery;
        public FGameplayTagQuery EffectTagQuery;
        public FGameplayTagQuery SourceTagQuery;
        public static FGameplayTagContainer TargetTags = new FGameplayTagContainer();
        public static FGameplayTagContainer GETags = new FGameplayTagContainer();
        public bool Matches(FGameplayEffectSpec Spec)
        {
            if (Spec == null)
            {
                return false;
            }
            if (OwningTagQuery.IsEmpty() == false)
            {
                TargetTags.Reset();
                if (Spec.Def.InheritableGameplayEffectTags.CombinedTags.Num() > 0)
                {
                    TargetTags.AppendTags(Spec.Def.InheritableGameplayEffectTags.CombinedTags);
                }
                if (Spec.Def.InheritableOwnedTagsContainer.CombinedTags.Num() > 0)
                {
                    TargetTags.AppendTags(Spec.Def.InheritableOwnedTagsContainer.CombinedTags);
                }
                if (Spec.DynamicGrantedTags.Num() > 0)
                {
                    TargetTags.AppendTags(Spec.DynamicGrantedTags);
                }
                //if (OwningTagQuery.Matches(TargetTags) == false)
                //{
                //    return false;
                //}
            }
            if (EffectTagQuery.IsEmpty() == false)
            {
                GETags.Reset();
                if (Spec.Def.InheritableGameplayEffectTags.CombinedTags.Num() > 0)
                {
                    GETags.AppendTags(Spec.Def.InheritableGameplayEffectTags.CombinedTags);
                }
                FGameplayTagContainer SpecDynamicAssetTags = Spec.GetDynamicAssetTags();
                if (SpecDynamicAssetTags.Num() > 0)
                {
                    GETags.AppendTags(SpecDynamicAssetTags);
                }

                //if (EffectTagQuery.Matches(GETags) == false)
                //{
                //    return false;
                //}
            }
            return true;
        }

    }

}

