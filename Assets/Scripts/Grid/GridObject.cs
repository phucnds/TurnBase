using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    private GridSystem<GridObject> gridSystem;
    private GridPostition gridPostition;
    private List<Unit> units;

    public GridObject(GridSystem<GridObject> gridSystem, GridPostition gridPostition)
    {
        this.gridSystem = gridSystem;
        this.gridPostition = gridPostition;
        units = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";

        foreach (Unit unit in units)
        {
            unitString += unit + "\n";
        }
        return gridPostition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        this.units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        this.units.Remove(unit);
    }

    public List<Unit> GetListUnit()
    {
        return units;
    }

    public bool HasAnyUnit()
    {
        return units.Count > 0;
    }

    public Unit GetUnit()
    {
        if(HasAnyUnit())
        {
            return units[0];
        }
        else
        {
            return null;
        }
    }
}
