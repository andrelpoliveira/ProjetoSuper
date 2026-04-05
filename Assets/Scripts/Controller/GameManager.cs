using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Presentation,
        WaitForGesture,
        Playing
    }

    [Header("Game State")]
    public GameState currentState;

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPresentation();
    }
    private void Update()
    {
        if(currentState == GameState.WaitForGesture)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Fallback acionado!");
                StartGame();
            }
        }
    }
    #endregion

    #region Estado de vídeo
    /// <summary>
    /// Início do vídeo
    /// </summary>
    public void StartPresentation()
    {
        currentState = GameState.Presentation;
        Debug.Log("Rodando apresentação...");

        Invoke(nameof(EndPresentation), 5f);

    }
    /// <summary>
    /// Pós vídeo - aguarda gesto para iniciar
    /// </summary>
    public void EndPresentation()
    {
        currentState = GameState.WaitForGesture;
        Debug.Log("Aguardadndo gesto...");
    }
    #endregion

    #region Game
    public void StartGame()
    {
        currentState = GameState.Playing;
        Debug.Log("Jogo iniciado!");
    }
    #endregion
}
