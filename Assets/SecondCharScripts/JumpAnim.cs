using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnim : MonoBehaviour
{
    public bool isJumping = false;
    public float jumpDuration = 0.2f;
    public Animator mAnimator;
    
    public void Jumpanims()
    {
        StartCoroutine(JumpAnimation()); 
        StartCoroutine(LandingAnimation());
    }

    IEnumerator JumpAnimation()
    {
        isJumping = true;
        mAnimator.SetTrigger("Jump");

        float elapsedTime = 0f;
        //Sets the original position as character's current position
        Vector3 originalPosition = transform.position;
        //Sets the targeted jump height by adding 1.0f to the y value
        Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y + 1.0f, originalPosition.z);

        //While the elapsed time is lower than the jump duration, player will slowly move towards the set Y value
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        //Make sure character has reached peak jump height
        transform.position = targetPosition;
    }

    IEnumerator LandingAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        mAnimator.SetTrigger("Landing");

        float elapsedTime = 0f;
        //Sets the original position as character's current position
        Vector3 originalPosition = transform.position;
        //Sets the targeted jump height by deducting 1.0f to the y value
        Vector3 targetDownPosition = new Vector3(originalPosition.x, originalPosition.y - 1.5f, originalPosition.z);

        // Move down
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetDownPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Making sure the character has touched the floor
        transform.position = targetDownPosition;

        //Delay for animation to play
        yield return new WaitForSeconds(1.0f);

        //Moving the character back up and sets the targeted down height by deducting 1.0f to the y value
        Vector3 targetUpPosition = new Vector3(originalPosition.x, originalPosition.y - 1.0f, originalPosition.z);
        elapsedTime = 0f;

        //While the elapsed time is lower than the jump duration, player will slowly move towards the set Y value
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(targetDownPosition, targetUpPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Make sure that the character has landed
        transform.position = targetUpPosition;

        isJumping = false;
    }

}
