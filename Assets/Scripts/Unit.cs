using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event UnityAction OnAnyActionPointsChanged;

    private GridPostition gridPostition;
    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;
    
    private int actionPoints = ACTION_POINTS_MAX;

    private BaseAction[] baseActionArray;

    [SerializeField] private bool isEnemy;

    private void Awake() {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() 
    {
        gridPostition = LevelGrid.Instance.GetGridPostition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPostition,this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += healthSystem_OnDead;
    }

    private void Update()
    {

        GridPostition newGridPostition = LevelGrid.Instance.GetGridPostition(transform.position);
        if(gridPostition != newGridPostition)
        {
            GridPostition oldGridPosition = gridPostition;
            gridPostition = newGridPostition;
            LevelGrid.Instance.UnitMovedGridPostion(this, oldGridPosition, newGridPostition);
        }
    }

    private void healthSystem_OnDead()
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPostition,this);
        Destroy(gameObject);
    }

    private void TurnSystem_OnTurnChanged()
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke();
        }
        
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPostition GetGridPostition()
    {
        return gridPostition;
    }

    public Vector3 GetWorldPostition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }

        return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointsCost();
    }

    public void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke();
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
}
