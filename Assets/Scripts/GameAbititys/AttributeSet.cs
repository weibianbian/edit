﻿using RailShootGame;
using System;

namespace GameplayAbilitySystem
{
    //   * 定义游戏所有GameplayAttributes的集合
    //* 游戏应该继承这个类并添加FGameplayAttributeData属性来表示诸如生命值、伤害等属性
    //* 属性集被作为子对象添加到actor中，然后注册到AbilitySystemComponent
    //* 它通常希望每个项目都有几个相互继承的集合//
    //* 你可以创建一个基础的生命值集合，然后拥有一个继承它并添加更多属性的玩家集合
    public class AttributeSet : ReferencePoolObject
    {
        public virtual bool PreGameplayEffectExecute(FGameplayEffectModCallbackData Data)
        {
            return true;
        }
        public virtual bool PostGameplayEffectExecute(FGameplayEffectModCallbackData Data)
        {
            return true;
        }
        public virtual void PreAttributeBaseChange(GameplayAttribute Attribute, float NewValue)
        {

        }
        public virtual void PostAttributeBaseChange(GameplayAttribute Attribute, float OldValue, float NewValue)
        {

        }
    }
}

