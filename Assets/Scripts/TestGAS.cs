using Core;
using GameplayAbilitySystem;
using JetBrains.Annotations;
using RailShootGame;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TestGAS : MonoBehaviour
{
    public UWorld World;
    AbilitySystemTestActor SourceActor;
    void Start()
    {
        Type t = typeof(GameplayAbilitiesModule);
        Debug.Log(t.GetInterface(typeof(IModuleInterface).Name));
        World = new UWorld();
        ULevel level = new ULevel();
        World.AddToWorld(level);
        World.CurrentLevel = level;

        float StartingHealth = 100.0f;
        float StartingMana = 200.0f;

        SourceActor = World.SpawnActor<AbilitySystemTestActor>();

        UGameplayAbilityJump gameplayAbilityJump = new UGameplayAbilityJump();
        FGameplayAbilitySpec AbilitySpec = new FGameplayAbilitySpec(gameplayAbilityJump, 1);
        AbilitySpec.DynamicAbilityTags.AddTag(new FGameplayTag("InputTag.Jump"));
        SourceActor.GetAbilitySystemComponent().GiveAbility(AbilitySpec);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SourceActor.GetAbilitySystemComponent().AbilityInputTagPressed(new FGameplayTag("InputTag.Jump"));
        }
        if (SourceActor.GetAbilitySystemComponent() != null)
        {
            SourceActor.GetAbilitySystemComponent().ProcessAbilityInput(Time.deltaTime);
        }
    }
}
