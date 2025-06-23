using System;
using UnityEngine;

public class CoralCollider : MonoBehaviour
{
    public event Action<GameObject> OnColetado;

    private bool coletado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Submarino") && !coletado)
        {
            coletado = true;
            OnColetado?.Invoke(gameObject);

            gameObject.SetActive(false);
        }
    }
}
