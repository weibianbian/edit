using Core.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RailShootGame
{
    public class ULevel
    {
        public List<AActor> Actors = new List<AActor>();
        public UWorld OwningWorld;
    }

    public class UWorld
    {
        public ULevel CurrentLevel;
        public FTimerManager TimerManager;
        public UWorld()
        {
            TimerManager=new FTimerManager();
        }
        public void AddToWorld(ULevel Level)
        {
            Level.OwningWorld = this;
        }
        public T SpawnActor<T>() where T : AActor
        {
            AActor Actor = SpawnActor(typeof(T), Vector3.zero, Quaternion.identity);
            return Actor as T;
        }
        public AActor SpawnActor(Type type, Vector3 Location, Quaternion Rotation)
        {
            ULevel LevelToSpawnIn = CurrentLevel;
            Guid ActorGuid = Guid.NewGuid();

            AActor Actor = ReferencePool.Acquire(type) as AActor;
            Actor.Outer = LevelToSpawnIn;
            LevelToSpawnIn.Actors.Add(Actor);

            Actor.PostSpawnInitialize();

            return Actor;
        }
        public FTimerManager GetTimerManager()
        {
            return TimerManager;
        }
        public void Tick(float InTime)
        {
            TimerManager.Tick(InTime);
        }
    }
}