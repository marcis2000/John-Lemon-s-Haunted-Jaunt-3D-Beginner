using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 m_Movement; // _m means non-public member variable
    Quaternion m_Rotation = Quaternion.identity; //Quaternion a way of storing rotation, part after = means no rotation


    Animator m_Animator;
    public float turnSpeed = 20f; // public variable sotring the speed of character turn to make it look natural
                                  // using camelCase not m_prefix because variable is public

    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;


    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // FixedUpdathe because OnAnimatorMove works with physics and we don't want conflicts with frames
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical); // moves in 3 directions (y=0)
        m_Movement.Normalize(); // Fixing diagonal distance form 1.414 to 1

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); // checking if values are equal
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking); // setting the variable stored in m_Animator

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        // vector responsible for turning the front of the character to movement direction, first two are Vector3 start and stop directions
        //  third is change in radians and fourth is magnitude
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward); // chacking for rotation

    }

    // Mathod allowing to apply root motion and rotation separately in physics time
    void OnAnimatorMove()
    {
        // taking cuurent position of root and changing it by movement vector multiplied by deltaPosition which is motion applied to this frame
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation); //basically changing rotation

    }
}
