using System.Collections.Generic;
using UnityEngine;

public class CoralCatcher : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> corais;
    private List<GameObject> coraisPegos;
    public MissionManager missionManager;

    private void Awake()
    {
        coraisPegos = new List<GameObject>();
    }

    public void PegouCoral(GameObject coral)
    {
        if (corais.Contains(coral) && !coraisPegos.Contains(coral))
        {
            coraisPegos.Add(coral);
            coral.SetActive(false);
        }

        if (coraisPegos.Count >= corais.Count)
        {
            Debug.Log("Missão 3 concluída");
            missionManager.MissionsManager();
        }
    }
}
