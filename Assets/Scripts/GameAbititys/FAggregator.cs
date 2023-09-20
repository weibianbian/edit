using System.Security.Cryptography;
using Unity.Collections;
namespace GameplayAbilitySystem
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
    public class FAggregatorMod
    {
        public FGameplayTagRequirements SourceTagReqs;
        public FGameplayTagRequirements TargetTagReqs;

        public float EvaluatedMagnitude;       // Magnitude this mod was last evaluated at
        public float StackCount;

        public FActiveGameplayEffectHandle ActiveHandle;   // Handle of the active GameplayEffect we are tied to (if any)
        public bool IsPredicted;
    }
    public class FAggregatorModChannel
    {
        public FAggregatorMod[] Mods = new FAggregatorMod[(int)EGameplayModOp.Max];
        public void RemoveModsWithActiveHandle(FActiveGameplayEffectHandle Handle)
        {
            for (int ModOpIdx = 0; ModOpIdx < Mods.Length; ++ModOpIdx)
            {
                //              Mods[ModOpIdx].RemoveAllSwap([Handle](FAggregatorMod Element)

                //      {
                //                  return (Element.ActiveHandle == Handle);
                //              }, 
                //false);
            }
        }
        public bool ReverseEvaluate(float FinalValue, FAggregatorEvaluateParameters Parameters, out float ComputedValue)
        {
            ComputedValue = 0;
            return true;
        }

        void AddMod(float EvaluatedMagnitude, EGameplayModOp ModOp, FGameplayTagRequirements SourceTagReqs, FGameplayTagRequirements TargetTagReqs, bool bIsPredicted, FActiveGameplayEffectHandle ActiveHandle)
        {
            FAggregatorMod[] ModList = Mods;

            //int NewIdx = ModList.AddUninitialized();
            int NewIdx = 0;
            FAggregatorMod NewMod = ModList[NewIdx];

            NewMod.SourceTagReqs = SourceTagReqs;
            NewMod.TargetTagReqs = TargetTagReqs;
            NewMod.EvaluatedMagnitude = EvaluatedMagnitude;
            NewMod.StackCount = 0;
            NewMod.ActiveHandle = ActiveHandle;
            NewMod.IsPredicted = bIsPredicted;
        }
    }
    public enum EGameplayModEvaluationChannel
    {
        Channel0,
        Channel1,
        Channel2,
        Channel3,
        Channel4,
        Channel5,
        Channel6,
        Channel7,
        Channel8,
        Channel9,

        // Always keep last
        Channel_MAX
    }
}

