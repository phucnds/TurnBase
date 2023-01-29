using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseAction : MonoBehaviour
{
    public static event UnityAction<BaseAction> OnAnyActionOnStarted;
    public static event UnityAction<BaseAction> OnAnyActionOnCompleted;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake() {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPostition gridPostition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPostition gridPostition)
    {
        List<GridPostition> validActionGridPosition = GetValidActionGridPositionList();
        return validActionGridPosition.Contains(gridPostition);
    }

    public abstract List<GridPostition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionOnStarted?.Invoke(this);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnAnyActionOnCompleted?.Invoke(this);
    }

    public Unit GetUnit()
    {
        return unit;
    }
}
