using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TestRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Quaternion q1 = Quaternion.Euler(0, -45, 0);

            Quaternion q2=transform.rotation;

            Debug.Log(Quaternion.Angle(q1, q2));
            Debug.Log(Quaternion.Angle(q2, q1));
            Quaternion q = Quaternion.AngleAxis(Quaternion.Angle(q1, q2), Vector3.up);

            transform.rotation = q* transform.rotation;
        }
    }
}
