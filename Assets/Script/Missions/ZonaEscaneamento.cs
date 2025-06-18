using System.Collections;
using UnityEngine;

public class ZonaEscaneamento : MonoBehaviour
{
    public string nomeDaZona;
    public float tempoNecessario = 5f;
    public MissionManager missionManager;

    private bool escaneado;

    private Coroutine espera = null;

    private void OnTriggerEnter(Collider other)
    {
        if (escaneado) return;

        if (other.CompareTag("Submarino") && espera == null)
        {
            espera = StartCoroutine(Waiting(tempoNecessario));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!escaneado && other.CompareTag("Submarino"))
            StopAllCoroutines();
    }

    IEnumerator Waiting(float temp)
    {
        Debug.Log("Escaneando.");
        yield return new WaitForSeconds(temp);
        escaneado = true;
        missionManager.ScanedZone(nomeDaZona);
        espera = null;
    }
}
