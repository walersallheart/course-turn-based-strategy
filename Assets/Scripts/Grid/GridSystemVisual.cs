using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    public enum GridVisualType {
        White, 
        Blue,
        Red,
        Yellow
    }

    private void Start() {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++) {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        UpdateGridVisual();
    }

    public void HideAllGridPositions() {
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++) {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++) {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList) {
        foreach(GridPosition gridPosition in gridPositionList) {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual() {
        HideAllGridPositions();

        BaseAction selectedUnit = UnitActionSystem.Instance.GetSelectedAction();

        ShowGridPositionList(selectedUnit.GetValidActionGridPositionList());
    }
    
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) {
        UpdateGridVisual();
    }
}
