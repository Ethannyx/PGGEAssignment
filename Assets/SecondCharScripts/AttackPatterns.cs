using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{
    //Making variables call attack patterns
    private enum AttackPattern
    {
        Pattern1,
        Pattern2
    }

    //Setting the default attack pattern
    private AttackPattern currentAttackPattern = AttackPattern.Pattern1;
    public Animator mAnimator; 
    
    //In main script, presses E to call this function and then switches the attack
    public void SwitchPattern()
    {
        if (currentAttackPattern == AttackPattern.Pattern1)
        {
            currentAttackPattern = AttackPattern.Pattern2;
        }
        else
        {
            currentAttackPattern = AttackPattern.Pattern1;
        }
    }

    //Triggers the animation based on the attack pattern
    public void PlayAttackPattern()
    {
        if (currentAttackPattern == AttackPattern.Pattern1)
        {
            mAnimator.SetTrigger("Punch");
        }
        else if (currentAttackPattern == AttackPattern.Pattern2)
        {
            mAnimator.SetTrigger("ComboPunch");
        }
    }
}
