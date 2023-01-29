using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    private void Awake() 
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() 
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += GetSelectedUnit;
        UpdateVisual();
    }

    private void GetSelectedUnit()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        meshRenderer.enabled = UnitActionSystem.Instance.GetSelectedUnit() == unit;
    }

    private void OnDestroy() {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= GetSelectedUnit;
    }
}
