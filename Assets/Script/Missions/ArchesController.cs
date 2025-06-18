using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ArchesController : MonoBehaviour
{
    private bool pegou = false;
    public MissionManager missionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (pegou && !other.CompareTag("Submarino")) return;

        string numeroNome = Regex.Replace(name, "[^0-9]", "");
        if (!int.TryParse(numeroNome, out int numArch))
        {
            Debug.LogWarning($"[Missão 2] Nome do arco inválido: {name}");
            return;
        }

        bool sucesso = missionManager.ValidArch(numArch);

        if (sucesso)
        {
            pegou = true;
            missionManager.PassarArco(numArch - 1);
        }
    }
}