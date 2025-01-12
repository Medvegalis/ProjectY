using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : MonoBehaviour
{
    //[ Header("Menu Navigation")]
    //[SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    //[Header("Confirmation Popup")]
    //[SerializeField] private ConfirmationPopupMenu confirmationPopupMenu;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    private void OnEnable()
    {
        ActivateMenu();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
      
        // case - new game, but the save slot has data
        if (saveSlot.hasData)
        {
            SaveGame(saveSlot.GetProfileId());
        }
        // case - new game, and the save slot has no data
        else
        {
            SaveGame(saveSlot.GetProfileId());
        }
    }

    public void OnLoadSlotClicked(SaveSlot saveSlot)
    {
       
        // case - new game, but the save slot has data
        if (saveSlot.hasData)
        {
            GameDataPersistenceManager.instance.NewGame();
            LoadGame(saveSlot.GetProfileId());
        }
        // case - new game, and the save slot has no data
        else
        {
            // GameDataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            GameDataPersistenceManager.instance.NewGame();
            SaveGame(saveSlot.GetProfileId());
        }
    }

    private void LoadGame(string profileId)
    {
        GameDataPersistenceManager.instance.LoadGame(profileId);
    }
    private void SaveGame(string profileId)
    {
        GameDataPersistenceManager.instance.SaveGame(profileId);
    }
    public void OnClearClicked(SaveSlot saveSlot)
    {
        GameDataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
        ActivateMenu();
    }


    public void ActivateMenu()
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = GameDataPersistenceManager.instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
