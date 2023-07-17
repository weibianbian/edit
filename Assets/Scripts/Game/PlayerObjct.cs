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
            if (Input.GetMouseButton(0))
            {
                Move(new Vector3(1, 0, 1).normalized * speed * Time.deltaTime);
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

