using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public SubmarineController subController;
    public GameObject InitialCanvas;
    public GameObject primeiroButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(primeiroButton);
    }

    public void Jogar()
    {
        subController.AtivarSubmarino();
        InitialCanvas.SetActive(false);
    }

    public void Sair()
    {
        Application.Quit();
    }
}
