using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ThrowController : MonoBehaviour
{

    public Animator animator;
    private Rigidbody weaponRb;
    public WeaponScript weaponScript;
    private float returnTime;

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
        weaponRb = weapon.GetComponent<Rigidbody>();
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
        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, 0, .2f), transform.eulerAngles.y, transform.eulerAngles.z);

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

        if (pulling)
        {
            if (returnTime < 1)
            {
                weapon.position = GetQuadraticCurvePoint(returnTime, pullPosition, curvePoint.position, hand.position);
                returnTime += Time.deltaTime;
            }
            else
            {
                WeaponCatch();
            }
        }
    }

    //The position of boomerang hit
    private Vector3 throwForwardVector;

    public TrailRenderer trailRenderer;
    public void WeaponThrow()
    {
        trailRenderer.enabled = true;
        throwForwardVector = gameObject.transform.forward;
        setAim(false);
        hasWeapon = false;
        weaponScript.activated = true;
        weaponRb.isKinematic = false;
        weapon.parent = null;
        weapon.eulerAngles = new Vector3(-90 + transform.eulerAngles.x, 0, 0);
        weapon.transform.position += transform.right / 5;
        weaponScript.activeForce = throwForwardVector * throwPower;
        weaponScript.forwardVector = throwForwardVector;
        weaponRb.AddForce(throwForwardVector * throwPower, ForceMode.Impulse);

    }

    public void WeaponStartPull()
    {
        pullPosition = weapon.position;
        weaponRb.Sleep();
        weaponRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        weaponRb.isKinematic = true;
        weapon.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        weapon.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        weaponScript.activated = true;
        pulling = true;
    }

    public void WeaponCatch()
    {
        trailRenderer.enabled = false;
        returnTime = 0;
        pulling = false;
        weapon.parent = hand;
        weaponScript.activated = false;
        weapon.localEulerAngles = origLocRot;
        weapon.localPosition = origLocPos;
        hasWeapon = true;
    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}
