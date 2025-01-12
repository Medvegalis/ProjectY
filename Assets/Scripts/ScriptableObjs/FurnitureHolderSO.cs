using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu]
public class FurnitureHolderSO : ScriptableObject
{
    public string furnitureSetName;
    public List<FurnitureDataSO> furnitureDataList;
}
