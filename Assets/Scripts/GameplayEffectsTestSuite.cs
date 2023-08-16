using GameplayAbilitySystem;
using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayEffectsTestSuite : MonoBehaviour
{
    public UWorld World;
    public AbilitySystemTestActor SourceActor;
    public AbilitySystemTestActor DestActor;
    public AbilitySystemComponent SourceComponent;
    public AbilitySystemComponent DestComponent;
    // Start is called before the first frame update
    void Start()
    {
        SourceActor = World.SpawnActor<AbilitySystemTestActor>();
        SourceComponent = SourceActor.GetAbilitySystemComponent();

        DestActor = World.SpawnActor<AbilitySystemTestActor>();
        DestComponent = DestActor.GetAbilitySystemComponent();
        Test_InstantDamage();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Test_InstantDamage()
    {
        float DamageValue = 5.0f;
        float StartingHealth = DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health;
        GameplayEffect BaseDmgEffect = new GameplayEffect();
        BaseDmgEffect.DurationPolicy = EGameplayEffectDurationType.Instant;
        SourceComponent.ApplyGameplayEffectToTarget(BaseDmgEffect, DestComponent, 1);
    }
    public void AddModifier(GameplayEffect Effect, EGameplayModOp Op)
    {
        GameplayModifierInfo Info = new GameplayModifierInfo();
        Effect.Modifiers.Add(Info);
        Info.ModifierOp = Op;
    }
}
