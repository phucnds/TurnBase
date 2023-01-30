using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow,

    }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;

    private GridSystenVisualSingle[,] gridSystenVisualSingleArray;



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gridSystenVisualSingleArray = new GridSystenVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPostition gridPostition = new GridPostition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPostition), Quaternion.identity);
                gridSystenVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystenVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        UpdateVisual();
    }



    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystenVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionRange(GridPostition gridPostition, int range, GridVisualType gridVisualType)
    {
        List<GridPostition> gridPostitionList = new List<GridPostition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPostition testGridPosition = gridPostition + new GridPostition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range) continue;

                gridPostitionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPostitionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPostition> gridPostitionsList, GridVisualType gridVisualType)
    {
        Material material = GetGridVisualTypeMaterial(gridVisualType);

        foreach (GridPostition gridPostition in gridPostitionsList)
        {
            gridSystenVisualSingleArray[gridPostition.x, gridPostition.z].Show(material);
        }
    }

    private void UpdateVisual()
    {
        HideAllGridPosition();
        BaseAction action = UnitActionSystem.Instance.GetSelectedAction();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        GridVisualType gridVisualType;

        switch (action)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPostition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
        }

        ShowGridPositionList(action.GetValidActionGridPositionList(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged()
    {
        UpdateVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition()
    {
        UpdateVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterials)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        return null;
    }

}
