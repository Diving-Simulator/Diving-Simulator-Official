using UnityEngine;

public class Seta : MonoBehaviour
{
    private MissionManager manager;
    private float rotationSpeed = 3f;

    void Start()
    {
        manager = FindAnyObjectByType<MissionManager>();
    }

    void Update()
    {
        if (manager == null)
            return;

        GameObject alvo = GetAlvoAtual();
        if (alvo == null)
        {
            gameObject.SetActive(false); // se não houver missão, seta some
            return;
        }

        gameObject.SetActive(true); // se houver missão, seta aparece

        Vector3 direction = alvo.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion rotacaoDesejada = Quaternion.LookRotation(direction);
        Quaternion rotacaoAtual = Quaternion.Slerp(transform.rotation, rotacaoDesejada, rotationSpeed * Time.deltaTime);

        Vector3 euler = rotacaoAtual.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f); // trava X e Z
    }

    GameObject GetAlvoAtual()
    {
        int missao = manager.GetMissaoAtual();

        switch (missao)
        {
            case 1: return manager.GetZonaAtual();
            case 2: return manager.GetArcoAtual();
            case 3: return manager.GetCoralAtivo();
            case 4: return manager.GetZonaFinal();
            default: return null;
        }
    }
}