using RailShootGame;
using System;
using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class FGameplayTagCountContainer
    {
        public class DelegateInfo
        {
            public Action<FGameplayTag, int> OnNewOrRemove;
            public Action<FGameplayTag, int> OnAnyChange;
        }
        public Dictionary<FGameplayTag, DelegateInfo> GameplayTagEventMap = new Dictionary<FGameplayTag, DelegateInfo>();
        public Dictionary<FGameplayTag, int> GameplayTagCountMap = new Dictionary<FGameplayTag, int>();
        public FGameplayTagContainer ExplicitTags;
        public FGameplayTagCountContainer()
        {
            ExplicitTags = new FGameplayTagContainer();
        }
        public Action<FGameplayTag, int> RegisterGameplayTagEvent(FGameplayTag Tag, EGameplayTagEventType EventType)
        {
            if (!GameplayTagEventMap.TryGetValue(Tag, out DelegateInfo Info))
            {
                Info = new DelegateInfo();
                GameplayTagEventMap.Add(Tag, Info);
            }
            if (EventType == EGameplayTagEventType.NewOrRemoved)
            {
                return Info.OnNewOrRemove;
            }
            return Info.OnAnyChange;
        }
        public FGameplayTagContainer GetExplicitGameplayTags()
        {
            return ExplicitTags;
        }
        public bool HasAnyMatchingGameplayTags(FGameplayTagContainer TagContainer)
        {
            if (TagContainer.Num() == 0)
            {
                return false;
            }

            bool AnyMatch = false;
            for (int i = 0; i < TagContainer.Num(); i++)
            {
                FGameplayTag Tag = TagContainer.GameplayTags[i];
                if (GameplayTagCountMap.TryGetValue(Tag, out int FindRef) && FindRef > 0)
                {
                    AnyMatch = true;
                }
            }
            return AnyMatch;
        }
    }

}

