using System;

namespace GameplayAbilitySystem
{
    public static class GameplayEffectUtilities
    {
        public static readonly float[] ModifierOpBiases = new float[(int)EGameplayModOp.Max] { 0.0f, 1.0f, 1.0f, 0.0f };
        public static float ComputeStackedModifierMagnitude(float BaseComputedMagnitude, int StackCount, EGameplayModOp ModOp)
        {
            float OperationBias = GameplayEffectUtilities.GetModifierBiasByModifierOp(ModOp);

            StackCount = Math.Clamp(StackCount, 0, StackCount);

            float StackMag = BaseComputedMagnitude;

            // Override modifiers don't care about stack count at all. All other modifier ops need to subtract out their bias value in order to handle
            // stacking correctly
            if (ModOp != EGameplayModOp.Override)
            {
                StackMag -= OperationBias;
                StackMag *= StackCount;
                StackMag += OperationBias;
            }

            return StackMag;
        }
        public static float GetModifierBiasByModifierOp(EGameplayModOp ModOp)
        {
            return ModifierOpBiases[(int)ModOp];
        }
    }
}

