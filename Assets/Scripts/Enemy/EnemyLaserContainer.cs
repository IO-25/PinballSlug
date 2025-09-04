using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserContainer : MonoBehaviour
{
    [SerializeField] EnemyLaser enemyLaser;
    [SerializeField] Animator warningAnimator;
    public float warningDuration;

    private void Start()
    {
        enemyLaser.gameObject.SetActive(false);

        warningAnimator.GetComponent<LineRenderer>().SetPosition(0, transform.parent.position + new Vector3(-20.0f, 0,0));
        warningAnimator.GetComponent<LineRenderer>().SetPosition(1, transform.parent.position + new Vector3(20.0f, 0, 0));
        Invoke("OnWarningEnd", warningDuration);
    }

    private void OnWarningEnd()
    {
        warningAnimator.SetBool("IsWarningEnd", true);
        warningAnimator.gameObject.SetActive(false);
        enemyLaser.gameObject.SetActive(true);
    }
}
