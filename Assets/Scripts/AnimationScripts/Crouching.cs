using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MonoBehaviour
{
    public bool isCrouching = false;
    public Animator mAnimator;
    
    public void ToggleCrouch()
    {
        if (isCrouching)
        {
            unCrouch();
        }
        else
        {
            Crouch();
        }
    }

    //Triggers the crouch animation
    void Crouch()
    {
        mAnimator.SetBool("Crouch", true); 
        isCrouching = true;
    }

    //Triggers the standing animation
    void unCrouch()
    {
        mAnimator.SetBool("Crouch", false); 
        isCrouching = false;
    }
}
