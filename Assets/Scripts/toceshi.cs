using System;
using System.Collections.Generic;
using Test.Core;
using Test.GamePlay;
using UnityEngine;

public class toceshi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //通过反射
        ClassC c = new ClassC();

        c.classList.Add(new ClassB());
        //提交信息给Core代码
        c.AddModifier(typeof(ClassB), "Health");
        //调用Core代码的执行
        c.ExecuteModifier();
        //=================================================

        //非反射
        c.classList.Add(new ClassB());
        //提交信息给Core代码
        c.AddModifier(typeof(ClassB), (int)EAttributeType.Health);
        //调用Core代码的执行
        c.ExecuteModifier();
    }


}
namespace Test.GamePlay
{
    public enum EAttributeType
    {
        Health,
    }
    public class ClassBase
    {
        public virtual void ExecuteModifier(int EAttributeType)
        {
        }

    }

    public class ClassB : ClassBase
    {
        public float Health;
        public override void ExecuteModifier(int InEAttributeType)
        {
            if (InEAttributeType == (int)EAttributeType.Health)
            {

            }
        }
    }
}
namespace Test.Core
{
    public class ClassC
    {
        public List<ClassBase> classList = new List<ClassBase>();
        public Type AttributeOwner;
        public string Attribute;
        public int EAttributeType;
        public void AddModifier(Type AttributeOwner, string NewProperty)
        {
            this.AttributeOwner = AttributeOwner;
            this.Attribute = NewProperty;
        }
        public void AddModifier(Type AttributeOwner, int EAttributeType)
        {
            this.AttributeOwner = AttributeOwner;
            this.EAttributeType = EAttributeType;
        }
        public void ExecuteModifier()
        {
            //反射代码
            //ClassBase instance = GetObj(AttributeOwner);
            //instance.GetType().GetField(Attribute).SetValue(instance, 10);

            //非反射
            ClassBase instance = GetObj(AttributeOwner);
            instance.ExecuteModifier(EAttributeType);
        }

        public ClassBase GetObj(Type type)
        {
            for (int i = 0; i < classList.Count; i++)
            {
                if (classList[i].GetType() == type)
                {
                    return classList[i];
                }
            }
            return null;
        }
    }
}

