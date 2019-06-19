using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class Player_Manny : MonoBehaviour
{
    public float gravity = -10;
    public float moveSpeed = 10f;
    public float jumpHeight = 7f;


    public float centreRadius = .1f; // For ladders

    private CharacterController2D cc;
    private SpriteRenderer sr;
    private Animator a;

    private Vector3 velocity;
    bool isClimbing = false;

    void Start()
    {
        cc = GetComponent<CharacterController2D>();
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
    }

    void Update()
    {
        // Gathers Left and Right input
        float inputH = Input.GetAxis("Horizontal");
        // Gathers Up and Down input
        float inputV = Input.GetAxis("Vertical");

        // Get Spacebar input
        bool isJumping = Input.GetButtonDown("Jump");

        if (!cc.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            a.SetBool("IsGrounded", false);
        }
        else
        {
            velocity.y = -1;
            a.SetBool("IsGrounded", true);
        }
        print("Y velocity = " + velocity.y);

        if (isJumping && cc.isGrounded)
        {
            Jump();
        }

        a.SetFloat("VelocityY", velocity.y);

        Run(inputH);
        Climb(inputH, inputV);

        // Applies velocity to CharacterController2D (to get it to move)
        cc.move(velocity * Time.deltaTime);
    }

    void Run(float inputH)
    {
        // Move the character cc left / right with input
        velocity.x = inputH * moveSpeed;

        // Set bool to true is input is pressed
        bool isRunning = inputH != 0;

        // Animate the player to running if input is pressed
        a.SetBool("IsRunning", isRunning);

        // Check if input is pressed
        if (isRunning)
        {
            // Flip character depending on left/right input
            sr.flipX = inputH < 0;
        }
    }

    /*
    void Climb(float inputV)
    {
        bool isOverLadder = false;
        bool isClimbing = false;

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        
        foreach (var hit in hits)
        {
            if (hit.tag == "Ladder")
            {
                isOverLadder = true;
                break; // Cancels foreach loop
            }

            if (isOverLadder && inputV != 0)
            {
                isClimbing = true;
            }
        }
    }
    */

    void Climb(float inputH, float inputV)
    {
        bool isOverLadder = false; // Is overlapping ladder
                                   // Get a list of all hit objects overlapping point
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, centreRadius);
        // Loop through each point
        foreach (var hit in hits)
        {
            // If point overlaps a climbable object
            if (hit.tag == "Ladder")
            {
                // Player is overlapping ladder!
                isOverLadder = true;
                break; // Exit foreach loop
            }
        }

        // If is overlapping ladder and input V has been made
        if (isOverLadder && inputV != 0)
        {
            //  Is Climbing
            isClimbing = true;
            velocity.y = 0; // Cancel Y velocity
        }

        // If NOT over ladder
        if (!isOverLadder)
        {
            // Not climbing anymore
            isClimbing = false;
        }

        // If is climbing
        if (isClimbing)
        {
            // Translate character up and down
            Vector3 inputDir = new Vector3(inputH, inputV);
            transform.Translate(inputDir * moveSpeed * Time.deltaTime);
        }

        a.SetBool("IsClimbing", isClimbing);
        a.SetFloat("ClimbSpeed", inputV);
    }

    void Jump()
    {
        // Set velocity's Y to height
        velocity.y = jumpHeight;
    }
}
