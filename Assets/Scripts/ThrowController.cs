using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ThrowController : MonoBehaviour
{

    public Animator animator;
    public WeaponScript weaponScript;

    private Vector3 origLocPos;
    private Vector3 origLocRot;
    private Vector3 pullPosition;

    [Header("Public References")]
    public Transform weapon;
    public Transform hand;
    public Transform curvePoint;
    [Space]
    [Header("Parameters")]
    public float throwPower = 150;
    [Space]
    [Header("Bools")]
    public bool hasWeapon = true;
    public static bool pulling = false;
    [HideInInspector]

    void Start()
    {
        WeaponCatch();
        weaponScript.throwPower = throwPower;
        origLocPos = weapon.localPosition;
        origLocRot = weapon.localEulerAngles;
    }

    public void setAim(bool isAiminig)
    {
        animator.SetBool("aiming", isAiminig);
    }
    void FixedUpdate()
    {
         //Animation States
        animator.SetBool("pulling", pulling);

        if (hasWeapon)
        {

            if (GameManagerScript.checkTapping())
            {
                setAim(true);
            }

        }
        else
        {
            if (GameManagerScript.checkTapping() && !pulling)
            {
                WeaponStartPull();
            }
        }
    }

    //The position of boomerang hit
    private Vector3 throwForwardVector;

    public TrailRenderer trailRenderer;

    public void WeaponThrow(){
        trailRenderer.enabled=true;
        setAim(false);
        hasWeapon=false;
        weaponScript.enabled=true;
        weaponScript.moveWeapon();
    }

    public void WeaponStartPull()
    {
        pullPosition = weapon.position;
        weapon.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        weapon.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        weaponScript.activated = true;
        pulling = true;
    }

    public void WeaponCatch()
    {
        GameManagerScript.changeShowTrajectory(true);
        trailRenderer.enabled = false;
        pulling = false;
        weaponScript.activated = false;
        weapon.localEulerAngles = origLocRot;
        weapon.localPosition = origLocPos;
        hasWeapon = true;
    }
}
