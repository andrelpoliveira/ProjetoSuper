using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controle do Player")]
    public BodyTracker bodyTracker;
    public float speed = 5f;
    public float jumpForce = 5f;
    [Tooltip("Componentes")]
    private Rigidbody rb;
    private bool isGrounded = true;

    #region Unity Methods
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState != GameManager.GameState.Playing)
            return;

        Move();
        Jump();
    }
    #endregion

    #region Movimentação básica
    /// <summary>
    /// Movimentação do personagem
    /// </summary>
    void Move()
    {
        float moveX = bodyTracker.GetMoveX();
        transform.position = new Vector3(moveX * speed, transform.position.y, 0);
    }
    /// <summary>
    /// Pulo do jogador
    /// </summary>
    void Jump()
    {
        if(bodyTracker.IsJumping() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
    #endregion
}
