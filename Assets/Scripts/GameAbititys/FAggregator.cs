using System.Collections.Generic;
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
    public class FAggregatorModChannelContainer
    {
        Dictionary<EGameplayModEvaluationChannel, FAggregatorModChannel> ModChannelsMap = new Dictionary<EGameplayModEvaluationChannel, FAggregatorModChannel>();
        public void RemoveAggregatorMod(FActiveGameplayEffectHandle ActiveHandle)
        {
            //if (ActiveHandle.IsValid())
            {
                foreach (var ChannelEntry in ModChannelsMap)
                {
                    FAggregatorModChannel CurChannel = ChannelEntry.Value;
                    CurChannel.RemoveModsWithActiveHandle(ActiveHandle);
                }
            }
        }
        public FAggregatorModChannel FindOrAddModChannel(EGameplayModEvaluationChannel Channel)
        {
            if (!ModChannelsMap.TryGetValue(Channel, out FAggregatorModChannel FoundChannel))
            {
                //添加新通道时，需要借助于映射来保存键序进行评估
                FoundChannel = new FAggregatorModChannel();
                ModChannelsMap.Add(Channel, FoundChannel);
            }
            return FoundChannel;
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

