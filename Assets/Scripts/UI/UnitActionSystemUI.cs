using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake() 
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UpdateActionPoints;
        TurnSystem.Instance.OnTurnChanged += UpdateActionPoints;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPoints();
       
    }

    private void CreateUnitActionButton()
    {

        foreach(Transform transform in actionButtonContainer)
        {
            Destroy(transform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButton = Instantiate(actionButtonPrefab,actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void OnSelectedUnitChanged()
    {
        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void OnSelectedActionChanged()
    {
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
    
    private void UpdateActionPoints()
    {
        textMeshPro.text = $"Action Points: {UnitActionSystem.Instance.GetSelectedUnit().GetActionPoints()}";
    }

    private void Unit_OnAnyActionPointsChanged()
    {
        UpdateActionPoints();
    }

    
}
