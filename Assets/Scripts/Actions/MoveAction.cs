using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveAction : BaseAction
{

    [SerializeField] private int maxMoveDistance = 2;

    public event UnityAction OnStartMoving;
    public event UnityAction OnStopMoving;

    private List<Vector3> positionList;
    private int currentPositionIndex;


    private void Update()
    {

        if (!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;

            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke();
                ActionComplete();
            }
        }
    }




    public override List<GridPostition> GetValidActionGridPositionList()
    {
        List<GridPostition> validGridPositionList = new List<GridPostition>();

        GridPostition unitGridPosition = unit.GetGridPostition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPostition offsetGridPostition = new GridPostition(x, z);
                GridPostition testGridPosition = unitGridPosition + offsetGridPostition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (unitGridPosition == testGridPosition) continue;
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) continue;
                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition)) continue;

                int pathfindingDistanceMultiplier = 10;
                if(Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier) continue;
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPostition gridPostition, Action onActionComplete)
    {
        List<GridPostition> pathGridPostitionList = Pathfinding.Instance.FindPath(unit.GetGridPostition(), gridPostition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPostition pathGridPosition in pathGridPostitionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke();
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPostition gridPostition)
    {

        int targetCountAtPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPostition);

        return new EnemyAIAction
        {
            gridPostition = gridPostition,
            actionValue = targetCountAtPosition * 10
        };
    }
}
