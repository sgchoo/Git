using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //Player Move Speed
    public float moveSpeed = 7;

    //Player Use Gravity
    float yVelocity = 0;
    float gravity = -20f;

    //Player Jump Power
    public int jumpPower = 10;

    CharacterController cc;

    public bool isJumping;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Player Move
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        dir = Camera.main.transform.TransformDirection(dir);

        //Player Gravity
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

        //Player Jump
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }
        }

        //Player Jump
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            yVelocity = jumpPower;
        }
    }
}
