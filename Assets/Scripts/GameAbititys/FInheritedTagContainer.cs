using RailShootGame;

namespace GameplayAbilitySystem
{
    //这个结构，用于以安全的方式组合父/子蓝图中的标签
    public class FInheritedTagContainer
    {
        //Tags that I inherited and tags that I added minus tags that I removed
        public FGameplayTagContainer CombinedTags;
        // Tags that I have in addition to my parent's tags
        public FGameplayTagContainer Added;
        //Tags that should be removed if my parent had them
        public FGameplayTagContainer Removed;
        public void AddTag(FGameplayTag TagToAdd)
        {
            //CombinedTags.AddTag(TagToAdd);
        }

        public void RemoveTag(FGameplayTag TagToRemove)
        {
            //CombinedTags.RemoveTag(TagToRemove);
        }
    }

}

