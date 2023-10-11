using RailShootGame;
using System;

namespace GameplayAbilitySystem
{
    public class FGameplayAbilityActivationInfo
    {

    }
    public class FGameplayAbilitySpec
    {
        public int id;
        public UGameplayAbility Ability;
        public int Level;
        public int InputID;
        public int ActiveCount = 0;
        public bool InputPressed;
        public FGameplayAbilitySpecHandle Handle;
        public FGameplayAbilityActivationInfo ActivationInfo;
        public static FGameplayAbilitySpec Default = new FGameplayAbilitySpec();
        //可选的被复制的能力标签。这些标签也被应用的游戏效果作为源标签捕获。
        public FGameplayTagContainer DynamicAbilityTags = new FGameplayTagContainer();
        public bool IsActive()
        {
            return Ability != null && ActiveCount > 0;
        }
        public FGameplayAbilitySpec()
        {
           
        }
        public FGameplayAbilitySpec(UGameplayAbility InAbility, int InLevel)
        {
            Ability = InAbility;
            Level = InLevel;

            Handle = new FGameplayAbilitySpecHandle();
            Handle.GenerateNewHandle();
        }
    }
}

