using Core.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RailShootGame
{
    public class ULevel
    {
        public List<Actor> Actors = new List<Actor>();
        public UWorld OwningWorld;
    }

    public class UWorld
    {
        public ULevel CurrentLevel;
        public TimerManager TimerManager;
        public UWorld()
        {
            TimerManager=new TimerManager();
        }
        public void AddToWorld(ULevel Level)
        {
            Level.OwningWorld = this;
        }
        public T SpawnActor<T>() where T : Actor
        {
            Actor Actor = SpawnActor(typeof(T), Vector3.zero, Quaternion.identity);
            return Actor as T;
        }
        public Actor SpawnActor(Type type, Vector3 Location, Quaternion Rotation)
        {
            ULevel LevelToSpawnIn = CurrentLevel;
            Guid ActorGuid = Guid.NewGuid();

            Actor Actor = ReferencePool.Acquire(type) as Actor;
            Actor.Outer = LevelToSpawnIn;
            LevelToSpawnIn.Actors.Add(Actor);

            Actor.PostSpawnInitialize();

            return Actor;
        }
        public TimerManager GetTimerManager()
        {
            return TimerManager;
        }
    }
}