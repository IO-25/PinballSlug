using UnityEngine;

public class InvisibleReturner : MonoBehaviour
{
    [SerializeField] private GameObject returnTarget;

    private void OnBecameInvisible()
    {
        if (ObjectPoolingManager.Instance == null) return;
        ObjectPoolingManager.Instance.Return(returnTarget);
    }
}
