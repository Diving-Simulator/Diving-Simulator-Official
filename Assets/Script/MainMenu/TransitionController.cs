using UnityEngine;
using System.Collections;

public class IntroAnimatorController : MonoBehaviour
{
    [Header("Animators")]
    public Animator animTransition;
    public Animator animFundo;
    public Animator animLogo;

    [Header("Objects")]
    public GameObject videoObject;
    public GameObject transitionObject;
    public GameObject canvasButtons;
    public Animator buttonJogar;
    public Animator buttonSair;

    public float fadeBotao = 0.25f;

    private bool iniciou = false;

    void Update()
    {
        if (!iniciou && Input.GetMouseButtonDown(0))
        {
            iniciou = true;
            StartCoroutine(ExecutarIntro());
        }


        if (iniciou)
        {

            StartCoroutine(AparecerBotoes());

        }
    }

    IEnumerator AparecerBotoes()
    {

        yield return new WaitForSeconds(9.5f);

        canvasButtons.SetActive(true);

        yield return new WaitForSeconds(fadeBotao);

        buttonJogar.SetTrigger("IsFade");

        yield return new WaitForSeconds(fadeBotao);
        buttonSair.SetTrigger("IsFade");
    }

    IEnumerator ExecutarIntro()
    {
        // inicia animação da transição
        animTransition.SetTrigger("TransitionStartFadeIn");

        // espera 2 segundos
        yield return new WaitForSeconds(4f);

        // inicia animação do fundo
        animFundo.SetTrigger("FundoStartFadeIn");

        // espera terminar a animação do fundo (1.5 segundos de animação)
        yield return new WaitForSeconds(1.5f);

        // inicia fade-in da logo
        animLogo.SetTrigger("LogoStartFadeIn");

        // aguarda 2.5 segundos.
        yield return new WaitForSeconds(2.5f);

        // sobe a logo para o todo
        animLogo.SetTrigger("LogoStartSubir");

        // Desativa objetos não mais usados (parar o vídeo para aumentar minimamente a performance)
        videoObject.SetActive(false);
        transitionObject.SetActive(false);
    }
}