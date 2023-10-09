using RailShootGame;
using System.Diagnostics;

namespace GameplayAbilitySystem
{
    public class FGameplayCueNotify_DecalInfo
    {
        public bool SpawnDecal()
        {
            UnityEngine.Debug.Log("FGameplayCueNotify_DecalInfo.SpawnDecal");
            DecalComponent SpawnedDecalComponent = null;
            return false;
        }
        public DecalComponent SpawnDecalAtLocation()
        {
            return null;
        }
        private DecalComponent CreateDecalComponent(AActor Actor, float LifeSpan)
        {
            DecalComponent DecalComp = new DecalComponent();

            if (LifeSpan > 0.0f)
            {
                DecalComp.SetLifeSpan(LifeSpan);
            }
            return DecalComp;
        }


    }
}

