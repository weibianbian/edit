using UEngine.GameplayTasks;

namespace UEngine.GameplayAbilities
{
    public class UAbilityTask_WaitInputRelease : UAbilityTask
    {
        public delegate void FInputReleaseDelegate();
        public delegate void FDelegateHandle();

        public float StartTime;
        public bool bTestInitialState;
        public FDelegateHandle DelegateHandle;
        public FInputReleaseDelegate OnRelease;

        public UAbilityTask_WaitInputRelease()
        {
            StartTime = 0.0f;
            bTestInitialState = false;
        }
        public static UAbilityTask_WaitInputRelease WaitInputRelease(UGameplayAbility OwningAbility, bool bTestAlreadyReleased = false)
        {
            UAbilityTask_WaitInputRelease Task = NewAbilityTask<UAbilityTask_WaitInputRelease>(OwningAbility);
            Task.bTestInitialState = bTestAlreadyReleased;
            return Task;
        }
        public void OnReleaseCallback()
        {
            UAbilitySystemComponent ASC = AbilitySystemComponent;
            if (Ability == null || ASC == null)
            {
                return;
            }
            OnRelease.Invoke();
            EndTask();
        }
        public override void Activate()
        {
            UAbilitySystemComponent ASC = AbilitySystemComponent;
            if (ASC != null && Ability != null)
            {
                if (bTestInitialState)
                {
                    FGameplayAbilitySpec Spec = Ability.GetCurrentAbilitySpec();
                    if (Spec != null && !Spec.InputPressed)
                    {
                        OnReleaseCallback();
                        return;
                    }
                }

                //DelegateHandle = ASC.AbilityReplicatedEventDelegate(EAbilityGenericReplicatedEvent::InputReleased, GetAbilitySpecHandle(), GetActivationPredictionKey()).AddUObject(this, &UAbilityTask_WaitInputRelease::OnReleaseCallback);
            }
        }
    }
}

