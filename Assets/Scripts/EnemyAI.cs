using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch(state)
        {
            case State.WaitingForEnemyTurn:
                break;

            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    
                    if(TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                    
                }
                break;

            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged()
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
        
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
                return true;
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit,Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPostition actionGridPostition = enemyUnit.GetGridPostition();

        if (!spinAction.IsValidActionGridPosition(actionGridPostition)) return false;
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) return false;
        
        spinAction.TakeAction(actionGridPostition, onEnemyAIActionComplete);
        return true;
        
    }
}
