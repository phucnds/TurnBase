using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyDetroyed += DestructibleCrate_OnAnyDetroyed;
    }

    private void DestructibleCrate_OnAnyDetroyed(DestructibleCrate destructibleCrate)
    {
        Pathfinding.Instance.SetWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
