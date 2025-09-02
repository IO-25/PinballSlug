using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/RandomAction")]
public class RandomAction : EnemyBehaviour
{
    [SerializeField] EnemyBehaviour[] PossibleActions;
    float[] Probability;

    public override void EnemyAction(Transform t)
    {
        if (Probability.Length == 0)
        {
            Probability = new float[PossibleActions.Length];
            for (int i = 0; i < PossibleActions.Length; i++)
                Probability[i] = 1.0f / PossibleActions.Length;
        }
        int index = RandomManager.RandomPicker(Probability);
        PossibleActions[index].EnemyAction(t);
    }
}
