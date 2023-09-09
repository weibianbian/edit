using GameplayAbilitySystem;
using JetBrains.Annotations;
using RailShootGame;
using Sirenix.Utilities.Editor;
using System;
using System.Reflection;
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
        SourceComponent.GetSet<AbilitySystemTestAttributeSet>().Health = new GameplayAttributeData(StartingHealth);
        SourceComponent.GetSet<AbilitySystemTestAttributeSet>().MaxHealth = new GameplayAttributeData(StartingHealth);
        SourceComponent.GetSet<AbilitySystemTestAttributeSet>().Mana = new GameplayAttributeData(StartingMana);
        SourceComponent.GetSet<AbilitySystemTestAttributeSet>().MaxMana = new GameplayAttributeData(StartingMana);

        DestActor = World.SpawnActor<AbilitySystemTestActor>();
        DestComponent = DestActor.GetAbilitySystemComponent();
        DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health = new GameplayAttributeData(StartingHealth);
        DestComponent.GetSet<AbilitySystemTestAttributeSet>().MaxHealth = new GameplayAttributeData(StartingHealth);
        DestComponent.GetSet<AbilitySystemTestAttributeSet>().Mana = new GameplayAttributeData(StartingMana);
        DestComponent.GetSet<AbilitySystemTestAttributeSet>().MaxMana = new GameplayAttributeData(StartingMana);
        //Test_InstantDamage();
        //Test_InstantDamageRemap();
        Test_PeriodicDamage();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Test_InstantDamage()
    {
        float DamageValue = 5.0f;
        float StartingHealth = DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health.CurrentValue;
        GameplayEffect BaseDmgEffect = new GameplayEffect();
        AddModifier(BaseDmgEffect, typeof(AbilitySystemTestAttributeSet).GetField("Health"), typeof(AbilitySystemTestAttributeSet), EGameplayModOp.Additive, new FScalableFloat(-DamageValue));
        BaseDmgEffect.DurationPolicy = EGameplayEffectDurationType.Instant;
        SourceComponent.ApplyGameplayEffectToTarget(BaseDmgEffect, DestComponent, 1);

        Debug.Log($"Health Reduced   {DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health.CurrentValue}={StartingHealth - DamageValue}");
    }
    public void Test_InstantDamageRemap()
    {
        float DamageValue = 5.0f;
        float StartingHealth = DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health.CurrentValue;
        GameplayEffect BaseDmgEffect = new GameplayEffect();
        AddModifier(BaseDmgEffect, typeof(AbilitySystemTestAttributeSet).GetField("Damage"), typeof(AbilitySystemTestAttributeSet), EGameplayModOp.Additive, new FScalableFloat(DamageValue));
        BaseDmgEffect.DurationPolicy = EGameplayEffectDurationType.Instant;
        SourceComponent.ApplyGameplayEffectToTarget(BaseDmgEffect, DestComponent, 1);

        Debug.Log($"Health Reduced   {DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health.CurrentValue}={StartingHealth - DamageValue}");
        Debug.Log($"Damage Applied   {DestComponent.GetSet<AbilitySystemTestAttributeSet>().Damage.CurrentValue}={0}");
    }
    public void Test_ManaBuff()
    {
        float BuffValue = 30.0f;
        float StartingMana = DestComponent.GetSet<AbilitySystemTestAttributeSet>().Mana.CurrentValue;
        FActiveGameplayEffectHandle BuffHandle;
        GameplayEffect DamageBuffEffect = new GameplayEffect();
        DamageBuffEffect.DurationPolicy = EGameplayEffectDurationType.Infinite;

        BuffHandle = SourceComponent.ApplyGameplayEffectToTarget(DamageBuffEffect, DestComponent, 1.0f);

        Debug.Log($"Mana Buffed   {DestComponent.GetSet<AbilitySystemTestAttributeSet>().Mana.CurrentValue}={StartingMana - BuffValue}");

        DestComponent.RemoveActiveGameplayEffect(BuffHandle);

        Debug.Log($"Mana Restored   {DestComponent.GetSet<AbilitySystemTestAttributeSet>().Mana.CurrentValue}={StartingMana}");
    }
    public void Test_PeriodicDamage()
    {
        int NumPeriods = 10;
        float PeriodSecs = 1.0f;
        float DamagePerPeriod = 5.0f;
        float StartingHealth = DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health.CurrentValue;
        GameplayEffect BaseDmgEffect = new GameplayEffect();
        AddModifier(BaseDmgEffect, typeof(AbilitySystemTestAttributeSet).GetField("Health"), typeof(AbilitySystemTestAttributeSet), EGameplayModOp.Additive, new FScalableFloat(-DamagePerPeriod));
        BaseDmgEffect.DurationPolicy = EGameplayEffectDurationType.HasDuration;
        BaseDmgEffect.DurationMagnitude = new FGameplayEffectModifierMagnitude(new FScalableFloat(NumPeriods * PeriodSecs));
        BaseDmgEffect.Period.Value = PeriodSecs;

        SourceComponent.ApplyGameplayEffectToTarget(BaseDmgEffect, DestComponent, 1.0f);

        int NumApplications = 0;

        TickWorld(PeriodSecs * 0.1f);

        for (int i = 0; i < NumPeriods; ++i)
        {
            // advance time by one period
            TickWorld(PeriodSecs);

            ++NumApplications;

            // check that health has been reduced
            Debug.Log($"DestComponent->GetSet<UAbilitySystemTestAttributeSet>()->Health={DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health.CurrentValue} " +
                $"   ={StartingHealth - (DamagePerPeriod * NumApplications)}");
            TickWorld(PeriodSecs);
        }
    }
    public void AddModifier(GameplayEffect Effect, FieldInfo Property, Type PropOwner, EGameplayModOp Op, FScalableFloat Magnitude)
    {
        FGameplayModifierInfo Info = new FGameplayModifierInfo();
        Effect.Modifiers.Add(Info);
        Info.ModifierOp = Op;
        Info.ModifierMagnitude = Magnitude;
        Info.Attribute.SetUProperty(Property, PropOwner);
    }
    public void TickWorld(float InTime)
    {
        {
            World.Tick(InTime);
        }
    }
}
