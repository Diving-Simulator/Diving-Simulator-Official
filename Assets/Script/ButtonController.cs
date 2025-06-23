using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{

    public void Jogar()
    {
        SceneManager.LoadScene(1);

    }

    public void Sair()
    {
        Application.Quit();
    }
}
