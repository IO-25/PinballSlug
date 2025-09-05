using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/RandomAction")]
public class RandomAction : EnemyBehaviour
{
    [SerializeField] EnemyBehaviour[] PossibleActions;

    public override void EnemyAction(Transform t)
    {
        int index = RandomManager.PickOne(PossibleActions.Length-1);
        PossibleActions[index].EnemyAction(t);
    }
}
