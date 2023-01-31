using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleCrate : MonoBehaviour
{
    public static event UnityAction<DestructibleCrate> OnAnyDetroyed;

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
        Destroy(gameObject);
        OnAnyDetroyed?.Invoke(this);
    }
}
