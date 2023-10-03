using RailShootGame;

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
        public bool InputPressed;
        public FGameplayAbilitySpecHandle Handle;
        public FGameplayAbilityActivationInfo ActivationInfo;
        public static FGameplayAbilitySpec Default=new FGameplayAbilitySpec();
        //可选的被复制的能力标签。这些标签也被应用的游戏效果作为源标签捕获。
        public FGameplayTagContainer DynamicAbilityTags;
        public bool IsActive()
        {
            return Ability != null;
        }
    }
}

