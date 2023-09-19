using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTag : MonoBehaviour
{
    GameplayTagsManager TagsManager=new GameplayTagsManager();

    // Start is called before the first frame update
    void Start()
    {
        TagsManager.ConstructGameplayTagTree();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
