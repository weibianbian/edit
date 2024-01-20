using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public CharacterController character;
    public Vector3 velocity;
    public bool isGround = false;
    public Collider ColliderA;
    public Collider ColliderB;
    public Vector3 outDir;
    public float distance;
    public bool isComputePenetration;
    public CapsuleCollider capsule;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //isComputePenetration = Physics.ComputePenetration(ColliderA, ColliderA.transform.position, ColliderA.transform.rotation,
        //   ColliderB, ColliderB.transform.position, ColliderB.transform.rotation, out outDir, out distance);
        //character.SimpleMove(velocity);
        //isGround = character.isGrounded;
        Vector3 p1 = capsule.transform.position - capsule.height * 0.5f * Vector3.up + Vector3.up * capsule.radius;
        Vector3 p2 = capsule.transform.position + capsule.height * 0.5f * Vector3.up - Vector3.up * capsule.radius;
        RaycastHit[] hits = Physics.CapsuleCastAll(p1, p2, capsule.radius, -Vector3.up, 100);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Debug.Log(hit.collider.gameObject);
        }
    }
}
