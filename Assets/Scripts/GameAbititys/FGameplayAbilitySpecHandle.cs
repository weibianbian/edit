namespace GameplayAbilitySystem
{
    //AbilitySystemComponent
    //              ---TryActivateAbility
    //GameplayAbility
    //              ---CanActivateAbility
    //GameplayAbility
    //              ---CallActivateAbility
    //GameplayAbility
    //              ---K2_ActivateAbility
    //GameplayAbility
    //              ---CommitAbility
    //执行蓝图AbilityTask
    //              ---PlayMontageTask-------Wati-------GameplayAbility(EndAbility)
    //执行事件（动画轴上的事件）
    //              ---SendGameplayEventToActor
    //GameplayAbility
    //              ---ApplyGameplayEffectToTarget
    public class FGameplayAbilitySpecHandle
    {

        int Handle;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(FGameplayAbilitySpecHandle a, FGameplayAbilitySpecHandle b)
        {
            return a.Handle == b.Handle;
        }
        public static bool operator !=(FGameplayAbilitySpecHandle a, FGameplayAbilitySpecHandle b)
        {
            return a.Handle != b.Handle;
        }

    }
}

