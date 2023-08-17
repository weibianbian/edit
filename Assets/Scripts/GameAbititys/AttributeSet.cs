using RailShootGame;

namespace GameplayAbilitySystem
{
    public class AttributeSet : ReferencePoolObject
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

