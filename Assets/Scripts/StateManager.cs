using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    public Action onGameStateChange;
    public Action onEditStateChange;

    [SerializeField]
    GameObject gridPreviewObj;

    public enum GameStates
    {
        ViewMode,
        EditMode
    }

    public enum EditState
    {
        PlacingFromInvetory,
        MovingObj,
        ChangingColorOfPlacedObj
    }

    public GameStates currentGameState { get; private set; }
    public EditState currentEditState { get; private set; }

    private void Awake()
    {
        instance = this;
        currentGameState = GameStates.ViewMode;
        currentEditState = EditState.PlacingFromInvetory;
        onEditStateChange?.Invoke();
        onGameStateChange?.Invoke();
    }

    public void ChangeGameState(GameStates state)
    {
        currentGameState = state;
        onGameStateChange?.Invoke();
        UpdateGridPreview();
    }

    public void ChangeEditState(EditState state)
    {
        currentEditState = state;
        onEditStateChange?.Invoke();
        UpdateGridPreview();
    }

    private void UpdateGridPreview()
    {
        if(currentGameState == GameStates.EditMode)
        {
            gridPreviewObj.SetActive(true);
        }
        else
        {
            gridPreviewObj.SetActive(false);
        }
    }

}
