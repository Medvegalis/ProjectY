using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TooltipManager : MonoBehaviour
{
    [SerializeField]
    PlayerInputControler playerInputControler;

    [SerializeField]
    TextMeshProUGUI keybindTooltipText;
    [SerializeField]
    TextMeshProUGUI objectStatusText;

    void Start()
    {
        StateManager.instance.onGameStateChange += HandleGameStateChange;
        StateManager.instance.onEditStateChange += HandleEditStateChange;
    }

    void Update()
    {

    }

    private void HandleGameStateChange()
    {
        switch (StateManager.instance.currentGameState)
        {
            case StateManager.GameStates.ViewMode:
                keybindTooltipText.text = $"{playerInputControler.enterEditMode.bindings[0].ToDisplayString()} - Enter/exit edit mode\n" +
                                           $"{playerInputControler.openInventory.bindings[0].ToDisplayString()} - Invetory\n";
                objectStatusText.text = $" ";
                break;
            case StateManager.GameStates.EditMode:
                keybindTooltipText.text = $"{playerInputControler.rotate.bindings[0].ToDisplayString()} - rotate\n" +
                                            $"{playerInputControler.place.bindings[0].ToDisplayString()} - place\n" +
                                            $"{playerInputControler.editObj.bindings[0].ToDisplayString()} - editObj\n" +
                                            $"{playerInputControler.ignoreGrid.bindings[0].ToDisplayString()} - freeMode";
                objectStatusText.text = $"Placing new object";
                break;
        }
    }

    private void HandleEditStateChange()
    {
        switch (StateManager.instance.currentEditState)
        {
            case StateManager.EditState.PlacingFromInvetory:
                objectStatusText.text = $"Placing new object";

                keybindTooltipText.text = $"{playerInputControler.rotate.bindings[0].ToDisplayString()} - rotate\n" +
                                            $"{playerInputControler.place.bindings[0].ToDisplayString()} - place\n" +
                                            $"{playerInputControler.editObj.bindings[0].ToDisplayString()} - editObj\n" +
                                            $"{playerInputControler.ignoreGrid.bindings[0].ToDisplayString()} - freeMode";
                break;
            case StateManager.EditState.MovingObj:
                objectStatusText.text = $"Object selected";

                keybindTooltipText.text = $"{playerInputControler.place.bindings[0].ToDisplayString()} - place\n" +
                                        $"{playerInputControler.editObj.bindings[0].ToDisplayString()} - cancel\n" +
                                        $"{playerInputControler.rotate.bindings[0].ToDisplayString()} - rotate\n" +
                                        $"{playerInputControler.editColor.bindings[0].ToDisplayString()} - change color\n" +
                                        $"{playerInputControler.deleteObj.bindings[0].ToDisplayString()} - delete\n" +
                                        $"{playerInputControler.ignoreGrid.bindings[0].ToDisplayString()} - freeMode";
                break;
        }
    }
}
