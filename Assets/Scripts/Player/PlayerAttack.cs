using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Weapon currentWepaon;

    private void Update()
    {
        if(Input.GetButton("Fire1"))
            currentWepaon.Fire();
    }

}
