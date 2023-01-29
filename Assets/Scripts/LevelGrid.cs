using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2f;

    public static LevelGrid Instance { get; private set; }

    public event UnityAction OnAnyUnitMovedGridPosition;

    [SerializeField] private Transform gridDebugPrefab;
    private GridSystem<GridObject> gridSystem;



    private void Awake()
    {
        Instance = this;
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPostition gridPostition) => new GridObject( g, gridPostition));
        //gridSystem.CreateDebugObjects(gridDebugPrefab);
    }

    public void AddUnitAtGridPosition(GridPostition gridPostition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetListUnitAtGridPosition(GridPostition gridPostition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        return gridObject.GetListUnit();
    }

    public void RemoveUnitAtGridPosition(GridPostition gridPostition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPostion(Unit unit, GridPostition fromGridPosition, GridPostition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition?.Invoke();
    }

    public GridPostition GetGridPostition(Vector3 worldPosition) => gridSystem.GetGridPostition(worldPosition);

    public Vector3 GetWorldPosition(GridPostition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPostition gridPostition) => gridSystem.IsValidGridPosition(gridPostition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyUnitOnGridPosition(GridPostition gridPostition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPostition gridPostition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        return gridObject.GetUnit();
    }
}
