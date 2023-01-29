using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugPrefab;

    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        gridSystem = new GridSystem<PathNode>(10,10,2f,(GridSystem<PathNode> g, GridPostition gridPostition) => new PathNode(gridPostition));
        gridSystem.CreateDebugObjects(gridDebugPrefab);
    }

    
}
