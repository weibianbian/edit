namespace Core
{
    public class Singleton<T> where T : new()
    {
        public static T SingletonManager;
        public static T Get()
        {
            if (SingletonManager == null)
            {
                SingletonManager = new T();
                (SingletonManager as Singleton<T>).InitializeManager();
            }
            return SingletonManager;
        }
        public virtual void InitializeManager()
        {

        }
    }
}

