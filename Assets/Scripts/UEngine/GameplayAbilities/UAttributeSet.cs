using RailShootGame;
using System;
using UEngine.Core;

namespace UEngine.GameplayAbilities
{
    public interface IProp
    {

    }
    //   * 定义游戏所有GameplayAttributes的集合
    //* 游戏应该继承这个类并添加FGameplayAttributeData属性来表示诸如生命值、伤害等属性
    //* 属性集被作为子对象添加到actor中，然后注册到AbilitySystemComponent
    //* 它通常希望每个项目都有几个相互继承的集合//
    //* 你可以创建一个基础的生命值集合，然后拥有一个继承它并添加更多属性的玩家集合
    public class UAttributeSet : ReferencePoolObject, IProp
    {
        public virtual bool PreGameplayEffectExecute(FGameplayEffectModCallbackData Data)
        {
            return true;
        }
        public virtual bool PostGameplayEffectExecute(FGameplayEffectModCallbackData Data)
        {
            return true;
        }
        public virtual void PreAttributeChange(FGameplayAttribute Attribute, float NewValue)
        {
        }
        public virtual void PostAttributeChange(FGameplayAttribute Attribute, float OldValue, float NewValue)
        {

        }
        public virtual void PreAttributeBaseChange(FGameplayAttribute Attribute, float NewValue)
        {

        }
        public virtual void PostAttributeBaseChange(FGameplayAttribute Attribute, float OldValue, float NewValue)
        {

        }
    }
}

