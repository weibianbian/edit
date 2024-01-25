using RailShootGame;
using UEngine.GameplayTasks;

namespace UEngine.GameplayAbilities
{
    public class UAbilityTask_StartAbilityState : UAbilityTask
    {
        public delegate void FAbilityStateDelegate();

        /** Invoked if the ability was interrupted and this state is active. */
        FAbilityStateDelegate OnStateEnded;
        FAbilityStateDelegate OnStateInterrupted;
        bool bWasEnded;
        bool bWasInterrupted;
        bool bEndCurrentState;
        public static UAbilityTask_StartAbilityState StartAbilityState(UGameplayAbility OwningAbility, string StateName, bool bEndCurrentState)
        {
            UAbilityTask_StartAbilityState Task = NewAbilityTask<UAbilityTask_StartAbilityState>(OwningAbility, StateName);
            Task.bEndCurrentState = bEndCurrentState;
            return Task;
        }
        public override void Activate()
        {
            if (Ability != null)
            {
                if (bEndCurrentState)
                {
                    Ability.OnGameplayAbilityStateEnded.Invoke(null);
                }
                Ability.OnGameplayAbilityStateEnded += OnEndState;
                Ability.OnGameplayAbilityCancelled += OnInterruptState;
            }
        }
        public override void OnDestroy(bool bInOwnerFinished)
        {
            if (Ability != null)
            {
                Ability.OnGameplayAbilityStateEnded -= OnEndState;
                Ability.OnGameplayAbilityCancelled -= (OnInterruptState);
            }
            if (bWasInterrupted)
            {
                //if (ShouldBroadcastAbilityTaskDelegates())
                {
                    OnStateInterrupted.Invoke();
                }
            }
            //else if ((bWasEnded || AbilityEnded) && OnStateEnded != null)
            //{
            //    if (ShouldBroadcastAbilityTaskDelegates())
            //    {
            //        OnStateEnded.Broadcast();
            //    }
            //}
            base.OnDestroy(bInOwnerFinished);
        }
        public void OnEndState(string StateNameToEnd)
        {
            // All states end if 'NAME_None' is passed to this delegate
            if (string.IsNullOrEmpty(StateNameToEnd) || StateNameToEnd == InstanceName)
            {
                bWasEnded = true;

                EndTask();
            }
        }
        public void OnInterruptState()
        {
            bWasInterrupted = true;
        }
    }
}

