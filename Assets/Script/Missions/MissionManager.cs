using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private int totalZones = 3;
    private int concludedZones = 0;

    private int totalMissions = 4;
    private int actualMission = 0;

    private int actualArch = 1;
    private int totalArchs = 6;

    [SerializeField]
    private List<GameObject> arcos;

    [SerializeField]
    private List<GameObject> Missoes;

    private void Start()
    {
        if (Missoes.Count < 3)
        {
            Debug.Log("Objeto de missões incompletos.");
        }
        MissionsManager();
    }

    public void ScanedZone(string nomeDaZona)
    {
        concludedZones++;
        Debug.Log($"Zona {nomeDaZona} escaneada com sucesso. {concludedZones}/{totalZones} zonas escaneadas.");

        if (concludedZones >= totalZones)
        {
            Debug.Log("Missão 1 concluída com sucesso.");
            MissionsManager();
        }
    }

    public bool ValidArch(int number)
    {
        if (number == actualArch)
        {
            Debug.Log($"Arco {actualArch} alcançado com sucesso.");
            actualArch++;
            if (actualArch > totalArchs)
            {
                Debug.Log("Missão 2 concluída com sucesso.");
                MissionsManager();
                return false;
            }
            return true;
        }
        else
        {
            Debug.Log($"Missão 2 esse não é o arco certo. Vá para o arco {actualArch}.");
            return false;
        }
    }

    public void PassarArco(int arcoAtual)
    {
        if (arcoAtual > arcos.Count)
        {
            Debug.Log($"Arco {arcoAtual} não foi colocado na lista");
            return;
        }

        arcos[arcoAtual].SetActive(false);
        arcos[arcoAtual + 1].SetActive(true);
    }

    public void MissionsManager()
    {
        actualMission++;

        if (actualMission > totalMissions)
        {
            Debug.Log("Parabéns, você concluiu todas as missões!");
            return;
        }

        ReloadMissions();

        if (actualMission - 1 > Missoes.Count)
        {
            Debug.LogWarning($"Missão {actualMission} não encontrada.");
            return;
        }

        switch (actualMission)
        {
            case 1:
                Missoes[0].SetActive(true);
                break;
            case 2:
                Missoes[1].SetActive(true);
                break;
            case 3:
                Missoes[2].SetActive(true);
                break;
            case 4:
                Missoes[3].SetActive(true);
                break;
            default:
                break;
        }
    }


    private void ReloadMissions()
    {
        foreach (var item in Missoes)
        {
            item.SetActive(false);
        }
    }
}