using UnityEngine;

public class BodyTracker : MonoBehaviour
{
    [Header("Body Points")]
    public Vector3 head;
    public Vector3 rightHand;
    public Vector3 hip;
    [Tooltip("Calibration")]
    private Vector3 initialHip;
    private float initialY;
    private bool calibrated = false;

    #region Unity Methods
    void Start()
    {
        Invoke(nameof(Calibrate), 2f); //Tempo para posicionar
    }

    private void Update()
    {
        SimulateBody();

        DetectedStartGesture();
    }
    #endregion

    #region Calibração e Simulação
    /// <summary>
    /// Calibração dos pontos
    /// </summary>
    void Calibrate()
    {
        initialHip = hip;
        initialY = hip.y;
        calibrated = true;

        Debug.Log("Calibrado");
    }
    /// <summary>
    /// Simulação do corpo
    /// </summary>
    void SimulateBody()
    {
        float moveX = Input.GetAxis("Horizontal");
        float jump = Input.GetKey(KeyCode.Space) ? 1f : 0f;

        hip = new Vector3(moveX, 1 + jump * 0.3f, 0);
        head = new Vector3(0, 1.7f, 0);
        rightHand = new Vector3(0, 1.8f + jump * 0.5f, 0);
    }
    #endregion

    #region Gestos Simulação
    /// <summary>
    /// Movimentação lateral
    /// </summary>
    /// <returns></returns>
    public float GetMoveX()
    {
        if(!calibrated) return 0;

        return hip.x - initialHip.x;
    }
    /// <summary>
    /// Pulo
    /// </summary>
    /// <returns></returns>
    public bool IsJumping()
    {
        if (!calibrated) return false;

        return hip.y > initialY + 0.15f;
    }
    /// <summary>
    /// Gesto de início
    /// </summary>
    /// <returns></returns>
    public bool StartGesture()
    {
        return rightHand.y > head.y + 0.2f;
    }
    #endregion

    #region Detectar Gestos
    void DetectedStartGesture()
    {
        if (GameManager.instance.currentState != GameManager.GameState.WaitForGesture)
            return;

        if(StartGesture())
        {
            Debug.Log("Gesto detectado!");
            GameManager.instance.StartGame();
        }
    }
    #endregion
}
