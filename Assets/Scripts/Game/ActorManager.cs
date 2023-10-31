using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RailShootGame
{
    public enum ESceneObjType
    {
        Enemy = 0,
        Bullet,
    }
    public class ResourceManager : BaseManager
    {
        public GameObject Load(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"path={path}");
            };
            return GameObject.Instantiate(prefab) as GameObject;
        }
    }
    public class ActorManager : BaseManager
    {
        private GameObject[] rootObjs = null;
        private Dictionary<EActorType, string> commonEmpty_Actors = new Dictionary<EActorType, string> {
            { EActorType.Cube,"Prefabs/Actors/EmptyCube"},
        };
        private Dictionary<EActorType, ESceneObjType> actorMapping = new Dictionary<EActorType, ESceneObjType>() {
            { EActorType.Cube,ESceneObjType.Enemy},
        };
        public List<AActor> gameActors = new List<AActor>(100);
        private ResourceManager resourceManager;
        public void Awake()
        {
            string[] names = Enum.GetNames(typeof(ESceneObjType));
            rootObjs = new GameObject[names.Length];
            for (int i = 0; i < names.Length; ++i)
            {
                GameObject obj = new GameObject();
                obj.transform.parent = gameObject.transform;
                obj.name = names[i];
                rootObjs[i] = obj;
            }
        }
        public void SetResourceManager(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }
        public AActor SpawnActor(EActorType actorType, Vector3 spawnPos)
        {
            ESceneObjType sceneObjType = actorMapping[actorType];

            GameObject instance = resourceManager.Load(commonEmpty_Actors[actorType]);

            ActorObject actorObject = instance.GetComponent<ActorObject>();

            if (actorObject == null)
            {
                actorObject = instance.AddComponent<ActorObject>();
            }

            instance.name = actorType.ToString();

            instance.transform.parent = rootObjs[(int)sceneObjType].transform;

            AActor actorLogic = actorObject.AttachLogic();

            actorLogic.Spawn();

            gameActors.Add(actorLogic);

            actorObject.transform.position = spawnPos;
            return actorLogic;
        }
    }
}

