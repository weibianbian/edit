using System.Collections.Generic;
using System.Linq;
namespace UEngine.GameplayAbilities
{
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
        public float ReverseEvaluate(float FinalValue, FAggregatorEvaluateParameters Parameters)
        {

            float ComputedValue = FinalValue;

            // TMap API不允许反向迭代，所以需要请求键数组，然后
            //反过来遍历它
            List<EGameplayModEvaluationChannel> ChannelArray = new List<EGameplayModEvaluationChannel>();
            ChannelArray = ModChannelsMap.Keys.ToList();

            //for (int ModChannelIdx = ChannelArray.Count - 1; ModChannelIdx >= 0; --ModChannelIdx)
            //{
            //    FAggregatorModChannel Channel = ModChannelsMap[(ChannelArray[ModChannelIdx])];
            //    if (!Channel.ReverseEvaluate(ComputedValue, Parameters, ComputedValue))
            //    {
            //        ComputedValue = FinalValue;
            //        break;
            //    }
            //}

            return ComputedValue;
        }
        public float EvaluateWithBase(float InlineBaseValue, FAggregatorEvaluateParameters Parameters)
        {
            float ComputedValue = InlineBaseValue;

            return ComputedValue;
        }
    }
}

