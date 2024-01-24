using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;
using Photon.Pun;

public class Multi2 : MonoBehaviour
{
    private PhotonView mPhotonView;

    [HideInInspector]
    public CharacterController mCharacterController;
    public Animator mAnimator;
    
    //Sets ammo value
    public int currentammo = 4;

    //Walking
    public float mWalkSpeed = 1.0f;
    public float mRotationSpeed = 50.0f;

    //Referencing functions from other codes
    private JumpAnim jumplanding;
    private AttackPatterns attacking;
    private Crouching crouch;

    // Start is called before the first frame update
    void Start()
    {
        mPhotonView = GetComponent<PhotonView>();
        mCharacterController = GetComponent<CharacterController>();
        //Getting the functions from the other scripts
        jumplanding = GetComponent<JumpAnim>();
        attacking = GetComponent<AttackPatterns>();
        crouch = GetComponent<Crouching>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mPhotonView.IsMine) return;
        //Movement
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        float speed = mWalkSpeed;
        bool isMoving = hInput != 0.0f || vInput != 0.0f;
        

        //Sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = mWalkSpeed * 2.0f;
        }   

        //running front walking back movement
        if (mAnimator == null) return;
        //Rotates the character in the speed of the rotation speed
        transform.Rotate(0.0f, hInput * mRotationSpeed * Time.deltaTime, 0.0f);
        Vector3 forward = transform.TransformDirection(Vector3.forward).normalized;
        //Make sure the character does not fly
        forward.y = 0.0f;
        //Moves the character forward using VInput and forward input
        mCharacterController.Move(forward * vInput * speed *Time.deltaTime);
        //Set the floats in the animator for the animations to play
        mAnimator.SetFloat("PosX", hInput * speed / (2.0f * mWalkSpeed));
        mAnimator.SetFloat("PosZ", vInput * speed / (2.0f * mWalkSpeed));

        //jumping animation
        if (Input.GetKeyDown(KeyCode.Space) && !jumplanding.isJumping)
        {
            jumplanding.Jumpanims();
        }

        //crouch animation
        if (Input.GetKeyDown(KeyCode.F))
        {
        if (mAnimator != null)
        {
            crouch.ToggleCrouch();
        }
        }

        //Attack animation       
        if (Input.GetMouseButtonDown(0))
        {
            if (mAnimator != null)
            {
                //Checks if current ammo is more than zero
               if (currentammo > 0)
                {
                    //plays animation if there is still ammo left
                    attacking.PlayAttackPattern();
                    //Reduces amount of ammo after attacking by 1
                    currentammo = Mathf.Max(0, currentammo - 1);
                    //Stating the amount of ammo left
                    Debug.Log("current ammo is " + currentammo);
                }
                else
                {
                    //Cancels playing of the animation and asks for reload as there is no more ammo
                    Debug.Log("Please reload");
                }
            }
        }

        //Input to detect for switch of attack pattern
        if (Input.GetKeyDown(KeyCode.E))
        {
            attacking.SwitchPattern();
            Debug.Log("Attack Pattern Switched");
        }

        //Reload Animation and reset ammo count to 4
        if (Input.GetKeyDown(KeyCode.R))
        {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("Reload");
            currentammo = 4;
        }
        }
    }
}
