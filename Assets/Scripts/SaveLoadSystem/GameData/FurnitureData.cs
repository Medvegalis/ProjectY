using UnityEngine;

[System.Serializable]
public class FurnitureData
{
    public int categoryIndex;
    public int furnitureIndex;
    public Color color;
    public Vector3 position;
    public Vector3 rotation;

    public FurnitureData()
    {
        this.categoryIndex = -1;
        this.furnitureIndex = -1;
        this.color = Color.white;
        this.position = new Vector3(0, 0, 0);
        this.rotation = new Vector3(0, 0, 0);
    }

    public FurnitureData(int categoryIndex, int furnitureIndex, Color color, Vector3 position, Vector3 rotation)
    {
        this.categoryIndex = categoryIndex;
        this.furnitureIndex = furnitureIndex;
        this.color = color;
        this.position = position;
        this.rotation = rotation;
    }
}
