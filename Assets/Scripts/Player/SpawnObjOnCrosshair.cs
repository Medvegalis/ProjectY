using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SpawnObjOnCrosshair : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject prefab;

    [Header("Spawn Settings")]
    public float maxSpawnDistance = 100f;
    public LayerMask spawnableLayers;
    public LayerMask furnitureLayer;

    private Camera playerCamera;

    [SerializeField]
    private Shader defaultShader;

    [SerializeField]
    private Shader transperantShader;
    private GameObject previewObject;
    [SerializeField]
    private int previewRoation;
    [SerializeField]
    private Vector3 previewPositionOffset;

    [SerializeField]
    private Vector3 previewRoationOffsetOffGrid;
    bool canRotate;
    bool inEditMode;

    public Grid grid;
    bool ignoreGrid;

    [SerializeField] 
    FurnitureHolderSO currentFurnitureSet;
    [SerializeField]
    int currentFurnitureIndex = -1;
    [SerializeField]
    int currentFurnitureSetIndex = -1;
    [SerializeField]
    FurnitureDataSO currentFurniture;
    [SerializeField]
    FurnitureDataSO lastFurnitureFromInventory;
    [SerializeField]
    FurnitureDataSO movedFurniture;

    PlayerInputControler playerInputs;

    Ray palceRay;
    RaycastHit palceRaycastHit;

    Ray editRay;
    RaycastHit editRaycastHit;

    GameObject movedObj;
    [SerializeField]
    GameObject ColorPickerUI;


    void Start()
    {
        playerInputs = GetComponent<PlayerInputControler>();

        // Get the player's camera
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("No main camera found. Assign a camera with the MainCamera tag.");
        }

        // Create a preview object
        if (prefab != null)
        {
            CreatePreviewObj(prefab);
        }
        canRotate = true;
        ignoreGrid = true;

        SetCurentSelectedObjId(0);

    }

    public void SetCurentSelectedObjId(int index)
    {
        currentFurniture = currentFurnitureSet.furnitureDataList[index];
        currentFurnitureIndex = index;
        if(currentFurniture != null)
            lastFurnitureFromInventory = currentFurniture;

        if (lastFurnitureFromInventory == null)
        {
            lastFurnitureFromInventory = currentFurniture;
        }

        prefab = currentFurnitureSet.furnitureDataList[index].furnitureGameObj;
        RemovePreviewObj();
        ResetRotation();
        CreatePreviewObj(prefab);
    }

    public void SetFurnitureSet(FurnitureHolderSO furnitureSet, int index)
    {
        currentFurnitureSet = furnitureSet;
        currentFurnitureSetIndex = index;
    }

    private void CreatePreviewObj(GameObject prefab)
    {
        previewObject = Instantiate(prefab);
        MeshCollider[] meshColldiers = previewObject.GetComponentsInChildren<MeshCollider>();
        foreach(MeshCollider meshCollider in meshColldiers)
        {
            meshCollider.enabled = false;
        }
        SetMaterialShader(previewObject);
        previewObject.SetActive(false);
    }

    private void RemovePreviewObj()
    {
        if(previewObject != null)
            Destroy(previewObject);
    }

    void Update()
    {
        if(playerInputs.enterEditMode.WasPerformedThisFrame())
        {
            if (prefab == null)
                return;

            if(inEditMode)
            {
                inEditMode = false;
                StateManager.instance.ChangeGameState(StateManager.GameStates.ViewMode);
                RemovePreviewObj();
                ResetRotation();
            }
            else
            {
                inEditMode = true;
                StateManager.instance.ChangeGameState(StateManager.GameStates.EditMode);
                CreatePreviewObj(prefab);
            }
        }

        if (!inEditMode || prefab == null)
            return;

        if (playerInputs.ignoreGrid.WasPerformedThisFrame())
        {
            if (ignoreGrid)
            {
                ignoreGrid = false;
                RotateOffset();
            }
            else
            {
                ignoreGrid = true;
                RotateOffset(); 
            }
            
        }
       

        if (playerInputs.editColor.WasPressedThisFrame())
        {
            EnableColorPicker();
            playerInputs.DisableInputsOnColorPickerInputs();
        }
        if (playerInputs.editColor.WasReleasedThisFrame())
        {
            DisableColorPicker();
            playerInputs.EnableInputs();

            PlacedFurnitureTracker.instance.UpdateFurnitureToList(movedObj, currentFurnitureSetIndex, currentFurnitureIndex);
        }

        // Update the position of the preview object
        UpdatePreviewPosition();

        if (playerInputs.rotate.WasPressedThisFrame() && canRotate)
        {
            RotatePreviewObject();
        }

        // Check for input to spawn the prefab (e.g., left mouse button)
        if (playerInputs.place.WasPerformedThisFrame())
        {
            SpawnPrefabAtLook();
        }
        
        if (playerInputs.editObj.WasPerformedThisFrame())
        {
            if(movedObj != null) 
            {
                CancelMove();
            }
            else
            {
                GetRaycastHitObject();
            }
            
        }

        if (playerInputs.deleteObj.WasPerformedThisFrame())
        {
            DeletePickedUpObject();
        }

        // Reset rotation flag after key release
        if (playerInputs.rotate.WasReleasedThisFrame())
        {
            canRotate = true;
        }
    }
    private void RotatePreviewObject()
    {
        if (previewObject != null)
        {
            AddToRoatation();
            previewObject.transform.Rotate(0, 90, 0);
            RotateOffset();
            UpdatePreviewPosition();
            canRotate = false; 
        }
        
    }

    private void AddToRoatation()
    {
        if (previewRoation == 270)
        {
            previewRoation = 0;
            return;
        }

        previewRoation += 90;
    }

    public void ResetRotation()
    {
        previewRoation = 0;
        RotateOffset();
    }

    private void RotateOffset()
    {
        if(ignoreGrid)
        {
            switch (previewRoation)
            {
                case 0:
                    previewRoationOffsetOffGrid = new Vector3(-(currentFurniture.sizeOnGrid.x / 2f), 0, -(currentFurniture.sizeOnGrid.y / 2f));
                    break;
                case 90:
                    previewRoationOffsetOffGrid = new Vector3(-(currentFurniture.sizeOnGrid.y / 2f), 0, (float)currentFurniture.sizeOnGrid.x - (currentFurniture.sizeOnGrid.x / 2f));
                    break;
                case 180:
                    previewRoationOffsetOffGrid = new Vector3((float)currentFurniture.sizeOnGrid.x - (currentFurniture.sizeOnGrid.x/2f), 0, (float)currentFurniture.sizeOnGrid.y - (currentFurniture.sizeOnGrid.y / 2f));
                    break;
                case 270:
                    previewRoationOffsetOffGrid = new Vector3((float)currentFurniture.sizeOnGrid.y - (currentFurniture.sizeOnGrid.y / 2f), 0, -(currentFurniture.sizeOnGrid.x / 2f));
                    break;
            }
        }
        else
        {
            switch (previewRoation)
            {
                case 0:
                    previewPositionOffset = new Vector3(0, 0, 0);
                    break;
                case 90:
                    previewPositionOffset = new Vector3(0, 0, currentFurniture.sizeOnGrid.x);
                    break;
                case 180:
                    previewPositionOffset = new Vector3(currentFurniture.sizeOnGrid.x, 0, currentFurniture.sizeOnGrid.y);
                    break;
                case 270:
                    previewPositionOffset = new Vector3(currentFurniture.sizeOnGrid.y, 0, 0);
                    break;
            }
        }
        
    }

    private void MatchRotation(GameObject objToMatchTo)
    {
        
        if(movedObj.name.Contains("sofa"))
        {
            Debug.Log("GotSofa");
            previewRoation = (int)objToMatchTo.transform.rotation.eulerAngles.y + 90;

        }
        else
        {
            previewRoation = (int)objToMatchTo.transform.rotation.eulerAngles.y;
        }

        RotateOffset();
    }

    void UpdatePreviewPosition()
    {
        if (playerCamera == null || previewObject == null)
            return;

        palceRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(palceRay, out palceRaycastHit, maxSpawnDistance, spawnableLayers))
        {
            previewObject.SetActive(true);

            if(ignoreGrid)
            {
                previewObject.transform.position = palceRaycastHit.point + previewRoationOffsetOffGrid;
            }
            else
            {
                Vector3Int gridPos = grid.WorldToCell(palceRaycastHit.point);
                previewObject.transform.position = grid.CellToWorld(gridPos) + previewPositionOffset;

            }
        }
        else
        {
            previewObject.SetActive(false);
        }
    }

    private void GetRaycastHitObject()
    {
        editRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(editRay, out editRaycastHit, maxSpawnDistance, furnitureLayer))
        {
            GameObject hitObj = editRaycastHit.transform.parent.parent.gameObject;
            RemovePreviewObj();
            ResetRotation();

            FurnitureDataHolder currentFurnitureData = hitObj.GetComponent<FurnitureDataHolder>();

            movedFurniture = currentFurnitureData.furnitureData;
            currentFurnitureSetIndex = currentFurnitureData.furnitureData.categoryId;
            currentFurnitureIndex = currentFurnitureData.furnitureData.id;

            Debug.Log($"Picked obj category= {currentFurnitureData.furnitureData.categoryId}");
            Debug.Log($"Picked obj name = {currentFurnitureData.furnitureData.name}");

            currentFurniture = movedFurniture;

            CreatePreviewObj(hitObj);
            prefab = hitObj;
            movedObj = hitObj;
            MatchRotation(movedObj);

            ChangePreviewMaterialColor(previewObject, Color.blue);
            StateManager.instance.ChangeEditState(StateManager.EditState.MovingObj);
        }
    }

    private void CancelMove()
    {
        movedObj = null;
        SetCurentSelectedObjId(currentFurnitureIndex);
        StateManager.instance.ChangeEditState(StateManager.EditState.PlacingFromInvetory);
    }

    private void DeletePickedUpObject()
    {
        if (StateManager.instance.currentEditState == StateManager.EditState.MovingObj)
        {
            Destroy(movedObj);
            AudioManager.instance.Play("Remove");
            PlacedFurnitureTracker.instance.RemoveFurnitureFromList(movedObj);

            if (lastFurnitureFromInventory != null)
            {
                prefab = lastFurnitureFromInventory.furnitureGameObj;
                currentFurniture = lastFurnitureFromInventory;
            }
            else
            {
                prefab = movedObj;
                currentFurniture = movedFurniture;
            }
            movedObj = null;

            RemovePreviewObj();
            ResetRotation();
            CreatePreviewObj(prefab);

            StateManager.instance.ChangeEditState(StateManager.EditState.PlacingFromInvetory);
        }
    }

    void SpawnPrefabAtLook()
    {
        if (playerCamera == null || prefab == null)
        {
            return;
        }

        // Raycast from the center of the screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxSpawnDistance, spawnableLayers))
        {
            prefab = Instantiate(prefab, previewObject.transform.position, previewObject.transform.rotation);
            Debug.Log($"Adding to placedList: {prefab.name} catId: {currentFurnitureSetIndex} id: {currentFurnitureIndex} to list");
            PlacedFurnitureTracker.instance.UpdateFurnitureToList(prefab, currentFurnitureSetIndex, currentFurnitureIndex);
            AudioManager.instance.Play("Place");
            if (movedObj != null)
            {
                Destroy(movedObj);
                movedObj = null;

                prefab = lastFurnitureFromInventory.furnitureGameObj;
                currentFurniture = lastFurnitureFromInventory;
                RemovePreviewObj();
                ResetRotation();
                CreatePreviewObj(prefab);
                ChangePreviewMaterialColor(previewObject, Color.white);
                
                StateManager.instance.ChangeEditState(StateManager.EditState.PlacingFromInvetory);
            }
        }
        else
        {
            Debug.Log("No valid surface found to spawn the prefab.");
        }
    }

    void SetMaterialShader(GameObject objects)
    {
        Renderer[] renderers = objects.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            defaultShader = material.shader;
            renderer.material.shader = transperantShader;
            renderer.material = material;
        }
    }

    void ChangePreviewMaterialColor(GameObject preview, Color color)
    {
        Renderer[] renderers = preview.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            renderer.material.color = color;
            renderer.material = material;
        }
    }

    private void EnableColorPicker()
    {
        if (movedObj == null)
            return; 

        ColorPickerUI.SetActive(true);
        ColorPickerControler cpc = ColorPickerUI.GetComponent<ColorPickerControler>();
        cpc.objColorChanger = movedObj.GetComponent<ColorChanger>();
    }

    private void DisableColorPicker()
    {
        ColorPickerUI.SetActive(false);
    }
}
