using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableChair : MonoBehaviour
{
    [Header("Sentar")]
    public Transform seatPosition;
    public GameObject locomotion;
    public bool disableMovementOnSit = true;

    private bool isSitting = false;

    void Awake()
    {
        if (isSitting) return;

        if (XRSettings.isDeviceActive)
        {
            // ✅ Modo VR ativo
            XROrigin xrOrigin = FindAnyObjectByType<XROrigin>();
            if (xrOrigin == null)
            {
                Debug.LogError("❌ XR Origin não encontrado!");
                return;
            }

            float alturaCabeca = xrOrigin.CameraInOriginSpacePos.y;
            Vector3 destino = seatPosition.position - new Vector3(0f, alturaCabeca - 1.5f, 0f);

            xrOrigin.transform.position = destino;
            xrOrigin.transform.rotation = Quaternion.LookRotation(seatPosition.forward, Vector3.up);

            Debug.Log("✅ Jogador sentou na cadeira via XR corretamente!");
        }

        if (disableMovementOnSit && locomotion != null)
        {
            locomotion.SetActive(false);
            this.enabled = false;
        }

        isSitting = true;
    }
}