using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleCrate : MonoBehaviour
{
    public static event UnityAction<DestructibleCrate> OnAnyDetroyed;

    [SerializeField] private Transform crateDetroyedPrefabs;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDetroyedPrefabs, transform.position, transform.rotation);
        
        Destroy(gameObject);
        ApplyExplosionToChildren(crateDestroyedTransform, 200f, transform.position, 10f);
        OnAnyDetroyed?.Invoke(this);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
