namespace GameplayAbilitySystem
{
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

