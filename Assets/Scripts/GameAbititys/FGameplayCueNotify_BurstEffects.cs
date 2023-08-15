namespace GameplayAbilitySystem
{
    public class FGameplayCueNotify_BurstEffects
    {
        public FGameplayCueNotify_DecalInfo BurstDecal=new FGameplayCueNotify_DecalInfo();

        public void ExecuteEffects()
        {
            BurstDecal.SpawnDecal();
        }
    }
}

