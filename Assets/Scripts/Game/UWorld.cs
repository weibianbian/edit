using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public T SpawnActor<T>() where T : Actor
        {
            return null;
        }
        public Actor SpawnActor(Type type, Vector3 Location, Quaternion Rotation)
        {
            ULevel LevelToSpawnIn = CurrentLevel;
            Guid ActorGuid = Guid.NewGuid();

            Actor Actor = ReferencePool.Acquire(type) as Actor;

            LevelToSpawnIn.Actors.Add(Actor);

            Actor.PostSpawnInitialize();

            return Actor;
        }
    }
}