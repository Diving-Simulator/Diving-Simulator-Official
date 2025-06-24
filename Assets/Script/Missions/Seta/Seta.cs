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
            return;

        Vector3 direction = alvo.transform.position - transform.position;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);

        if (flatDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion rotacaoDesejada = Quaternion.LookRotation(flatDirection);
        Quaternion rotacaoAtual = Quaternion.Slerp(transform.rotation, rotacaoDesejada, rotationSpeed * Time.deltaTime);

        Vector3 euler = rotacaoAtual.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

    }

    GameObject GetAlvoAtual()
    {
        int missao = manager.GetMissaoAtual();
        GameObject alvo = null;

        switch (missao)
        {
            case 1:
                alvo = manager.GetZonaAtual();
                break;
            case 2:
                alvo = manager.GetArcoAtual();
                break;
            case 3:
                alvo = manager.GetCoralAtivo();
                break;
            case 4:
                alvo = manager.GetZonaFinal();
                break;
        }

        return (alvo != null && alvo.activeInHierarchy) ? alvo : null;
    }
}