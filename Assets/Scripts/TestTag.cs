using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTag : MonoBehaviour
{
    UGameplayTagsManager TagsManager=new UGameplayTagsManager();

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
