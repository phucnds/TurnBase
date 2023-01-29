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

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {

        if (!isActive) return;

        float stoppingDistance = .1f;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;    
        }
        else
        {
            OnStopMoving?.Invoke();
            ActionComplete(); 
        }

        float rotateSpeed = 10f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);
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
                
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPostition gridPostition, Action onActionComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPostition);
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
