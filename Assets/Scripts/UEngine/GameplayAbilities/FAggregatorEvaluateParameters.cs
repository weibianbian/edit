using UEngine.GameplayTags;

namespace UEngine.GameplayAbilities
{
    //从调用者/游戏代码传递的聚合器评估中使用的数据
    public class FAggregatorEvaluateParameters
    {
        public FGameplayTagContainer SourceTags;
        public FGameplayTagContainer TargetTags;

        public FAggregatorEvaluateParameters()
        {
            SourceTags = new FGameplayTagContainer();
            TargetTags = new FGameplayTagContainer();
        }
    }

}

