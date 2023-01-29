using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private int gCost;
    private int hCost;
    private int fCost;

    private GridPostition gridPostition;
    private PathNode cameFromPathNode;
    


    public PathNode(GridPostition gridPostition)
    {
        this.gridPostition = gridPostition;
    }

    public override string ToString()
    {
        string unitString = "";
        return gridPostition.ToString() + "\n" + unitString;
    }
}
