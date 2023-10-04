using RailShootGame;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class GameplayCueNotifyHitImpact : GameplayCueNotifyStatic
    {
        public override bool HandlesEvent(EGameplayCueEvent EventType)
        {
            return EventType == EGameplayCueEvent.Executed;
        }
        public override void HandleGameplayCue(AActor TargetActor, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {
            Debug.Log("GameplayCueNotifyHitImpact.HandleGameplayCue");
            if (EventType != EGameplayCueEvent.Executed)
            {
                return;
            }
        }
    }
}

