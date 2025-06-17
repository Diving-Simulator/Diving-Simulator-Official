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

    //[Header("Levantar (opcional)")]
    //public XRSimpleInteractable standInteractable; // pode deixar sem uso
    //public bool disableMovementOnStand = false;

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

        //var interactable = GetComponent<XRSimpleInteractable>();
        //interactable.activated.AddListener(OnSit);
        // Se quiser monitorar cancelamento/desativação:
        // interactable.deactivated.AddListener(OnDeactivated);
    }

    private void OnTriggerEnter()
    {
        if (isSitting) return;

        player.transform.SetPositionAndRotation(seatPosition.position, seatPosition.rotation);
        if (disableMovementOnSit && locomotion != null)
        {
            locomotion.SetActive(false);
            this.enabled = false;
        }

        isSitting = true;
        Debug.Log("Jogador sentou na cadeira via 'activated'!");

        /*
        // Prepare standInteractable para levantar:
        if (standInteractable != null)
        {
            standInteractable.gameObject.SetActive(true);
            standInteractable.activated.AddListener(OnStand);
        }
        */
    }

    //private void OnStand(ActivateEventArgs args)
    //{
    //    if (!isSitting) return;

    //    if (disableMovementOnStand && locomotion != null)
    //        locomotion.SetActive(false);
    //    else if (locomotion != null)
    //        locomotion.SetActive(true);

    //    isSitting = false;
    //    Debug.Log("Jogador se levantou da cadeira.");

    //    /*
    //    if (standInteractable != null)
    //    {
    //        standInteractable.activated.RemoveListener(OnStand);
    //        standInteractable.gameObject.SetActive(false);
    //    }
    //    */
    //}
}