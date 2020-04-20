using UnityEngine;

public enum AttackOptionSelected
{
    Attack = 0,
    Magic = 1,
    Guard = 2
}

public class AttackSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform[] ActionOption;

    private AttackOptionSelected optionSelected = AttackOptionSelected.Attack;

    private void Awake()
    {
        GameEvents.OnSelectActionUp += OnUpSelection;
        GameEvents.OnSelectActionDown += OnDownSelection;
        GameEvents.OnConfirmAction += OnConfirmAction;
        
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.OnSelectActionUp -= OnUpSelection;
        GameEvents.OnSelectActionDown -= OnDownSelection;
        GameEvents.OnConfirmAction -= OnConfirmAction;
    }

    private void OnEnable()
    {
        UpdateSelected();
    }

    private void UpdateSelected()
    {
        foreach (var pointer in ActionOption)
        {
            pointer.gameObject.SetActive(false);
        }
        ActionOption[(int)optionSelected].gameObject.SetActive(true);
    }

    private void OnDownSelection()
    {
        var currentSelected = (int) optionSelected;
        currentSelected++;
        if (currentSelected > (int) AttackOptionSelected.Guard)
            currentSelected = 0;
        optionSelected = (AttackOptionSelected)currentSelected;
        UpdateSelected();
    }

    private void OnUpSelection()
    {
        var currentSelected = (int) optionSelected;
        currentSelected--;
        if (currentSelected < 0)
            currentSelected = (int) AttackOptionSelected.Guard;
        optionSelected = (AttackOptionSelected)currentSelected;
        UpdateSelected();
    }

    private void OnConfirmAction()
    {
        GameEvents.OnChooseOption?.Invoke(optionSelected);
    }
}
