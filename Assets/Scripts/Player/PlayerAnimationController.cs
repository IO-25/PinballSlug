using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animators")]
    public Animator upperBodyAnimator;
    public Animator lowerBodyAnimator;

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
}
