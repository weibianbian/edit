using UEngine.GameFramework;

namespace UEngine.GameplayAbilities
{
    public class FGameplayEffectContextHandle
    {
        public FGameplayEffectContext Data;
        public FGameplayEffectContextHandle(FGameplayEffectContext InData)
        {
            Data = InData;
        }
        public void AddInstigator(AActor InInstigator, AActor InEffectCauser)
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

