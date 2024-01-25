using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;

//Class for the player states in the finite state machine (FSM)
public class PlayerState_Multiplayer : FSMState
{
    //Reference to the Player_Multiplayer instance
    protected Player_Multiplayer mPlayer = null;
    //Initializes the player reference and FSM
    public PlayerState_Multiplayer(Player_Multiplayer player) 
        : base()
    {
        mPlayer = player;
        mFsm = mPlayer.mFsm;
    }

    //Enters the current state
    public override void Enter()
    {
        base.Enter();
    }
    //Exits the current state
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

//State representing the player's movement state
public class PlayerState_Multiplayer_MOVEMENT : PlayerState_Multiplayer
{
    //Sets the state ID for movement
    public PlayerState_Multiplayer_MOVEMENT(Player_Multiplayer player) : base(player)
    {
        mId = (int)(PlayerStateType.MOVEMENT);
    }
    //Enters movement state
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        HandleMovement();
        CheckAttackButtons();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    //Handles player movement
    private void HandleMovement()
    {
        mPlayer.Move();
    }
    //Checks for attack inputs and changes to attack state if pressed
    private void CheckAttackButtons()
    {
        for (int i = 0; i < mPlayer.mAttackButtons.Length; ++i)
        {
            if (mPlayer.mAttackButtons[i] && mPlayer.mBulletsInMagazine > 0)
            {
                TransitionToAttackState(i);
            }
        }
    }

    private void TransitionToAttackState(int attackId)
    {
        PlayerState_Multiplayer_ATTACK attackState = (PlayerState_Multiplayer_ATTACK)mFsm.GetState((int)PlayerStateType.ATTACK);
        attackState.AttackID = attackId;
        mPlayer.mFsm.SetCurrentState((int)PlayerStateType.ATTACK);
    }
}

//State representing the player's attack state
public class PlayerState_Multiplayer_ATTACK : PlayerState_Multiplayer
{
    //ID representing the type of attack
    private int mAttackID = 0;

    //Name of the attack animation
    private string mAttackAnimName;

    //Getting and setting the attack ID
    public int AttackID
    {
        get
        {
            return mAttackID;
        }
        set
        {
            mAttackID = value;
            mAttackAnimName = "Attack" + (mAttackID + 1).ToString();
        }
    }

    //Sets the state ID for attack
    public PlayerState_Multiplayer_ATTACK(Player_Multiplayer player) : base(player)
    {
        mId = (int)(PlayerStateType.ATTACK);
    }
    //Enters attack state
    public override void Enter()
    {
        //Set the attack animation to true
        mPlayer.mAnimator.SetBool(mAttackAnimName, true);
    }
    //Exits attack state
    public override void Exit()
    {
        //Set the attack animation to false
        mPlayer.mAnimator.SetBool(mAttackAnimName, false);
    }
    public override void Update()
    {
        base.Update();
        //Debug information about ammunition count and bullets in the magazine
        Debug.Log("Ammo count: " + mPlayer.mAmunitionCount + ", In Magazine: " + mPlayer.mBulletsInMagazine);
        ChangeState();
        CheckForFire();
    }

    //Changes the state of the player
    public void ChangeState()
    {
        //Transitions to reload state if the current magazine bullets are 0 but there is still ammunition
        if (mPlayer.mBulletsInMagazine == 0 && mPlayer.mAmunitionCount > 0)
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.RELOAD);
            return;
        }

        //Transitions to movement state if both magazine and ammunition are depleted
        if (mPlayer.mAmunitionCount <= 0 && mPlayer.mBulletsInMagazine <= 0)
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
            mPlayer.NoAmmo();
            return;
        }
    }

    //Checks if the player fires the bullet
    public void CheckForFire()
    {
        //Fires if an attack button is pressed
        if (mPlayer.mAttackButtons[mAttackID])
        {
            //Sets attack animation to true and calls the Fire method
            mPlayer.mAnimator.SetBool(mAttackAnimName, true);
            mPlayer.Fire(AttackID);
        }
        else
        {
            //Sets attack animation to true and changes to MOVEMENT state
            mPlayer.mAnimator.SetBool(mAttackAnimName, false);
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }
    }
}



// State representing the player's reload state
public class PlayerState_Multiplayer_RELOAD : PlayerState_Multiplayer
{
    public float ReloadTime = 3.0f;
    float dt = 0.0f;
    public int previousState;

    //Sets the state ID for reload
    public PlayerState_Multiplayer_RELOAD(Player_Multiplayer player) : base(player)
    {
        mId = (int)(PlayerStateType.RELOAD);
    }
    //Enters reload state
    public override void Enter()
    {
        mPlayer.mAnimator.SetTrigger("Reload");
        mPlayer.Reload();
        dt = 0.0f;
    }

    //Increasion magazine and decreases ammunition by bullets before reload
    public override void Exit()
    {
        if (mPlayer.mAmunitionCount > mPlayer.mMaxAmunitionBeforeReload)
        {
            mPlayer.mBulletsInMagazine += mPlayer.mMaxAmunitionBeforeReload;
            mPlayer.mAmunitionCount -= mPlayer.mBulletsInMagazine;
        }
        else if (mPlayer.mAmunitionCount > 0 && mPlayer.mAmunitionCount < mPlayer.mMaxAmunitionBeforeReload)
        {
            mPlayer.mBulletsInMagazine += mPlayer.mAmunitionCount;
            mPlayer.mAmunitionCount = 0;
        }
    }

    public override void Update()
    {
        dt += Time.deltaTime;
        if (dt >= ReloadTime)
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }
    }

    public override void FixedUpdate()
    {
    }
}
