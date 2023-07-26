namespace GameplayAbilitySystem
{
    public class AbilitySystemTestAttributeSe : AttributeSet
    {
        public float Health, MaxHealth = 100f;
        public float Mana, MaxMana = 100f;
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

