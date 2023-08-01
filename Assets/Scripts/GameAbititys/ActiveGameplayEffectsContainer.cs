using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class ActiveGameplayEffectsContainer
    {
        AbilitySystemComponent owner;
        public List<GameplayAbilitySpec> GameplayEffects_Internal;
        public Dictionary<GameplayAttribute, OnGameplayAttributeValueChange> AttributeValueChangeDelegates;

        public ActiveGameplayEffect ApplyGameplayEffectSpec(GameplayEffectSpec Spec)
        {
            ActiveGameplayEffect AppliedActiveGE = null;
            ActiveGameplayEffectHandle NewHandle = ActiveGameplayEffectHandle.GenerateNewHandle(owner);

            AppliedActiveGE = new ActiveGameplayEffect()
            {
                Handle = NewHandle,
                Spec = Spec,
            };
            return new ActiveGameplayEffect();
        }
        public OnGameplayAttributeValueChange GetGameplayAttributeValueChangeDelegate(GameplayAttribute Attribute)
        {
            if (!AttributeValueChangeDelegates.TryGetValue(Attribute, out OnGameplayAttributeValueChange value))
            {
                value = new OnGameplayAttributeValueChange();
                AttributeValueChangeDelegates.Add(Attribute, value);
            }
            return value;
        }
    }

}

