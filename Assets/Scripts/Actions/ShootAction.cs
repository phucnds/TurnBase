using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootAction : BaseAction
{

    public event EventHandler<OnShootEventArgs> onShoot;

    public class OnShootEventArgs: EventArgs
    {
        public Unit targetUnit;
        public Unit shootUnit;
    }

    private enum State
    {
        Amiming,
        Shooting,
        Cooloff,
    }

    private State state;
    private int maxShootDistance = 7;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update() 
    {
        if(!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Amiming:
                Vector3 aimDir = (targetUnit.GetWorldPostition() - unit.GetWorldPostition()).normalized;

                float rotateSpeed = 10f;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDir), rotateSpeed * Time.deltaTime);
                break;
            case State.Shooting:
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        onShoot?.Invoke(this, new OnShootEventArgs{
            targetUnit = this.targetUnit,
            shootUnit = this.unit
        });

        targetUnit.Damage(40);
    }

    private void NextState()
    {
        switch(state)
        {
            case State.Amiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float cooloffStateTime = 0.5f;
                stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }


    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPostition> GetValidActionGridPositionList()
    {
        GridPostition unitGridPosition = unit.GetGridPostition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPostition> GetValidActionGridPositionList(GridPostition unitGridPosition)
    {
        List<GridPostition> validGridPositionList = new List<GridPostition>();

        

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPostition offsetGridPostition = new GridPostition(x, z);
                GridPostition testGridPosition = unitGridPosition + offsetGridPostition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootDistance) continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPostition gridPostition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPostition);
       
        state = State.Amiming;
        float amimingStateTime = 1f;
        stateTimer = amimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPostition gridPostition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPostition);


        return new EnemyAIAction
        {
            gridPostition = gridPostition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f)
        };
    }

    public int GetTargetCountAtPosition(GridPostition gridPostition)
    {
        return GetValidActionGridPositionList(gridPostition).Count;
    }
}
