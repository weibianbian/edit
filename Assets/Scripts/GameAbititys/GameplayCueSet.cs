﻿using RailShootGame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class GameplayCueNotifyData
    {
        public Type LoadedGameplayCueClass;
        public GameplayTag GameplayCueTag;
        public GameplayTag ParentGameplayCueTag;
    }
    //数据类（允许编辑模式进行编辑）
    public class GameplayCueSet
    {
        public Dictionary<GameplayTag, GameplayCueNotifyData> GameplayCueDataMap = new Dictionary<GameplayTag, GameplayCueNotifyData>();
        public virtual bool HandleGameplayCue(Actor TargetActor, GameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {
            if (GameplayCueDataMap.TryGetValue(GameplayCueTag, out GameplayCueNotifyData CueData))
            {
                return HandleGameplayCueNotify_Internal(TargetActor, CueData, EventType, Parameters);
            }
            return false;
        }
        public virtual bool HandleGameplayCueNotify_Internal(Actor TargetActor, GameplayCueNotifyData CueData, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {
            bool bReturnVal = false;
            if (CueData.LoadedGameplayCueClass.IsSubclassOf(typeof(GameplayCueNotifyStatic)))
            {
                GameplayCueNotifyStatic NonInstancedCue = Activator.CreateInstance(CueData.LoadedGameplayCueClass) as GameplayCueNotifyStatic;
                if (NonInstancedCue.HandlesEvent(EventType))
                {
                    NonInstancedCue.HandleGameplayCue(TargetActor, EventType, Parameters);
                    bReturnVal = true;
                    if (!NonInstancedCue.IsOverride)
                    {

                    }

                }
                else if (CueData.LoadedGameplayCueClass.IsSubclassOf(typeof(GameplayCueNotifyActor)))
                {
                    GameplayCueNotifyActor InstancedCue = Activator.CreateInstance(CueData.LoadedGameplayCueClass) as GameplayCueNotifyActor;
                    if (true)
                    {

                    }
                }
            }
            return bReturnVal;
        }
    }
}
