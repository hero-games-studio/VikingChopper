﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;

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
    [Space]
    [Header("Parameters")]
    public float throwPower = 150;
    public float cameraZoomOffset = .3f;
    [Space]
    [Header("Bools")]
    public bool hasWeapon = true;
    public bool pulling = false;
    [Space]
    [Header("Particles and Trails")]
    public ParticleSystem catchParticle;
    public ParticleSystem trailParticle;
    public TrailRenderer trailRenderer;
    [Space]
    [Header("UI")]
    public Image reticle;

    [Space]
    //Cinemachine Shake
    public CinemachineFreeLook virtualCamera;
    public CinemachineImpulseSource impulseSource;

    void Start()
    {
        Cursor.visible = false;

        animator = GetComponent<Animator>();
        weaponRb = weapon.GetComponent<Rigidbody>();
        weaponScript = weapon.GetComponent<WeaponScript>();
        origLocPos = weapon.localPosition;
        origLocRot = weapon.localEulerAngles;
        reticle.DOFade(0, 0);

    }

    void Update()
    {
        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, 0, .2f), transform.eulerAngles.y, transform.eulerAngles.z);

        //Animation States
        animator.SetBool("pulling", pulling);

        if (hasWeapon)
        {

            if (Input.GetMouseButtonDown(0))
            {
                WeaponThrow();
                animator.SetTrigger("throw");
            }

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                WeaponStartPull();
            }
        }

        if (pulling)
        {
            if (returnTime < 1)
            {
                weapon.position = GetQuadraticCurvePoint(returnTime, pullPosition, curvePoint.position, hand.position);
                returnTime += Time.deltaTime * 1.5f;
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

    public void WeaponThrow()
    {

        hasWeapon = false;
        weaponScript.activated = true;
        weaponRb.isKinematic = false;
        weaponRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        weapon.parent = null;
        weapon.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
        weapon.transform.position += transform.right / 5;
        weaponRb.AddForce(Camera.main.transform.forward * throwPower + transform.up * 30, ForceMode.Impulse);

        //Trail
        trailRenderer.emitting = true;
        trailParticle.Play();
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

        //Shake
        impulseSource.GenerateImpulse(Vector3.right);

    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    void CameraOffset(float offset)
    {
        virtualCamera.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
        virtualCamera.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
        virtualCamera.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
    }
}
