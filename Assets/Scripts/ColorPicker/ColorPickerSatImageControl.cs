using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickerSatImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField]
    private Image pickerImage;

    private RawImage SatValueImage;
    private ColorPickerControler colPickerControler;
    private RectTransform rectTransfrom;
    private RectTransform pickerTransfrom;


    private void Awake()
    {
        SatValueImage = GetComponent<RawImage>();
        colPickerControler = transform.parent.parent.gameObject.GetComponent<ColorPickerControler>();
        rectTransfrom = GetComponent<RectTransform>();

        pickerTransfrom = pickerImage.gameObject.GetComponent<RectTransform>();
        pickerTransfrom.position = new Vector2(-(rectTransfrom.sizeDelta.x * 0.5f), -(rectTransfrom.sizeDelta.y * 0.5f));
    
    }

    private void UpdateColor(PointerEventData eventData)
    {
        Vector3 pos = rectTransfrom.InverseTransformPoint(eventData.position);

        float deltaX = rectTransfrom.sizeDelta.x * 0.5f;
        float deltaY = rectTransfrom.sizeDelta.y * 0.5f;

        pos.x = Mathf.Clamp(pos.x, -deltaX, deltaX);
        pos.y = Mathf.Clamp(pos.y, -deltaY, deltaY);

        float x = pos.x + deltaX;
        float y = pos.y + deltaY;

        float normalizedX = x / rectTransfrom.sizeDelta.x;
        float normalizedY = y / rectTransfrom.sizeDelta.y;

        pickerTransfrom.localPosition= pos;
        pickerImage.color = Color.HSVToRGB(0, 0, 1 - normalizedY);


        colPickerControler.SetSaturationValue(normalizedX, normalizedY);

    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

}
