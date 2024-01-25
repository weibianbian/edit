namespace UEngine.GameplayAbilities
{
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
}

