using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ChairInteractable : MonoBehaviour
{
    [Header("Sentar")]
    public Transform seatPosition;
    public GameObject locomotion;
    public bool disableMovementOnSit = true;

    private bool isSitting = false;
    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("Tag 'Player' não encontrada no XR Origin!\nJogo não é em VR.");
            return;
        }

        if (isSitting) return;

        player.transform.SetPositionAndRotation(seatPosition.position, seatPosition.rotation);
        if (disableMovementOnSit && locomotion != null)
        {
            locomotion.SetActive(false);
            this.enabled = false;
        }

        isSitting = true;
        Debug.Log("Jogador sentou na cadeira via 'activated'!");
    }
}