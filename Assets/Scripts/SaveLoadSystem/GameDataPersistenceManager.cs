using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System;

public class GameDataPersistenceManager : MonoBehaviour
{
    public static GameDataPersistenceManager instance { get; private set; }

    [SerializeField]
    private string fileName;

    private SaveFileHandler saveFileHandler;

    private string selectedProfileId = "";

    [SerializeField]
    private GameData gameData;

    private List<IDataSaver> dataSaverObjectlist;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("Other GameDataPersistenceManager detected");
    }

    private void Start()
    {
        saveFileHandler = new SaveFileHandler(Application.persistentDataPath, fileName); 
        dataSaverObjectlist = FindAllDataSaverObjects();
       // LoadGame();
    }

    private List<IDataSaver> FindAllDataSaverObjects()
    {
        IEnumerable<IDataSaver> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataSaver>();

        return new List<IDataSaver>(dataPersistenceObjects);
    }

    public void SaveGame(string profileId)
    {
        gameData = new GameData();
        foreach (IDataSaver data in dataSaverObjectlist)
        {
            data.SaveData(ref gameData);
        }

        Debug.Log($"Saved {gameData.placedFurnitureData.Count} furniture objects");
        saveFileHandler.Save(gameData, profileId);
    }

    public void LoadGame(string profileId)
    {
        gameData = saveFileHandler.Load(profileId);

        if(gameData == null)
        {
            Debug.Log("No data to load found");
            NewGame();
        }

        foreach(IDataSaver data in dataSaverObjectlist)
        {
            data.LoadData(gameData);
        }

        Debug.Log($"Loaded {gameData.placedFurnitureData.Count} furniture objects");
    }
    public void NewGame()
    {
        gameData = new GameData();
        foreach (IDataSaver data in dataSaverObjectlist)
        {
            data.NewGame();
        }
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return saveFileHandler.LoadAllProfiles();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our game data accordingly
        LoadGame(newProfileId);
    }

    public void DeleteProfileData(string profileId)
    {
        // delete the data for this profile id
        saveFileHandler.Delete(profileId);
        // initialize the selected profile id
        InitializeSelectedProfileId();
        // reload the game so that our data matches the newly selected profile id
        LoadGame(profileId);
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = saveFileHandler.GetMostRecentlyUpdatedProfileId();

        //if (overrideSelectedProfileId)
        //{
        //    this.selectedProfileId = testSelectedProfileId;
        //    Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        //}
    }
}
