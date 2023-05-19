namespace CopyBT
{
    public class Brain
    {
        BehaviourTree bt = null;

        public void Start()
        {
            OnStart();
        }
        protected virtual void OnStart()
        {

        }

    }
}

