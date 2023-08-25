using GameplayAbilitySystem;
using RailShootGame;
using UnityEngine;
public struct TestA
{
    public TestB b;
    public TestA(TestB Inb)
    {
        b = Inb;
    }
    public static implicit operator TestA(TestB data)
    {
        return new TestA(data);
    }
}
public struct TestB
{

}
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
        TestB testB = new TestB();
        TestA testA = testB;
        World = new UWorld();
        ULevel level = new ULevel();
        World.AddToWorld(level);
        World.CurrentLevel = level;

        float StartingHealth = 100.0f;
        float StartingMana = 200.0f;

        SourceActor = World.SpawnActor<AbilitySystemTestActor>();
        SourceComponent = SourceActor.GetAbilitySystemComponent();
        SourceComponent.GetSet<AbilitySystemTestAttributeSet>().Health = StartingHealth;
        SourceComponent.GetSet<AbilitySystemTestAttributeSet>().MaxHealth = StartingHealth;
        SourceComponent.GetSet<AbilitySystemTestAttributeSet>().Mana = StartingMana;
        SourceComponent.GetSet<AbilitySystemTestAttributeSet>().MaxMana = StartingMana;

        DestActor = World.SpawnActor<AbilitySystemTestActor>();
        DestComponent = DestActor.GetAbilitySystemComponent();
        DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health = StartingHealth;
        DestComponent.GetSet<AbilitySystemTestAttributeSet>().MaxHealth = StartingHealth;
        DestComponent.GetSet<AbilitySystemTestAttributeSet>().Mana = StartingMana;
        DestComponent.GetSet<AbilitySystemTestAttributeSet>().MaxMana = StartingMana;
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
        AddModifier(BaseDmgEffect, EGameplayModOp.Additive, new FScalableFloat(-DamageValue));
        BaseDmgEffect.DurationPolicy = EGameplayEffectDurationType.Instant;
        SourceComponent.ApplyGameplayEffectToTarget(BaseDmgEffect, DestComponent, 1);

        Debug.Log($"Health Reduced   {DestComponent.GetSet<AbilitySystemTestAttributeSet>().Health}={StartingHealth - DamageValue}");
    }
    public void AddModifier(GameplayEffect Effect, EGameplayModOp Op, FScalableFloat Magnitude)
    {
        GameplayModifierInfo Info = new GameplayModifierInfo();
        Effect.Modifiers.Add(Info);
        Info.ModifierOp = Op;
        Info.ModifierMagnitude = Magnitude;
    }
}
