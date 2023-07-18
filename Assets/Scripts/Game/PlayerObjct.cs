using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace RailShootGame
{
    public class PlayerObjct : MonoBehaviour
    {
        [System.NonSerialized]
        public CharacterController characterController;
        public float speed = 1.8f;

        public void Awake()
        {
            characterController = transform.GetComponent<CharacterController>();
        }
        public void Update()
        {
            Vector3 dir = Vector3.zero;
            //if (Input.GetMouseButton(0))
            //{
            //    dir+=Vector3 
            //}
            if (Input.GetKey(KeyCode.W))
            {
                dir += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.D))
            {
                dir += Vector3.right;
            }
            if (Input.GetKey(KeyCode.S))
            {
                dir += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A))
            {
                dir += Vector3.left;
            }
            if (dir!=Vector3.zero)
            {
                Move(dir.normalized * speed * Time.deltaTime);
            }
        }
        public void Move(Vector3 velocity)
        {
            //transform.position += Vector3.up * Time.deltaTime;
            velocity.y -= 9 * Time.deltaTime;
            CollisionFlags flags = characterController.Move(velocity);
        }
    }
}

