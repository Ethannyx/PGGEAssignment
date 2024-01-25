using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;
using Photon.Pun;

public class Player_Multiplayer : MonoBehaviour
{
    private PhotonView mPhotonView;

    [HideInInspector]
    public FSM mFsm = new FSM();
    public Animator mAnimator;  
    public PlayerMovement mPlayerMovement;

    // This is the maximum number of bullets that the player 
    // needs to fire before reloading.
    public int mMaxAmunitionBeforeReload = 40;

    // This is the total number of bullets that the 
    // player has.
    [HideInInspector]
    public int mAmunitionCount = 100;

    // This is the count of bullets in the magazine.
    [HideInInspector]
    public int mBulletsInMagazine = 40;

    [HideInInspector]
    public bool[] mAttackButtons = new bool[3];

    public Transform mGunTransform;
    public LayerMask mPlayerMask;
    public Canvas mCanvas;
    public RectTransform mCrossHair;


    public GameObject mBulletPrefab;
    public float mBulletSpeed = 10.0f;

    public int[] RoundsPerSecond = new int[3];
    bool[] mFiring = new bool[3];


    // Start is called before the first frame update
    void Start()
    {
        //Get the photonview component for multiplayer
        mPhotonView = GetComponent<PhotonView>();
        //Add player states to FSM and set the initial state
        mFsm.Add(new PlayerState_Multiplayer_MOVEMENT(this));
        mFsm.Add(new PlayerState_Multiplayer_ATTACK(this));
        mFsm.Add(new PlayerState_Multiplayer_RELOAD(this));
        mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
    }

    void Update()
    {
        //Check if the player object is controlled by the local player
        if (!mPhotonView.IsMine) return;
        //Update FSM and handle aiming
        mFsm.Update();
        Aim();
        //Update attack buttons based on Firing style   
        UpdateAttackMode();    
    }

    //Aiming of the game based on player's cursor and gun direction
    public void Aim()
    {
        //Calculate ray from gun to detect objects in front of the player
        Vector3 dir = -mGunTransform.right.normalized;
        Vector3 gunpoint = mGunTransform.transform.position +
                           dir * 1.2f -
                           mGunTransform.forward * 0.1f;
        //Find the layer mask that the ray can interact with  
        LayerMask objectsMask = ~mPlayerMask;

        RaycastHit hit;
        bool flag = Physics.Raycast(gunpoint, dir,
                        out hit, 50.0f, objectsMask);
        if (flag)
        {
            //Shows an X (crosshair) if the ray hits an object
            Debug.DrawLine(gunpoint, gunpoint +
                (dir * hit.distance), Color.red, 0.0f);

            RectTransform CanvasRect = mCanvas.GetComponent<RectTransform>();

            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(hit.point);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            mCrossHair.anchoredPosition = WorldObject_ScreenPosition;

            mCrossHair.gameObject.SetActive(true);
        }
        else
        {
            mCrossHair.gameObject.SetActive(false);
        }
    }

    //Handles player movement inputs
    public void Move()
    {
        if (!mPhotonView.IsMine) return;

        mPlayerMovement.HandleInputs();
        mPlayerMovement.Move();
    }

    //Placeholder for when the player has no ammo
    public void NoAmmo()
    {

    }
    //Placeholder for when the player is reloading
    public void Reload()
    {

    }
    //Initiate firing for a specific attack type
    public void Fire(int id)
    {
        if (mFiring[id] == false)
        {
            StartCoroutine(Coroutine_Firing(id));
        }
    }

    //Fires the bullet
    public void FireBullet()
    {
        if (mBulletPrefab == null) return;
        //Calcuate the direction of which the bullet will move and the spawning point of the bullet
        Vector3 dir = -mGunTransform.right.normalized;
        Vector3 firePoint = mGunTransform.transform.position + dir *
            1.2f - mGunTransform.forward * 0.1f;
        GameObject bullet = Instantiate(mBulletPrefab, firePoint,
            Quaternion.LookRotation(dir) * Quaternion.AngleAxis(90.0f, Vector3.right));
        //Apply the force to the bullet to make it move at the shooting direction
        bullet.GetComponent<Rigidbody>().AddForce(dir * mBulletSpeed, ForceMode.Impulse);
    }

    //Controls firing rate (make sure bullets dont spawn too many at a time)
    IEnumerator Coroutine_Firing(int id)
    {
        mFiring[id] = true;
        FireBullet();
        yield return new WaitForSeconds(1.0f / RoundsPerSecond[id]);
        mFiring[id] = false;
        mBulletsInMagazine -= 1;
    }

    //Update attack patterns based on input
    private void UpdateAttackMode()
    {
        //Set attack buttons based on input
        if (Input.GetButton("Fire1"))
        {
            mAttackButtons[0] = true;
            mAttackButtons[1] = false;
            mAttackButtons[2] = false;
        }
        else
        {
            mAttackButtons[0] = false;
        }

        if (Input.GetButton("Fire2"))
        {
            mAttackButtons[0] = false;
            mAttackButtons[1] = true;
            mAttackButtons[2] = false;
        }
        else
        {
            mAttackButtons[1] = false;
        }

        if (Input.GetButton("Fire3"))
        {
            mAttackButtons[0] = false;
            mAttackButtons[1] = false;
            mAttackButtons[2] = true;
        }
        else
        {
            mAttackButtons[2] = false;
        }
    }
}
