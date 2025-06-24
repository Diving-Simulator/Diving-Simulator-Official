using UnityEngine;

public class InteractableChair : MonoBehaviour
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
            Debug.Log("Tag 'Player' não encontrada no XR Origin! Jogo não é em VR.");
            return;
        }

        if (isSitting) return;

        // Teleportar jogador para a posição da cadeira
        player.transform.SetPositionAndRotation(seatPosition.position, seatPosition.rotation);

        // Resetar offset acumulado do XR se existir
        Transform offset = player.transform.Find("Camera Offset");
        if (offset != null)
        {
            offset.localPosition = Vector3.zero;
            offset.localRotation = Quaternion.identity;
            Debug.Log("🔄 Offset da câmera resetado na cadeira (VR).");
        }

        if (disableMovementOnSit && locomotion != null)
        {
            locomotion.SetActive(false);
            this.enabled = false;
        }

        isSitting = true;
        Debug.Log("Jogador sentou na cadeira via 'activated'!");
    }
}