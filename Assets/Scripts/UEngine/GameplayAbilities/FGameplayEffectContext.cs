using UEngine.GameFramework;

namespace UEngine.GameplayAbilities
{
    public class FGameplayEffectContext
    {
        public int AbilityLevel;
        public AActor Instigator;
        public AActor EffectCauser;
        public UAbilitySystemComponent InstigatorAbilitySystemComponent;
        public void AddInstigator(AActor InInstigator, AActor InEffectCauser)
        {
            Instigator = InInstigator;
            EffectCauser = InEffectCauser;
        }
        public UAbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return InstigatorAbilitySystemComponent;
        }
        public void SetAbility(UGameplayAbility InGameplayAbility)
        {
            if (InGameplayAbility != null)
            {
                AbilityLevel = InGameplayAbility.GetAbilityLevel();
            }
        }
    }
}

