using RailShootGame;
using static UnityEngine.UI.GridLayoutGroup;
using System.Numerics;

namespace GameplayAbilitySystem
{

    public class GameplayCueNotifyActor : Actor
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
        GameplayTag GameplayCueTag;
    }
    public class FGameplayCueNotify_BurstEffects
    {
        public FGameplayCueNotify_DecalInfo BurstDecal;

        public void ExecuteEffects()
        {
            BurstDecal.SpawnDecal();
        }
    }
    public class FGameplayCueNotify_SpawnResult
    {
        
    }
    public class FGameplayCueNotify_DecalInfo
    {
        public bool SpawnDecal()
        {
            DecalComponent SpawnedDecalComponent = null;
            return false;
        }
        public DecalComponent SpawnDecalAtLocation()
        {
            return null;
        }
        private DecalComponent CreateDecalComponent(Actor Actor, float LifeSpan)
        {
            DecalComponent DecalComp = new DecalComponent(Actor);

            if (LifeSpan > 0.0f)
            {
                DecalComp.SetLifeSpan(LifeSpan);
            }
            return DecalComp;
        }


    }
}

