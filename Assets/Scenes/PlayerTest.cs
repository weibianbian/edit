using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public CharacterController character;
    public Vector3 velocity;
    public bool isGround=false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        character.SimpleMove(velocity);
        isGround = character.isGrounded;
    }
}
