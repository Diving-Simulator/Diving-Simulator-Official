using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Tooltip("Canvas exibido quando o jogo estiver em pause")]
    public GameObject pauseCanvas;

    public bool Paused { get; private set; }

    public GameObject primeiroButton;

    void Start()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    void Update()
    {
        bool pausePressed = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
        if (Gamepad.current != null)
            pausePressed |= Gamepad.current.startButton.wasPressedThisFrame;

        if (pausePressed)
            TogglePause();
    }

    public void TogglePause()
    {
        Paused = !Paused;
        AudioListener.pause = Paused;

        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(Paused);
            Time.timeScale = Paused ? 0 : 1;
        }

        if (Paused)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(primeiroButton);
        }
    }

    public void Sair()
    {
        Application.Quit();
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.scene.name == null)
                Destroy(obj);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
