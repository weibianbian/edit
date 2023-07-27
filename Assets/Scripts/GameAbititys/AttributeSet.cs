using RailShootGame;

namespace GameplayAbilitySystem
{
    public class AbilitySystemTestAttributeSe : AttributeSet
    {
        public GameplayAttributeData Health, MaxHealth = null;
        public GameplayAttributeData Mana, MaxMana = null;
    }
    public class AttributeSet
    {
        public virtual bool PreGameplayEffectExecute()
        {
            return true;
        }
        public virtual bool PostGameplayEffectExecute()
        {
            return true;
        }
    }
}

