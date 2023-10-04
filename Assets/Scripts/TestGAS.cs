using GameplayAbilitySystem;
using JetBrains.Annotations;
using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGAS : MonoBehaviour
{
    UAbilitySystemComponent uAbilitySystemComponent;
    // Start is called before the first frame update
    void Start()
    {
        uAbilitySystemComponent = new UAbilitySystemComponent();

        GameplayAbilityJump gameplayAbilityJump = new GameplayAbilityJump();
        FGameplayAbilitySpec AbilitySpec = new FGameplayAbilitySpec(gameplayAbilityJump,1);
        AbilitySpec.DynamicAbilityTags.AddTag(new FGameplayTag("InputTag.Jump"));
        uAbilitySystemComponent.GiveAbility(AbilitySpec);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            uAbilitySystemComponent.AbilityInputTagPressed(new FGameplayTag("InputTag.Jump"));
        }
        if (uAbilitySystemComponent != null)
        {
            uAbilitySystemComponent.ProcessAbilityInput(Time.deltaTime);
        }
    }
}
