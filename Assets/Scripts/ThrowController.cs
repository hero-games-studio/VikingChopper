using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ThrowController : MonoBehaviour
{

    [Header("Public References")]
    public Animator animator;
    public WeaponScript weaponScript;
    public Transform weapon;
    public Transform hand;
    private Vector3 origLocPos;
    private Vector3 origLocRot;
    private Vector3 pullPosition;


    void Start()
    {
        WeaponCatch();
        origLocPos = weapon.localPosition;
        origLocRot = weapon.localEulerAngles;
    }

    public void setAim(bool isAiminig)
    {
        animator.SetBool("aiming", isAiminig);
    }
    public void WeapoonPulling()
    {
        setPulling(true);
    }

    public TrailRenderer trailRenderer;

    public void WeaponThrow()
    {
        GameManagerScript.enableTrailRenderer();
        weaponScript.activated = true;
        weapon.parent = null;
        setAim(false);
        GameManagerScript.hasWeapon = false;
        GameManagerScript.enableSpinning();
        weaponScript.moveWeapon();
    }

    public void WeaponCatch()
    {
        weapon.parent = hand;
        GameManagerScript.disableSpinning();
        GameManagerScript.disableTrailRenderer();
        weapon.localEulerAngles = origLocRot;
        weapon.localPosition = origLocPos;
        setPulling(false);
        GameManagerScript.hasWeapon = true;
    }

    private void setPulling(bool val)
    {
        animator.SetBool("pulling", val);
    }
}
