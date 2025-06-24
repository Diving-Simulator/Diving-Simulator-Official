using UnityEngine;

public class Seta : MonoBehaviour
{
    [Header("Raiz das missões")]
    public Transform missoesRaiz;

    [Header("Velocidade de rotação")]
    public float rotationSpeed = 3f;

    private Transform alvoAtual;

    void Update()
    {
        if (missoesRaiz == null)
            return;

        alvoAtual = BuscarUltimoFilhoAtivo(missoesRaiz);
        if (alvoAtual == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        Vector3 pontoAlvo = ObterCentroVisual(alvoAtual);

        Vector3 direction = pontoAlvo - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        targetRotation *= Quaternion.Euler(0f, 90f, 0f); // eixo X é a frente da seta

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    Transform BuscarUltimoFilhoAtivo(Transform pai)
    {
        Transform ultimoAtivo = null;

        foreach (Transform filho in pai)
        {
            if (!filho.gameObject.activeInHierarchy)
                continue;

            Transform filhoProfundo = BuscarUltimoFilhoAtivo(filho);
            if (filhoProfundo != null)
            {
                Transform maisProximo = EncontrarMaisProximo(transform.position, filho);
                if (maisProximo != null)
                    return maisProximo;

                return filhoProfundo;
            }

            ultimoAtivo = filho;
        }

        return ultimoAtivo;
    }

    Transform EncontrarMaisProximo(Vector3 origem, Transform pai)
    {
        Transform maisProximo = null;
        float menorDist = float.MaxValue;

        foreach (Transform filho in pai)
        {
            if (!filho.gameObject.activeInHierarchy)
                continue;

            float dist = Vector3.Distance(origem, ObterCentroVisual(filho));
            if (dist < menorDist)
            {
                menorDist = dist;
                maisProximo = filho;
            }
        }

        return maisProximo;
    }

    Vector3 ObterCentroVisual(Transform alvo)
    {
        Collider col = alvo.GetComponentInChildren<Collider>();
        if (col != null)
            return col.bounds.center;

        Renderer rend = alvo.GetComponentInChildren<Renderer>();
        if (rend != null)
            return rend.bounds.center;

        return alvo.position;
    }
}