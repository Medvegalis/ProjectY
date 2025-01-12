using UnityEngine;

public interface IDataSaver 
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
    void NewGame();
}
