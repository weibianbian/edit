using System.Reflection;
using UnityEngine;

namespace RailShootGame
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            InitManagers();
        }
        private void Start()
        {

        }
        public void Update()
        {
        }
        public void InitManagers()
        {
            ActorManager = CreateManager<ActorManager>();
            ResourceManager = CreateManager<ResourceManager>();

            ActorManager.SetResourceManager( ResourceManager );
        }
        private T CreateManager<T>() where T : BaseManager
        {
            GameObject obj = new GameObject(typeof(T).Name);
            T t = obj.AddComponent<T>() as T;
            obj.transform.parent = transform;
            return t;
        }
        public static ActorManager ActorManager
        {
            get;
            private set;
        }
        public static ResourceManager ResourceManager
        {
            get;
            private set;
        }
    }
}

