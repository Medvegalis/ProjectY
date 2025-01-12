using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    PlayerInputControler playerInputs;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject SaveLoadMenu;
    
    private bool gameStarted;
    [SerializeField]
    GameObject PlayerUI;

    private bool isPaused = false;
    

    private void Update()
    {
        if (!gameStarted)
            return;
        
        if (playerInputs.pauseGame.WasPressedThisFrame())
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void StartGame()
    {
        gameStarted = true;        
    }

    public void PauseGame()
    {
        isPaused = true;

        if (PlayerUI != null)
        {
            PlayerUI.SetActive(false);
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }

        Time.timeScale = 0f;

        playerInputs.DisableInputs();

    }

    public void ResumeGame()
    {
        isPaused = false;

        if (PlayerUI != null)
        {
            PlayerUI.SetActive(true);
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        Time.timeScale = 1f;

        playerInputs.EnableInputs();

    }

    public void CloseApplication()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // Quit the application
                Application.Quit();
        #endif
    }

}
