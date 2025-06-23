using System.Collections.Generic;
using UnityEngine;

public class CoralCatcher : MonoBehaviour
{
    List<GameObject> objetosParaColetar = new List<GameObject>();
    int coletados = 0;

    [SerializeField] MissionManager missionManager;
    [SerializeField] GameObject particlePrefab;

    private void Awake()
    {
        foreach (Transform filho in transform)
        {
            Collider col = filho.GetComponent<Collider>();

            if (col == null)
            {
                filho.gameObject.AddComponent<MeshCollider>();
                col = filho.GetComponent<Collider>();
                col.isTrigger = true;
            }

            if (col != null && col.isTrigger)
            {
                objetosParaColetar.Add(filho.gameObject);

                if (particlePrefab != null)
                {
                    GameObject instancia = Instantiate(particlePrefab, filho);
                    instancia.transform.localPosition = Vector3.zero;
                }

                var coletavel = filho.GetComponent<CoralCollider>();
                if (coletavel == null)
                    coletavel = filho.gameObject.AddComponent<CoralCollider>();

                coletavel.OnColetado += PegouCoral;
            } else
            {
                filho.gameObject.SetActive(false);
            }
        }
    }

    public void PegouCoral(GameObject coral)
    {
        if (!objetosParaColetar.Contains(coral)) return;

        coletados++;
        Debug.Log($"[Missão 3] Coletado {coletados}/{objetosParaColetar.Count}");

        if (coletados >= objetosParaColetar.Count)
        {
            Debug.Log("[Missão 3] Todos os objetos foram coletados.");
            missionManager.MissionsManager();
        }
    }
}
