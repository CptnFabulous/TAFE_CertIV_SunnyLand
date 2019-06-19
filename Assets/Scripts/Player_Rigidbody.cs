using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;


[RequireComponent(typeof (CharacterController2D))]
public class Player_Rigidbody : MonoBehaviour
{
    CharacterController2D cc;

    public float speed = 10f;
    public float gravity = -10f;

    Animator a;
    SpriteRenderer sr;
    Rigidbody2D rb;


    
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();

        a = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        cc.move(transform.right * input.x * speed * Time.deltaTime);

        Animation();
    }

    void Move(float inputH, float inputV)
    {
        cc.move(transform.right * inputH * speed * Time.deltaTime);

    }

    void Animation()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            a.SetBool("IsRunning", true);
            sr.flipX = false;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            a.SetBool("IsRunning", true);
            sr.flipX = true;
        }
        else
        {
            a.SetBool("IsRunning", false);
        }

        a.SetFloat("VelocityX", rb.velocity.x);
        a.SetFloat("VelocityY", rb.velocity.y);
    }
}

