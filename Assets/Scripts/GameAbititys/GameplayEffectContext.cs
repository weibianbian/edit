using RailShootGame;

namespace GameplayAbilitySystem
{
    public class GameplayEffectContext
    {
        public Actor Instigator;
        public Actor EffectCauser;
        public UAbilitySystemComponent InstigatorAbilitySystemComponent;
        public void AddInstigator(Actor InInstigator, Actor InEffectCauser)
        {
            Instigator = InInstigator;
            EffectCauser = InEffectCauser;
        }
        public UAbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return InstigatorAbilitySystemComponent;
        }
    }
}

