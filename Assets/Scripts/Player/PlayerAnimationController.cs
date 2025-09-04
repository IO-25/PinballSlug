using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animators")]
    public Animator upperBodyAnimator;
    public Animator lowerBodyAnimator;

    public void Initialize()
    {
        SetAnimSpeed(1f);
        SetBool("IsDying", false);
        SetBool("IsMoving", false);
        SetBool("IsJumping", false);
        SetBool("IsSitting", false);
        SetBool("IsShooting", false);
        SetBool("IsDropping", false);
        SetBool("IsDying", false);
        SetFloat_Upper("Y", 0f);
    }

    public void SetAnimController(RuntimeAnimatorController upper, RuntimeAnimatorController lower)
    {
        upperBodyAnimator.runtimeAnimatorController = upper;
        lowerBodyAnimator.runtimeAnimatorController = lower;
    }

    public void SetFloat_Upper(string param, float value)
    {
        upperBodyAnimator.SetFloat(param, value);
    }

    public void SetBool(string param, bool value)
    {
        upperBodyAnimator.SetBool(param, value);
        lowerBodyAnimator.SetBool(param, value);
    }
    public void SetTrigger(string param)
    {
        upperBodyAnimator.SetTrigger(param);
        lowerBodyAnimator.SetTrigger(param);
    }

    public void SetBool_Upper(string param, bool value)
    {
        upperBodyAnimator.SetBool(param, value);
    }

    public void SetBool_Lower(string param, bool value)
    {
        lowerBodyAnimator.SetBool(param, value);
    }

    public void SetAnimSpeed(float speed)
    {
        upperBodyAnimator.SetFloat("Speed", speed);
        lowerBodyAnimator.SetFloat("Speed", speed);
    }

    public void SetUpperActive(bool active)
    {
        upperBodyAnimator.gameObject.SetActive(active);
    }

    public void SetLowerActive(bool active) {
        lowerBodyAnimator.gameObject.SetActive(active);
    }
}
