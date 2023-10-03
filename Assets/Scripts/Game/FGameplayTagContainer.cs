using System.Collections.Generic;

namespace RailShootGame
{
    public class FGameplayTagContainer
    {
        public List<FGameplayTag> GameplayTags = new List<FGameplayTag>();
        public List<FGameplayTag> ParentTags = new List<FGameplayTag>();

        public void Reset()
        {
            GameplayTags.Clear();
            ParentTags.Clear();
        }
        public int Num()
        {
            return GameplayTags.Count;
        }
        public bool HasAll(FGameplayTagContainer ContainerToCheck)
        {
            foreach (var OtherTag in ContainerToCheck.GameplayTags)
            {
                if (!GameplayTags.Contains(OtherTag) && !ParentTags.Contains(OtherTag))
                {
                    return false;
                }
            }
            return true;
        }
        public bool HasAny(FGameplayTagContainer ContainerToCheck)
        {
            foreach (var OtherTag in ContainerToCheck.GameplayTags)
            {
                if (GameplayTags.Contains(OtherTag) || ParentTags.Contains(OtherTag))
                {
                    return true;
                }
            }
            return false;
        }
        public bool HasTagExact(FGameplayTag TagToCheck)
        {
            return GameplayTags.Contains(TagToCheck);
        }

        public void AppendTags(FGameplayTagContainer Other)
        {
            GameplayTags.AddRange(Other.GameplayTags);
            ParentTags.AddRange(Other.ParentTags);
        }

    }
}

