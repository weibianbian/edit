using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM
{
    public class StateBase 
    {
        public string name;
        public StateBase(string name)
        {
            this.name = name;
        }

        public virtual void Enter()
        {
            Debug.Log($"enter state={name}");
        }
        public virtual void Exit()
        {

        }
        public virtual void OnLogic()
        {

        }
    }
}

