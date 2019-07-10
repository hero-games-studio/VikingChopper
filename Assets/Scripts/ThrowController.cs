using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ThrowController : MonoBehaviour
{

    private Animator animator;
    private Rigidbody weaponRb;
    private WeaponScript weaponScript;
    private float returnTime;

    private Vector3 origLocPos;
    private Vector3 origLocRot;
    private Vector3 pullPosition;

    [Header("Public References")]
    public Transform weapon;
    public Transform hand;
    public Transform curvePoint;
    private Vector3 originCurvePoint;
    [Space]
    [Header("Parameters")]
    public float throwPower = 150;
    [Space]
    [Header("Bools")]
    public bool hasWeapon = true;
    public bool pulling = false;
    [Space]
    [Header("Particles and Trails")]
    public ParticleSystem catchParticle;
    public ParticleSystem trailParticle;
    public TrailRenderer trailRenderer;

    void Start()
    {
        Cursor.visible = false;

        animator = GetComponent<Animator>();
        weaponRb = weapon.GetComponent<Rigidbody>();
        weaponScript = weapon.GetComponent<WeaponScript>();
        weaponScript.throwPower = throwPower;
        origLocPos = weapon.localPosition;
        origLocRot = weapon.localEulerAngles;
        originCurvePoint = curvePoint.localPosition;
    }

    public void setAim(bool isAiminig)
    {
        animator.SetBool("aiming", isAiminig);
    }
    void Update()
    {
        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, 0, .2f), transform.eulerAngles.y, transform.eulerAngles.z);

        //Animation States
        animator.SetBool("pulling", pulling);

        if (hasWeapon)
        {

            if (GameManagerScript.checkTapping())
            {
                setAim(true);
                setCurvePoint();
            }

        }
        else
        {
            if (GameManagerScript.checkTapping() && !pulling)
            {
                WeaponStartPull();
                setCurvePoint();
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }

    private void setCurvePoint()
    {
        if (gameObject.transform.rotation.y > CameraMovementScript.currentRotation)
        {
            curvePoint.eulerAngles = originCurvePoint;
        }
        else
        {
            curvePoint.eulerAngles = new Vector3(originCurvePoint.x, -originCurvePoint.y, originCurvePoint.z);
        }

    }

    public void WeaponThrow()
    {
        setAim(false);
        hasWeapon = false;
        weaponScript.activated = true;
        weaponRb.isKinematic = false;
        weaponRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        weaponRb.interpolation = RigidbodyInterpolation.Interpolate;
        weapon.parent = null;
        weapon.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
        weapon.transform.position += transform.right / 5;
        weaponRb.useGravity = false;
        weaponScript.activeForce = gameObject.transform.forward * throwPower;
        weaponScript.forwardVector = gameObject.transform.forward;
        weaponRb.AddForce(gameObject.transform.forward * throwPower, ForceMode.Impulse);

        //Trail
        trailRenderer.emitting = true;
        trailParticle.Play();
    }

    public void WeaponStartPull()
    {
        pullPosition = weapon.position;
        weaponRb.Sleep();
        weaponRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        weaponRb.isKinematic = true;
        weapon.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        weapon.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        weaponScript.activated = true;
        pulling = true;
    }

    public void WeaponCatch()
    {
        returnTime = 0;
        pulling = false;
        weapon.parent = hand;
        weaponScript.activated = false;
        weapon.localEulerAngles = origLocRot;
        weapon.localPosition = origLocPos;
        hasWeapon = true;

        //Particle and trail
        catchParticle.Play();
        trailRenderer.emitting = false;
        trailParticle.Stop();
    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}
