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

            //覆盖修饰符根本不关心堆栈计数所有其他修饰操作都需要减去它们的偏差值才能处理
            //正确堆叠
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

