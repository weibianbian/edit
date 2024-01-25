namespace UEngine.GameplayAbilities
{
    public class FAggregator
    {
        public float BaseValue = 0;
        public FAggregatorModChannelContainer ModChannels;
        public FAggregator(float InBaseValue = 0.0f)
        {
            BaseValue = InBaseValue;
            ModChannels = new FAggregatorModChannelContainer();
        }
        public float GetBaseValue()
        {
            return BaseValue;
        }
        public void SetBaseValue(float NewBaseValue)
        {
            BaseValue = NewBaseValue;
        }
        public static float StaticExecModOnBaseValue(float BaseValue, EGameplayModOp ModifierOp, float EvaluatedMagnitude)
        {
            switch (ModifierOp)
            {
                case EGameplayModOp.Override:
                    {
                        BaseValue = EvaluatedMagnitude;
                        break;
                    }
                case EGameplayModOp.Additive:
                    {
                        BaseValue += EvaluatedMagnitude;
                        break;
                    }
                case EGameplayModOp.Multiplicitive:
                    {
                        BaseValue *= EvaluatedMagnitude;
                        break;
                    }
                case EGameplayModOp.Division:
                    {
                        BaseValue /= EvaluatedMagnitude;
                        break;
                    }
            }

            return BaseValue;
        }
        public void UpdateAggregatorMod(FActiveGameplayEffectHandle ActiveHandle, FGameplayAttribute Attribute, FGameplayEffectSpec Spec, bool bWasLocallyGenerated, FActiveGameplayEffectHandle InHandle)
        {
            //删除mod，但不要将其标记为dirty，直到我们重新添加聚合器，我们这样做是为了让UAttributeSets统计只知道增量变化。
            ModChannels.RemoveAggregatorMod(ActiveHandle);

            for (int ModIdx = 0; ModIdx < Spec.Modifiers.Count; ++ModIdx)
            {
                FGameplayModifierInfo ModDef = Spec.Def.Modifiers[ModIdx];
                if (ModDef.Attribute == Attribute)
                {
                    //FAggregatorModChannel ModChannel = ModChannels.FindOrAddModChannel(ModDef.EvaluationChannelSettings.GetEvaluationChannel());
                    //ModChannel.AddMod(Spec.GetModifierMagnitude(ModIdx, true), ModDef.ModifierOp, ModDef.SourceTags, ModDef.TargetTags, bWasLocallyGenerated, InHandle);
                }
            }
        }
        //使用任意基值计算聚合器
        public float EvaluateWithBase(float InlineBaseValue, FAggregatorEvaluateParameters Parameters)
        {
            EvaluateQualificationForAllMods(Parameters);
            return ModChannels.EvaluateWithBase(InlineBaseValue, Parameters);
        }
        //在每个mod上调用::updatequaliqs。当您需要手动检查聚合器时非常有用
        public void EvaluateQualificationForAllMods(FAggregatorEvaluateParameters Parameters)
        {
            // First run our "Default" qualifies function
            //ModChannels.EvaluateQualificationForAllMods(Parameters);

            // Then run custom func
            //if (EvaluationMetaData && EvaluationMetaData->CustomQualifiesFunc)
            //{
            //    EvaluationMetaData->CustomQualifiesFunc(Parameters, this);
            //}
        }
    }
}

