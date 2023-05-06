using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyCreatorAgent : SerializedMonoBehaviour
{
    [Title("敌人生成器")]
    [HideReferenceObjectPicker,HideLabel]
    public EnemyCreatorData enemyCreatorData=new EnemyCreatorData();

}
[Serializable]
public class EnemyCreatorData
{
    public WaypointGroup waypointGroup = new WaypointGroup();

    [LabelText("Enemy")]
    public CreateEnemyAction createEnemyAction =new CreateEnemyAction();
}
[Serializable]
public class CreateEnemyAction
{
    [ShowInInspector]
    public string prefabPath;
}

