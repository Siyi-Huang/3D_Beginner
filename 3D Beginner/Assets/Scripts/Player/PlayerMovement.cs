﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // radians that character rotates per second
    public float turnSpeed = 20f;

    // Reference
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    // Vector of 3D space
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    private float m_SpeedRatio = 1f;

    // Start is called before the first frame update
    void Start ()
    {
        // Set reference
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource>();
    }

    /* FixedUpdate is called 
     * before the physics system 
     * handles all collisions and other interactions that have occurred
     */
    void FixedUpdate ()
    {
        // Variate of horizontal axis
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");

        // Set value (x_axis, y_axis, z_axis)
        m_Movement.Set(horizontal, 0f, vertical);
        /* Nomalization:
         * keep the direction of the vector the same, but change its magnitude to 1
         */
        m_Movement.Normalize ();

        // hasHorizontalInput is true only if horizontal is not approximate to 0
        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        // OR operator
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        if(isWalking)
        {
            if(!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play ();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }

        m_Animator.SetBool ("IsWalking", isWalking);
        // Compute the forward vector of the character 
        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, 
            turnSpeed * Time.deltaTime, 0f);
        // Create a rotation in the direction of the given parameters
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    // Apply root motion as needed
    void OnAnimatorMove ()
    {
        // Movement
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement 
            * m_Animator.deltaPosition.magnitude * m_SpeedRatio);
        // Rotation
        m_Rigidbody.MoveRotation (m_Rotation);
    }

    // Set speed
    public void SetSpeed(float ratio, bool IsSet)
    {
        if (IsSet == true)
        {
            m_SpeedRatio = ratio;
        }
        else if (IsSet == false)
        {
            m_SpeedRatio = 1f;
        }
    }
}
