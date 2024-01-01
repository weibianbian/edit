using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class UEMoveTest : MonoBehaviour
{
    public UWorld World;
    public ACharacter character;
    // Start is called before the first frame update
    void Start()
    {
        World = new UWorld();
        ULevel level = new ULevel();
        World.AddToWorld(level);
        World.CurrentLevel = level;
        character = World.SpawnActor<ACharacter>();
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
    private void FixedUpdate()
    {
    }
}
