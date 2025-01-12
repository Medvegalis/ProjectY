using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objWithColorList;

    public Color currentColor { get; private set; }

    private void OnEnable()
    {
        currentColor = Color.white;
    }

    public void ChangeChildrenMaterialColor(Color newColor)
    {
        if(objWithColorList.Count > 0)
        {
            currentColor = newColor;
            foreach (GameObject obj in objWithColorList)
            {
                Renderer renderer = obj.GetComponent<Renderer>();

                if (renderer != null)
                {
                    foreach (var material in renderer.materials)
                    {
                            
                        material.color = newColor;
                    }
                }
            }
            
        }
    }
}
