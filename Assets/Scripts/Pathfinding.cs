using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugPrefab;

    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        Instance = this;
        gridSystem = new GridSystem<PathNode>(10, 10, 2f, (GridSystem<PathNode> g, GridPostition gridPostition) => new PathNode(gridPostition));
        gridSystem.CreateDebugObjects(gridDebugPrefab);
    }

    public List<GridPostition> FindPath(GridPostition startGridPosition, GridPostition endGridPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPostition gridPostition = new GridPostition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPostition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if(closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(),neighbourNode.GetGridPosition());   
                if(tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endNode.GetGridPosition()));
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }

        }

        return null;
    }

    private List<GridPostition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();

        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;

        while(currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPostition> gridPostitionList = new List<GridPostition>();

        foreach (PathNode pathNode in pathNodeList)
        {
            gridPostitionList.Add(pathNode.GetGridPosition());
        }

        return gridPostitionList;
    }

    public int CalculateDistance(GridPostition gridPostitionA, GridPostition gridPostitionB)
    {
        GridPostition gridPostitionDistance = gridPostitionA - gridPostitionB;
        int xDistance = Mathf.Abs(gridPostitionDistance.x);
        int zDistance = Mathf.Abs(gridPostitionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPostition(x, z));

    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPostition gridPostition = currentNode.GetGridPosition();

        /*
        bool LEFT,RIGHT,UP,DOWN;

        LEFT  = gridPostition.x - 1 >= 0;
        RIGHT = gridPostition.x + 1 <= gridSystem.GetWidth();
        UP    = gridPostition.z <= gridSystem.GetHeight();
        DOWN  = gridPostition.z - 1 >= 0;

        if(LEFT)  neighbourList.Add(GetNode(gridPostition.x - 1, gridPostition.z + 0));
        if(RIGHT) neighbourList.Add(GetNode(gridPostition.x + 1, gridPostition.z + 0));
        if(UP)    neighbourList.Add(GetNode(gridPostition.x + 0, gridPostition.z + 1));
        if(DOWN)  neighbourList.Add(GetNode(gridPostition.x + 0, gridPostition.z - 1));

        if(LEFT && DOWN)  neighbourList.Add(GetNode(gridPostition.x - 1, gridPostition.z - 1));
        if(LEFT && UP)    neighbourList.Add(GetNode(gridPostition.x - 1, gridPostition.z + 1));
        if(RIGHT && DOWN) neighbourList.Add(GetNode(gridPostition.x + 1, gridPostition.z - 1));
        if(RIGHT && UP)   neighbourList.Add(GetNode(gridPostition.x + 1, gridPostition.z + 1));
        */


        
        if(gridPostition.x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(gridPostition.x - 1, gridPostition.z + 0));

            if(gridPostition.z - 1 >= 0)
            {
                //Left Down
                neighbourList.Add(GetNode(gridPostition.x - 1, gridPostition.z - 1));
            }
            
            if(gridPostition.z <= gridSystem.GetHeight())
            {
                //Left Up
                neighbourList.Add(GetNode(gridPostition.x - 1, gridPostition.z + 1));
            }

            
        }
        
        if(gridPostition.x + 1 <= gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(gridPostition.x + 1, gridPostition.z + 0));

            if(gridPostition.z - 1 >=0)
            {
                //Right Down
                neighbourList.Add(GetNode(gridPostition.x + 1, gridPostition.z - 1));
            }

            if (gridPostition.z <= gridSystem.GetHeight())
            {
                //Right Up
                neighbourList.Add(GetNode(gridPostition.x + 1, gridPostition.z + 1));
            }
            
        }

        if (gridPostition.z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(gridPostition.x + 0, gridPostition.z - 1));
        }

        if (gridPostition.z <= gridSystem.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(gridPostition.x + 0, gridPostition.z + 1));
        }
        

        return neighbourList;
    }

}
