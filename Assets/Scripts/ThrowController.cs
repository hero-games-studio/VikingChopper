using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ThrowController : MonoBehaviour
{

    private Animator animator;
    [Header("Public References")]
    public WeaponScript weaponScript;
    public Transform weapon;
    public Transform hand;
    private Vector3 origLocPos;
    private Vector3 origLocRot;
    private Vector3 pullPosition;


    void Start()
    {
        WeaponCatch();
        animator = gameObject.GetComponent<Animator>();
        origLocPos = weapon.localPosition;
        origLocRot = weapon.localEulerAngles;
    }

    public TrailRenderer trailRenderer;

    public void WeaponThrow()
    {
        GameManagerScript.enableTrailRenderer();
        weaponScript.activated = true;
        weapon.parent = null;
        GameManagerScript.hasWeapon = false;
        GameManagerScript.enableSpinning();
        weaponScript.moveWeapon();
    }

    public void WeaponCatch()
    {
        weapon.parent = hand;
        GameManagerScript.disableSpinning();
        GameManagerScript.disableTrailRenderer();
        GameManagerScript.setShowTrajectory(false);
        weapon.localEulerAngles = origLocRot;
        weapon.localPosition = origLocPos;
        GameManagerScript.hasWeapon = true;
    }
    private void checkAnimator()
    {
        if (animator == null)
            animator = gameObject.GetComponent<Animator>();
    }
}
