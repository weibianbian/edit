using RailShootGame;

namespace GameplayAbilitySystem
{
    public class AbilitySystemTestAttributeSet : AttributeSet
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

