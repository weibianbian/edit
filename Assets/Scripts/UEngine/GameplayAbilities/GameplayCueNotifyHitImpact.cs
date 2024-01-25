using UEngine.GameFramework;
using UnityEngine;

namespace UEngine.GameplayAbilities
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

