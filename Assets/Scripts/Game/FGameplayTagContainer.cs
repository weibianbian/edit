using System.Collections.Generic;

namespace RailShootGame
{
    public class FGameplayTagContainer
    {
        public List<GameplayTag> GameplayTags = new List<GameplayTag>();
        public List<GameplayTag> ParentTags = new List<GameplayTag>();

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
        public void AppendTags(FGameplayTagContainer Other)
        {
            GameplayTags.AddRange(Other.GameplayTags);
            ParentTags.AddRange(Other.ParentTags);
        }
        
    }
}

