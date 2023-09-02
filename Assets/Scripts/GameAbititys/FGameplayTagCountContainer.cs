using RailShootGame;
using System;
using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class FGameplayTagCountContainer
    {
        public class DelegateInfo
        {
            public Action<GameplayTag, int> OnNewOrRemove;
            public Action<GameplayTag, int> OnAnyChange;
        }
        public Dictionary<GameplayTag, DelegateInfo> GameplayTagEventMap = new Dictionary<GameplayTag, DelegateInfo>();
        public FGameplayTagContainer ExplicitTags;
        public Action<GameplayTag, int> RegisterGameplayTagEvent(GameplayTag Tag, EGameplayTagEventType EventType)
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

