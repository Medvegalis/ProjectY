using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureSelectionManager : MonoBehaviour
{
    [SerializeField]
    GameObject funitureSeletorObj;

    [SerializeField]
    SpawnObjOnCrosshair spawnObjOnCrosshair;

    bool menuOpen;
    [SerializeField]
    PlayerInputControler playerInputs;
    [SerializeField]
    List<FurnitureHolderSO> categoryList;
    [SerializeField]
    GameObject buttonParent;
    [SerializeField]
    List<GameObject> buttonList;

    FurnitureHolderSO currentCategory;
    int currentCategoryIndex = -1;

    private void OnEnable()
    {
        SetCategory(0);
    }

    private void Update()
    {
        if (playerInputs.openInventory.WasPressedThisFrame())
        {
            menuOpen = true;
            funitureSeletorObj.SetActive(true);
            playerInputs.DisableInputsOnInvenotryInputs();
        }

        if(playerInputs.openInventory.WasReleasedThisFrame())
        {
            menuOpen = false;
            funitureSeletorObj.SetActive(false);
            playerInputs.EnableInputs();
        }
    }

    public void SetCategory(int index)
    {
        currentCategory = categoryList[index];
        currentCategoryIndex = index;
        spawnObjOnCrosshair.SetFurnitureSet(currentCategory, currentCategoryIndex);

        foreach (Transform child in buttonParent.transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < currentCategory.furnitureDataList.Count; i++)
        {
            FurnitureDataSO currentFurnitureData = currentCategory.furnitureDataList[i];
            buttonList[i].SetActive(true);
            buttonList[i].GetComponentInChildren<RawImage>().texture = currentFurnitureData.previewImage;
            buttonList[i].GetComponentInChildren<TextMeshProUGUI>().text = currentFurnitureData.objName;
            int iCopy = i;

            buttonList[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                spawnObjOnCrosshair.SetCurentSelectedObjId(iCopy);
            });
        }
    }


}
