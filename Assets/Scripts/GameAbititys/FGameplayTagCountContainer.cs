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
    }

}

