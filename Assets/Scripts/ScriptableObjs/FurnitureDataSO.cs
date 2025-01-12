using UnityEngine;

[CreateAssetMenu]
public class FurnitureDataSO : ScriptableObject
{
    public string objName;
    public int id;
    public int categoryId;
    public Vector2Int sizeOnGrid;
    public GameObject furnitureGameObj;
    public Texture previewImage;
}
