namespace GameplayAbilitySystem
{
    public class GameplayCueNotifyHitImpact : GameplayCueNotifyStatic
    {
        public override bool HandlesEvent(EGameplayCueEvent EventType)
        {
            return EventType == EGameplayCueEvent.Executed;
        }
        public override void HandleGameplayCue(EGameplayCueEvent EventType)
        {
            if (EventType != EGameplayCueEvent.Executed)
            {
                return;
            }

        }
    }
}

