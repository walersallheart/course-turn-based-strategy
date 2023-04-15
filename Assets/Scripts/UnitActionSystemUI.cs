using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    

    private List<ActionButtonUI> actionButtonList;

    private void Awake() {
        actionButtonList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnOnActionStarted;
        
        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainerTransform) {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnOnActionStarted(object sender, EventArgs e) {
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual() {
        foreach (ActionButtonUI action in actionButtonList)
        {
            action.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints() {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }
}
