using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            GridPostition mouseGridPosition = LevelGrid.Instance.GetGridPostition(MouseWorld.GetPosition());
            GridPostition startGridPosition = new GridPostition(0, 0);

            List<GridPostition> gridPostitionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            for (int i = 0; i < gridPostitionList.Count - 1; i++)
            {
                Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(gridPostitionList[i]), LevelGrid.Instance.GetWorldPosition(gridPostitionList[i + 1]), Color.white, 10f);
            }
        }
    }
}
