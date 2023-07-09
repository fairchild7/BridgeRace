using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] DynamicJoystick joystick;

    [SerializeField] float moveSpeed = 5f;

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(joystick.Horizontal * moveSpeed, rb.velocity.y, joystick.Vertical * moveSpeed);    
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            //ChangeAnimation("Run");
        }
        else
        {
            //ChangeAnimation("Idle");
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        brickCount = 0;
        //gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
