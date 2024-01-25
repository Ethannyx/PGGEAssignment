using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;

public class Player : MonoBehaviour
{
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
    private Vector3 gunpoint;
    private Vector3 dir;
    public int[] RoundsPerSecond = new int[3];
    bool[] mFiring = new bool[3];


    void Start()
    {
        InitializeFSM();
    }

    void Update()
    {
        mFsm.Update();
        Aim();
        UpdateAttackButtons();
    }

    //Calculating the direction of bullet, position of crosshair and the interactable objects
    public void Aim()
    {
        Vector3 dir = -mGunTransform.right.normalized;
        Vector3 gunpoint = CalculateGunpointPosition(dir);
        LayerMask objectsMask = ~mPlayerMask;

        //Raycast of gun direction
        RaycastHit hit;
        bool hitObject = Physics.Raycast(gunpoint, dir, out hit, 50.0f, objectsMask);

        if (hitObject)
        {
            UpdateCrosshair(hit);
        }
        else
        {
            mCrossHair.gameObject.SetActive(false);
        }

    }

    //Calculate the direction of which the gun is facing
    private Vector3 CalculateGunpointPosition(Vector3 dir)
    {
        return mGunTransform.transform.position + dir * 1.2f - mGunTransform.forward * 0.1f;
    }

    //Makes sure the crosshair stays in the middle od the screen
    private void UpdateCrosshair(RaycastHit hit)
    {
        Debug.DrawLine(gunpoint, gunpoint + (dir * hit.distance), Color.red, 0.0f);
        RectTransform canvasRect = mCanvas.GetComponent<RectTransform>();
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(hit.point);
        Vector2 screenPosition = new Vector2(
            (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));

        mCrossHair.anchoredPosition = screenPosition;
        mCrossHair.gameObject.SetActive(true);
    }

    //Handles the movement of the player
    public void Move()
    {
        mPlayerMovement.HandleInputs();
        mPlayerMovement.Move();
    }

    public void NoAmmo()
    {

    }

    public void Reload()
    {

    }

    public void Fire(int id)
    {
        if (mFiring[id] == false)
        {
            StartCoroutine(Coroutine_Firing(id));
        }
    }

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
    //Initialize the FSMs to add the states of our player
    private void InitializeFSM()
    {
        mFsm.Add(new PlayerState_MOVEMENT(this));
        mFsm.Add(new PlayerState_ATTACK(this));
        mFsm.Add(new PlayerState_RELOAD(this));
        mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
    }

    //Updates the fire mode based on the input
    private void UpdateAttackButtons()
    {
        UpdateFireMode("Fire1", 0);
        UpdateFireMode("Fire2", 1);
        UpdateFireMode("Fire3", 2);
    }

    private void UpdateFireMode(string buttonName, int buttonIndex)
    {
        mAttackButtons[buttonIndex] = Input.GetButton(buttonName);

        // Reset other buttons to false if the current button is true
        if (mAttackButtons[buttonIndex])
        {
            for (int i = 0; i < mAttackButtons.Length; i++)
            {
                if (i != buttonIndex)
                {
                    mAttackButtons[i] = false;
                }
            }
        }
    }
}
