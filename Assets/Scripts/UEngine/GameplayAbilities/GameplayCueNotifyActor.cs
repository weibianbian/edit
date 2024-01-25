using RailShootGame;
using UEngine.GameFramework;
using UEngine.GameplayTags;

namespace UEngine.GameplayAbilities
{

    public class GameplayCueNotifyActor : AActor
    {
        // 处理玩法提示事件
        public virtual void HandleGameplayCue()
        {

        }
        // 当一个玩法提示执行时被调用，这用于即时效果或周期性tick
        public void OnExecute()
        {

        }
        // 当具有持续时间的玩法提示首次激活时调用，只有在客户端见证激活时才会调用
        public void OnActive()
        {

        }
        // 当具有持续时间的玩法提示首次被视为激活时调用，即使它实际上并没有被应用（正在进行加入等）
        public void WhileActive()
        {

        }
        // 当具有持续时间的玩法提示被移除时调用
        public void OnRemove()
        {

        }
        // 该通知实例被激活的对应标签
        FGameplayTag GameplayCueTag;
    }
}

