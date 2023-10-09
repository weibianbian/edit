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
        public void AddTag(FGameplayTag TagToAdd)
        {
            if (TagToAdd.IsValid())
            {
                // Don't want duplicate tags
                GameplayTags.Add(TagToAdd);

                AddParentsForTag(TagToAdd);
            }
        }
        public void AddParentsForTag(FGameplayTag Tag)
        {
            FGameplayTagContainer SingleContainer = UGameplayTagsManager.Get().GetSingleTagContainer(Tag);
            if (SingleContainer != null)
            {
                // Add Parent tags from this tag to our own
                for (int i = 0; i < SingleContainer.ParentTags.Count; i++)
                {
                    FGameplayTag ParentTag = SingleContainer.ParentTags[i];
                    ParentTags.Add(ParentTag);
                }
            }
        }
    }
}

