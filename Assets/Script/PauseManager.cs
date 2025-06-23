using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Tooltip("Canvas exibido quando o jogo estiver em pause")]
    public GameObject pauseCanvas;

    public bool Paused { get; private set; }

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
        Time.timeScale = Paused ? 0f : 1f;
        AudioListener.pause = Paused;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(Paused);
    }
}
