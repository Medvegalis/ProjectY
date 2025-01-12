using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacedFurnitureTracker : MonoBehaviour, IDataSaver
{
    public static PlacedFurnitureTracker instance;

    public Dictionary<GameObject, FurnitureData> placedFurnitureList { get; private set; }

    [SerializeField]
    private List<FurnitureHolderSO> furnitureSetList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            placedFurnitureList = new Dictionary<GameObject,FurnitureData>();

            //TODO better solutions
            //Sets catagory for each furnitureDataSO acording to what category it is in
            for (int i = 0; i < furnitureSetList.Count; i++)
            {
                for (int j = 0; j < furnitureSetList[i].furnitureDataList.Count; j++)
                {
                    furnitureSetList[i].furnitureDataList[j].categoryId = i;
                    furnitureSetList[i].furnitureDataList[j].id = j;
                }
            }
        }
        else
            Debug.Log("Other PlacedFurnitureTracker detected");
    }

    public void UpdateFurnitureToList(GameObject obj, int categoryIndex, int furnitureIndex)
    {
        if (obj == null)
        {
            Debug.LogError("Cannot add/update null GameObject to the furniture list.");
            return;
        }

        ColorChanger colorChanger = obj.GetComponent<ColorChanger>();
        if (colorChanger == null)
        {
            Debug.LogError($"No ColorChanger component found on {obj.name}. Cannot update furniture data.");
            return;
        }

        if (placedFurnitureList.ContainsKey(obj))
        {
            placedFurnitureList[obj] = new FurnitureData(
                categoryIndex,
                furnitureIndex,
                colorChanger.currentColor,
                obj.transform.position,
                obj.transform.rotation.eulerAngles
            );
            Debug.Log($"Updated: {obj.name} in the list.");
        }
        else
        {
            placedFurnitureList.Add(obj, new FurnitureData(
                categoryIndex,
                furnitureIndex,
                colorChanger.currentColor,
                obj.transform.position,
                obj.transform.rotation.eulerAngles
            ));
            Debug.Log($"Added: {obj.name} to the list.");
        }

        Debug.Log($"with Color: {colorChanger.currentColor}.");
    }

    public void RemoveFurnitureFromList(GameObject obj)
    {
        if (placedFurnitureList.ContainsKey(obj))
        {
            placedFurnitureList.Remove(obj);
            Debug.Log($"Removed: {obj.name}");
        }
    }

    public void LoadData(GameData data)
    {
        foreach(FurnitureData fData in data.placedFurnitureData)
        {
            GameObject spawnedFurnitureObj = SpawnFurnitureObj(fData);

            if(fData != null)
            {
                placedFurnitureList.Add(spawnedFurnitureObj, fData);

            }
        }
        
    }

    public void SaveData(ref GameData data)
    {
        foreach (KeyValuePair<GameObject, FurnitureData> kvp in placedFurnitureList)
        {
            data.placedFurnitureData.Add(kvp.Value);
        }
    }

    public void NewGame()
    {
        if(placedFurnitureList.Count > 0)
        {
            foreach (KeyValuePair<GameObject, FurnitureData> kvp in placedFurnitureList)
            {
                Destroy(kvp.Key);
            }

            placedFurnitureList = new Dictionary<GameObject, FurnitureData>();
        }
    }

    private GameObject SpawnFurnitureObj(FurnitureData data)
    {
        if(furnitureSetList.Count > 0)
        {
            GameObject furnitureToSpawn = furnitureSetList[data.categoryIndex].furnitureDataList[data.furnitureIndex].furnitureGameObj;

            GameObject furnitureObj = Instantiate(furnitureToSpawn,data.position, Quaternion.Euler(data.rotation.x, data.rotation.y, data.rotation.z));
            furnitureObj.GetComponent<ColorChanger>().ChangeChildrenMaterialColor(data.color);
            return furnitureObj;
        }
        else
        {
            Debug.Log("No furniture set data found. Fill in the list manualy");
            return null;
        }
    }
}
