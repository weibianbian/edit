using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UEMoveTest : MonoBehaviour
{
    public ACharacter character;
    // Start is called before the first frame update
    void Start()
    {
        character=new ACharacter();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            character.Jump();
        }
        character.Tick(Time.deltaTime);
    }
}
