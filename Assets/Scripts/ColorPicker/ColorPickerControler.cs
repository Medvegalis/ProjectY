using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ColorPickerControler : MonoBehaviour
{ 
    public float currentHue;
    public float currentSaturation;
    public float currentValue;

    [SerializeField]
    private RawImage hueImage;
    [SerializeField]
    private RawImage saturationImage;
    [SerializeField]
    private RawImage finalOutputImage;

    private Texture2D hueTexture;
    private Texture2D saturationTexture;
    private Texture2D finalOutputTexture;

    [SerializeField]
    private MeshRenderer changeThisColor;

    [SerializeField]
    private Slider hueSlider;

    public ColorChanger objColorChanger;

    private void Start()
    {
        CreateHueImage();

        CreateSaturationImage();

        CreateOutputImage();

        UpdateOutputImage();
    }

    private void CreateHueImage()
    {
        hueTexture = new Texture2D(16, 1);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "hueTexture";

        for (int i = 0; i < hueTexture.width; i++)
        {
            hueTexture.SetPixel(i, 0, Color.HSVToRGB(i / (float)hueTexture.width, 1, 0.7f));
        }

        hueTexture.Apply();
        currentHue = 0;
        hueImage.texture = hueTexture;
    }

    private void CreateSaturationImage()
    {
        saturationTexture = new Texture2D(16, 16);
        saturationTexture.wrapMode = TextureWrapMode.Clamp;
        saturationTexture.name = "saturationTexture";

        for (int y = 0; y < saturationTexture.height; y++)
        {
            for (int x = 0; x < saturationTexture.width; x++)
            {
                saturationTexture.SetPixel(y, x, Color.HSVToRGB(currentHue, y / (float)saturationTexture.width, x / (float)saturationTexture.height));
            }
        }

        saturationTexture.Apply();
        currentSaturation = 0;
        currentValue = 0;
        saturationImage.texture = saturationTexture;
    }

    private void CreateOutputImage()
    {
        finalOutputTexture = new Texture2D(16, 1);
        finalOutputTexture.wrapMode = TextureWrapMode.Clamp;
        finalOutputTexture.name = "finalOutputTexture";

        Color currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentValue);

        for (int i = 0; i < finalOutputTexture.width; i++)
        {
            finalOutputTexture.SetPixel(i, 0, currentColor);
        }

        finalOutputTexture.Apply();
        finalOutputImage.texture = finalOutputTexture;
    }

    private void UpdateOutputImage()
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentValue);

        for (int i = 0; i < finalOutputTexture.width; i++)
        {
            finalOutputTexture.SetPixel(i, 0, currentColor);
        }

        finalOutputTexture.Apply();
        finalOutputImage.texture = finalOutputTexture;


        if (objColorChanger != null)
            objColorChanger.ChangeChildrenMaterialColor(currentColor);
    }

    

    public void SetSaturationValue(float sat, float value)
    {
        currentSaturation = sat;
        currentValue = value;

        UpdateOutputImage();
    }

    public void UpdateSaturationValueImage()
    {
        currentHue = hueSlider.value;

        for (int y = 0; y < saturationTexture.height; y++)
        {
            for (int x = 0; x < saturationTexture.height; x++)
            {
                saturationTexture.SetPixel(x, y, Color.HSVToRGB(currentHue, x / (float)saturationTexture.width, y / (float)saturationTexture.height));
            }
        }

        saturationTexture.Apply();
        
        UpdateOutputImage();
    }
}
