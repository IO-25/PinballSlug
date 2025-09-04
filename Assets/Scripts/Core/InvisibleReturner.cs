using UnityEngine;

public class InvisibleReturner : MonoBehaviour
{
    [SerializeField] private GameObject returnTarget;

    private void OnBecameInvisible()
        => ObjectPoolingManager.Instance.Return(returnTarget);
}
