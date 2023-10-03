using RailShootGame;

namespace GameplayAbilitySystem
{
    public class FGameplayEffectContextHandle
    {
        public FGameplayEffectContext Data;
        public FGameplayEffectContextHandle(FGameplayEffectContext InData)
        {
            Data = InData;
        }
        public void AddInstigator(Actor InInstigator, Actor InEffectCauser)
        {
            Data.AddInstigator(InInstigator, InEffectCauser);
        }
        public UAbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return Data.GetInstigatorAbilitySystemComponent();
        }
        public void SetAbility(UGameplayAbility InGameplayAbility)
        {
            Data.SetAbility(InGameplayAbility);
        }
    }
}

