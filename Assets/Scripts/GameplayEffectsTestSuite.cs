using RailShootGame;
using System;
using System.Reflection;
using UEngine;
using UEngine.GameplayAbilities;
using UnityEngine;

public class GameplayEffectsTestSuite : MonoBehaviour
{
    public UWorld World;
    public AbilitySystemTestActor SourceActor;
    public AbilitySystemTestActor DestActor;
    public UAbilitySystemComponent SourceComponent;
    public UAbilitySystemComponent DestComponent;
    // Start is called before the first frame update
    void Start()
    {
        World = new UWorld();
        ULevel level = new ULevel();
        World.AddToWorld(level);
        World.CurrentLevel = level;

        float StartingHealth = 100.0f;
        float StartingMana = 200.0f;

        SourceActor = World.SpawnActor<AbilitySystemTestActor>();
        SourceComponent = SourceActor.GetAbilitySystemComponent();
        SourceComponent.GetSet<UAbilitySystemTestAttributeSet>().Health = new FGameplayAttributeData(StartingHealth);
        SourceComponent.GetSet<UAbilitySystemTestAttributeSet>().MaxHealth = new FGameplayAttributeData(StartingHealth);
        SourceComponent.GetSet<UAbilitySystemTestAttributeSet>().Mana = new FGameplayAttributeData(StartingMana);
        SourceComponent.GetSet<UAbilitySystemTestAttributeSet>().MaxMana = new FGameplayAttributeData(StartingMana);

        DestActor = World.SpawnActor<AbilitySystemTestActor>();
        DestComponent = DestActor.GetAbilitySystemComponent();
        DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Health = new FGameplayAttributeData(StartingHealth);
        DestComponent.GetSet<UAbilitySystemTestAttributeSet>().MaxHealth = new FGameplayAttributeData(StartingHealth);
        DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Mana = new FGameplayAttributeData(StartingMana);
        DestComponent.GetSet<UAbilitySystemTestAttributeSet>().MaxMana = new FGameplayAttributeData(StartingMana);
        //Test_InstantDamage();
        //Test_InstantDamageRemap();
        Test_PeriodicDamage();
    }
    public void Test_InstantDamage()
    {
        float DamageValue = 5.0f;
        float StartingHealth = DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Health.CurrentValue;
        UGameplayEffect BaseDmgEffect = new UGameplayEffect();
        AddModifier(BaseDmgEffect, typeof(UAbilitySystemTestAttributeSet).GetField("Health"), typeof(UAbilitySystemTestAttributeSet), EGameplayModOp.Additive, new FScalableFloat(-DamageValue));
        BaseDmgEffect.DurationPolicy = EGameplayEffectDurationType.Instant;
        SourceComponent.ApplyGameplayEffectToTarget(BaseDmgEffect, DestComponent, 1);

        Debug.Log($"Health Reduced   {DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Health.CurrentValue}={StartingHealth - DamageValue}");
    }
    public void Test_InstantDamageRemap()
    {
        float DamageValue = 5.0f;
        float StartingHealth = DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Health.CurrentValue;
        UGameplayEffect BaseDmgEffect = new UGameplayEffect();
        AddModifier(BaseDmgEffect, typeof(UAbilitySystemTestAttributeSet).GetField("Damage"), typeof(UAbilitySystemTestAttributeSet), EGameplayModOp.Additive, new FScalableFloat(DamageValue));
        BaseDmgEffect.DurationPolicy = EGameplayEffectDurationType.Instant;
        SourceComponent.ApplyGameplayEffectToTarget(BaseDmgEffect, DestComponent, 1);

        Debug.Log($"Health Reduced   {DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Health.CurrentValue}={StartingHealth - DamageValue}");
        Debug.Log($"Damage Applied   {DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Damage.CurrentValue}={0}");
    }
    public void Test_ManaBuff()
    {
        float BuffValue = 30.0f;
        float StartingMana = DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Mana.CurrentValue;
        FActiveGameplayEffectHandle BuffHandle;
        UGameplayEffect DamageBuffEffect = new UGameplayEffect();
        DamageBuffEffect.DurationPolicy = EGameplayEffectDurationType.Infinite;

        BuffHandle = SourceComponent.ApplyGameplayEffectToTarget(DamageBuffEffect, DestComponent, 1.0f);

        Debug.Log($"Mana Buffed   {DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Mana.CurrentValue}={StartingMana - BuffValue}");

        DestComponent.RemoveActiveGameplayEffect(BuffHandle);

        Debug.Log($"Mana Restored   {DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Mana.CurrentValue}={StartingMana}");
    }
    public void Test_PeriodicDamage()
    {
        int NumPeriods = 10;
        float PeriodSecs = 1.0f;
        float DamagePerPeriod = 5.0f;
        float StartingHealth = DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Health.CurrentValue;
        UGameplayEffect BaseDmgEffect = new UGameplayEffect();
        AddModifier(BaseDmgEffect, typeof(UAbilitySystemTestAttributeSet).GetField("Health"), typeof(UAbilitySystemTestAttributeSet), EGameplayModOp.Additive, new FScalableFloat(-DamagePerPeriod));
        BaseDmgEffect.DurationPolicy = EGameplayEffectDurationType.HasDuration;
        BaseDmgEffect.DurationMagnitude = new FGameplayEffectModifierMagnitude(new FScalableFloat(NumPeriods * PeriodSecs));
        BaseDmgEffect.Period.Value = PeriodSecs;

        SourceComponent.ApplyGameplayEffectToTarget(BaseDmgEffect, DestComponent, 1.0f);

        int NumApplications = 0;

        TickWorld(PeriodSecs * 0.1f);

        for (int i = 0; i > NumPeriods; ++i)
        {
            // advance time by one period
            TickWorld(PeriodSecs);

            ++NumApplications;

            // check that health has been reduced
            Debug.Log($"DestComponent->GetSet<UAbilitySystemTestAttributeSet>()->Health={DestComponent.GetSet<UAbilitySystemTestAttributeSet>().Health.BaseValue} " +
                $"   ={StartingHealth - (DamagePerPeriod * NumApplications)}");
        }
        TickWorld(PeriodSecs);
    }
    public void AddModifier(UGameplayEffect Effect, FieldInfo Property, Type PropOwner, EGameplayModOp Op, FScalableFloat Magnitude)
    {
        FGameplayModifierInfo Info = new FGameplayModifierInfo();
        Effect.Modifiers.Add(Info);
        Info.ModifierOp = Op;
        Info.ModifierMagnitude = Magnitude;
        Info.Attribute.SetUProperty(Property, PropOwner);
    }
    public void Update()
    {
        TickWorld(Time.deltaTime);
    }
    public void TickWorld(float InTime)
    {
        {
            World.Tick(InTime);
        }
    }
}
