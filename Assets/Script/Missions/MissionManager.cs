using Assets.Script.Missions.Dialog;
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

    public DialogManager dialogManager;

    [SerializeField]
    private List<GameObject> arcos;

    [SerializeField]
    private List<GameObject> Missoes;

    [SerializeField]
    private List<GameObject> Zonas;

    [SerializeField]
    private GameObject ZonaFinal;

    public int GetMissaoAtual() => actualMission;
    public int GetZonaAtualIndex() => concludedZones;
    public int GetArcoAtualIndex() => actualArch - 1;

    public GameObject Seta;

    private void Start()
    {
        if (Missoes.Count < 3)
        {
            Debug.Log("Objeto de missões incompletos.");
        }
        MissionsManager();
        Seta.SetActive(true);
    }

    public GameObject GetZonaAtual()
    {
        if (concludedZones >= Zonas.Count)
            return null;
        return Zonas[concludedZones];
    }

    public GameObject GetArcoAtual()
    {
        if (actualArch - 1 >= arcos.Count)
            return null;
        return arcos[actualArch - 1];
    }

    public GameObject GetCoralAtivo()
    {
        GameObject missao3 = Missoes.Count >= 3 ? Missoes[2] : null;
        if (missao3 == null) return null;

        foreach (Transform t in missao3.transform)
        {
            if (t.gameObject.activeInHierarchy)
                return t.gameObject;
        }

        return null;
    }

    public GameObject GetZonaFinal()
    {
        return ZonaFinal;
    }

    public void mudarZona(int zonaID)
    {
        if (zonaID < 0 || zonaID >= Zonas.Count)
        {
            Debug.LogWarning($"zonaID {zonaID} fora do intervalo. Tamanho de Zonas: {Zonas.Count}");
            return;
        }

        foreach (var zona in Zonas)
        {
            zona.gameObject.SetActive(false);
        }

        Zonas[zonaID].SetActive(true);
    }

    public void ScanedZone(string nomeDaZona)
    {
        if (nomeDaZona == "ZonaEscaneamento_Final")
        {
            dialogManager.ShowDialog(new()
        {
            new() { text = "Parabéns, você concluiu todas as missões do mergulho com sucesso!" }
        });
            return;
        }

        concludedZones++;

        List<DialogLine> dialog = new()
    {
        new() { text = $"Zona '{nomeDaZona}' escaneada com sucesso." },
        new() { text = $"Progresso: {concludedZones}/{totalZones} zonas concluídas." }
    };

        if (concludedZones >= totalZones)
        {
            dialog.Add(new() { text = "Missão 1 concluída com sucesso! Preparando próxima missão..." });
            dialogManager.ShowDialog(dialog);
            MissionsManager();
        }
        else
        {
            dialogManager.ShowDialog(dialog);
            mudarZona(concludedZones);
        }
    }

    public bool ValidArch(int number)
    {
        List<DialogLine> dialog;

        if (number == actualArch)
        {
            dialog = new()
            {
                    new() { text = $"Arco {actualArch} alcançado com sucesso." }
            };
            dialogManager.ShowDialog(dialog);
            actualArch++;
            if (actualArch > totalArchs)
            {
                dialog = new()
                {
                        new() { text = $"Missão 2 concluída com sucesso, parabéns!" }
                };
                dialogManager.ShowDialog(dialog);
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

        TextoMissoes(actualMission);

        switch (actualMission)
        {
            case 1:
                Missoes[0].SetActive(true);
                mudarZona(concludedZones);
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

    private void TextoMissoes(int idMissao)
    {
        List<DialogLine> dialog = new()
        {
            new() { text = "Missão não reconhecida, contate os desenvolvedores." }
        };

        switch (idMissao)
        {
            case 1:
                dialog = new()
                {
                    new() { text = "Missão 1: Escaneie todas as zonas verdes espalhadas pelo fundo do oceano. Elas são esferas grandes e brilhantes em locais estratégicos." }
                };
                break;
            case 2:
                dialog = new()
                {
                    new() { text = "Missão 2: A navegação requer precisão agora. Passe cuidadosamente por todos os arcos de aço submersos." }
                };
                break;
            case 3:
                dialog = new()
                {
                    new() { text = "Missão 3: Explore a biodiversidade local procurando por corais exóticos para análise científica." }
                };
                break;
            case 4:
                dialog = new()
                {
                    new() { text = "Missão 4: Leve os dados coletados até a zona próxima à superfície para transmissão via satélite." }
                };
                break;
            default:
                Debug.LogWarning($"Missão {idMissao} não reconhecida.");
                break;
        }
        dialogManager.ShowDialog(dialog);
    }

    private void ReloadMissions()
    {
        foreach (var item in Missoes)
        {
            item.SetActive(false);
        }
    }
}