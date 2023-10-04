using RailShootGame;
using System;

namespace GameplayAbilitySystem
{
    public class FGameplayAbilityActorInfo : ReferencePoolObject
    {
        public AActor OwnerActor;
        public AActor AvatarActor;
        public UAbilitySystemComponent AbilitySystemComponent;
        public void InitFromActor(AActor InOwnerActor, AActor InAvatarActor, UAbilitySystemComponent InAbilitySystemComponent)
        {
            OwnerActor = InOwnerActor;
            AvatarActor = InAvatarActor;
            AbilitySystemComponent = InAbilitySystemComponent;
        }
    }
}

