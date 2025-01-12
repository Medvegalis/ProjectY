using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<FurnitureData> placedFurnitureData;

    public GameData() 
    { 
        placedFurnitureData = new List<FurnitureData>();
    }
}
