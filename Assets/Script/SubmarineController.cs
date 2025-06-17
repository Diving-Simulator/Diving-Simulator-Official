using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class SubmarineController : MonoBehaviour
{
    public enum ModoControle
    {
        TecladoEMouse,
        Controle,
        VR
    }

    public InputActionAsset inputActionsAsset;
    private InputAction moveAction;


    [Header("Referência ao corpo visual (onde está o Rigidbody) e Camera Shaker")]
    public Transform corpoVisual;
    public CameraShaker cameraShaker;

    [Header("Velocidade")]
    [Range(0f, 35f)] public float velocidadeMaxima = 35f;
    public float aceleracao = 3f;
    public float desaceleracao = 4f;
    public float recuo = 10f;

    [Header("Rotação")]
    public float velocidadeRotacao = 50f;
    public float pitchMaximo = 50f;

    [Header("Roll")]
    public float velocidadeRoll = 40f;
    public float fatorCorrecaoRoll = 0.4f;

    [Header("Teclas")]
    public KeyCode acelerarKey = KeyCode.LeftShift;
    public KeyCode frearKey = KeyCode.Space;
    public KeyCode rollEsquerdaKey = KeyCode.Q;
    public KeyCode rollDireitaKey = KeyCode.E;

    [Header("Luz")]
    public GameObject luzExterna;

    private ModoControle modoAtual;
    private Rigidbody rb;

    private float velocidadeAtual = 0f;
    private bool modoLivre = false;
    private bool realinhando = false;
    private bool luzAtiva = false;

    private float inputPitch, inputYaw, inputRoll;
    private bool inputAcelerar, inputFrear;

    void Start()
    {
        rb = corpoVisual.GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("⚠ Rigidbody não encontrado no corpoVisual!");

        var actionMap = inputActionsAsset.FindActionMap("Player", true);
        moveAction = actionMap.FindAction("Move", true);
        moveAction.Enable();

        InputSystem.settings.disableRedundantEventsMerging = true;

        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
        DetectarModo();
    }

    private void InputSystem_onDeviceChange(UnityEngine.InputSystem.InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
            {
                Debug.Log("🎮 Controle conectado.");
                DetectarModo();
            }
        }
    }

    void Update()
    {
        DetectarModo();
        LerInput();
        TratarEntradaDeTecla();
    }

    void FixedUpdate()
    {
        if (realinhando)
        {
            RealinharVisual();
            return;
        }

        Rotacionar();
        MoverFisicamente();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.isTrigger) return;

        if (Mathf.Abs(velocidadeAtual) > 1f)
        {
            velocidadeAtual *= 0.2f;

            if (cameraShaker != null)
            {
                cameraShaker.Shake();
            }
        }
    }

    void TratarEntradaDeTecla()
    {
        if (Input.GetKeyDown(KeyCode.F) || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            luzAtiva = !luzAtiva;
            luzExterna?.SetActive(luzAtiva);
        }

        if (Input.GetKeyDown(KeyCode.R) || (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame))
        {
            if (modoLivre) realinhando = true;
            modoLivre = !modoLivre;
            Debug.Log("Modo Livre: " + (modoLivre ? "Ativado" : "Desativado"));
        }
    }

    void RealinharVisual()
    {
        Quaternion alvo = Quaternion.Euler(0f, corpoVisual.localEulerAngles.y, 0f);
        corpoVisual.localRotation = Quaternion.RotateTowards(corpoVisual.localRotation, alvo, velocidadeRotacao * 3f * Time.fixedDeltaTime);

        if (Quaternion.Angle(corpoVisual.localRotation, alvo) < 0.5f)
            realinhando = false;
    }

    void Rotacionar()
    {
        Vector3 euler = corpoVisual.localEulerAngles;
        float pitch = NormalizeAngle(euler.x);
        float yaw = NormalizeAngle(euler.y);
        float roll = NormalizeAngle(euler.z);

        if (Mathf.Abs(inputYaw) > 0.01f)
        {
            yaw += inputYaw * velocidadeRotacao * Time.fixedDeltaTime;
        }

        if (Mathf.Abs(inputPitch) > 0.01f)
        {
            pitch += inputPitch * velocidadeRotacao * Time.fixedDeltaTime;
            pitch = Mathf.Clamp(pitch, -pitchMaximo, pitchMaximo);
        }
        else
        {
            if (pitch < 0f && Mathf.Abs(velocidadeAtual) > 0.01f)
            {
                float fator = Mathf.InverseLerp(0f, velocidadeMaxima, Mathf.Abs(velocidadeAtual));
                float intensidade = velocidadeRotacao * fator * 0.3f;
                pitch = Mathf.MoveTowards(pitch, 0f, intensidade * Time.fixedDeltaTime);
            }
        }

        if (modoLivre)
        {
            if (Mathf.Abs(inputRoll) > 0.01f)
            {
                roll += inputRoll * velocidadeRoll * Time.fixedDeltaTime;
                roll = Mathf.Clamp(roll, -180f, 180f);
            }
        }
        else
        {
            if (Mathf.Abs(inputRoll) > 0.01f)
            {
                roll += inputRoll * velocidadeRoll * Time.fixedDeltaTime;
                roll = Mathf.Clamp(roll, -45f, 45f);
            }

            float fatorZ = Mathf.InverseLerp(0f, velocidadeMaxima, Mathf.Abs(velocidadeAtual));
            float intensidadeZ = velocidadeRoll * Mathf.Max(fatorZ, 0.3f) * fatorCorrecaoRoll;
            roll = Mathf.MoveTowards(roll, 0f, intensidadeZ * Time.fixedDeltaTime);
        }

        if (realinhando)
        {
            pitch = Mathf.MoveTowards(pitch, 0f, velocidadeRotacao * 3f * Time.fixedDeltaTime);
            roll = Mathf.MoveTowards(roll, 0f, velocidadeRoll * 3f * Time.fixedDeltaTime);

            if (Mathf.Abs(pitch) < 0.1f && Mathf.Abs(roll) < 0.1f)
                realinhando = false;
        }

        corpoVisual.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }


    void MoverFisicamente()
    {
        if (inputAcelerar)
            velocidadeAtual += aceleracao * Time.fixedDeltaTime;
        else if (inputFrear)
            velocidadeAtual = Mathf.MoveTowards(velocidadeAtual, -recuo, desaceleracao * Time.fixedDeltaTime);
        else
            velocidadeAtual = Mathf.MoveTowards(velocidadeAtual, 0f, desaceleracao * Time.fixedDeltaTime);

        velocidadeAtual = Mathf.Clamp(velocidadeAtual, -recuo, velocidadeMaxima);

        Vector3 movimento = corpoVisual.forward * velocidadeAtual * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movimento);
    }

    void LerInput()
    {
        inputPitch = inputYaw = inputRoll = 0f;
        inputAcelerar = inputFrear = false;

        switch (modoAtual)
        {
            case ModoControle.TecladoEMouse:
                Vector2 move = moveAction.ReadValue<Vector2>();
                inputYaw = move.x;
                inputPitch = -move.y;
                if (Keyboard.current.qKey.isPressed) inputRoll = 1f;
                if (Keyboard.current.eKey.isPressed) inputRoll = -1f;
                inputAcelerar = Input.GetKey(acelerarKey);
                inputFrear = Input.GetKey(frearKey);
                break;

            case ModoControle.Controle:
            case ModoControle.VR:
                if (Gamepad.current != null)
                {
                    inputYaw = Gamepad.current.leftStick.x.ReadValue();
                    inputPitch = -Gamepad.current.leftStick.y.ReadValue();
                    inputAcelerar = Gamepad.current.rightTrigger.ReadValue() > 0.1f;
                    inputFrear = Gamepad.current.leftTrigger.ReadValue() > 0.1f;
                    if (Gamepad.current.leftShoulder.isPressed) inputRoll = 1f;
                    if (Gamepad.current.rightShoulder.isPressed) inputRoll = -1f;
                }
                break;
        }
    }

    void DetectarModo()
    {
        var novoModo =
            XRSettings.isDeviceActive
                ? (Gamepad.current != null ? ModoControle.VR : modoAtual)
                : (Gamepad.current != null ? ModoControle.Controle : ModoControle.TecladoEMouse);

        if (novoModo != modoAtual)
        {
            Debug.Log("Mudança de modo detectada: " + novoModo);
            modoAtual = novoModo;
        }

        if (modoAtual == ModoControle.VR && Gamepad.current == null)
        {
            Debug.LogError("VR ativo, mas nenhum controle detectado!");
            enabled = false;
        }
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}