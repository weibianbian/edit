using CopyBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCopyBT : MonoBehaviour
{
    // Start is called before the first frame update
    public AbigailBrain brain;
    void Start()
    {
        brain = new AbigailBrain();
        brain.Start();
    }

    // Update is called once per frame
    void Update()
    {
        brain.bt.Update();
    }
}
