using Assets.Script.Missions.Dialog;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ZonaFinal : MonoBehaviour
{
    public string nomeDaZona;
    public float tempoNecessario = 15f;
    public MissionManager missionManager;
    public DialogManager dialogManager;

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
        {
            StopAllCoroutines();
            espera = null;
        }
    }

    IEnumerator Waiting(float temp)
    {
        escaneado = true;

        List<DialogLine> inicio = new()
        {
            new() { text = $"Iniciando escaneamento da zona '{nomeDaZona}'..." }
        };
        dialogManager.ShowDialog(inicio);

        yield return new WaitForSeconds(temp);

        missionManager.ScanedFinalZone();
        espera = null;
    }
}