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
        //ͨ������
        ClassC c = new ClassC();

        c.classList.Add(new ClassB());
        //�ύ��Ϣ��Core����
        c.AddModifier(typeof(ClassB), "Health");
        //����Core�����ִ��
        c.ExecuteModifier();
        //=================================================

        //�Ƿ���
        c.classList.Add(new ClassB());
        //�ύ��Ϣ��Core����
        c.AddModifier(typeof(ClassB), (int)EAttributeType.Health);
        //����Core�����ִ��
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
            //�������
            //ClassBase instance = GetObj(AttributeOwner);
            //instance.GetType().GetField(Attribute).SetValue(instance, 10);

            //�Ƿ���
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

