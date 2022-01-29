using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.Managers
{
        /// <summary>
        /// The game manager class.
        /// </summary>
        public class GameManager : MonoBehaviour
        {
                /// <summary>
                /// The game manager instance
                /// </summary>
                private static GameManager instance;

                /// <summary>
                /// Gets the instance of the game manager, or creates a new instance if not already instantiated
                /// </summary>
                public static GameManager Instance
                {
                        get
                        {
                                if (!instance)
                                {
                                        instance = new GameObject().AddComponent<GameManager>();
                                        // name it for easy recognition
                                        instance.name = instance.GetType().ToString();
                                        // mark root as DontDestroyOnLoad();
                                        DontDestroyOnLoad(instance.gameObject);
                                }
                                return instance;
                        }
                }

                /// <summary>
                /// The current game state
                /// </summary>
                public GameState State;

                [SerializeField]
                private KeyCode newGameKey = KeyCode.N;
                [SerializeField]
                private KeyCode endTurnKey = KeyCode.Space;

                [SerializeField]
                private int systemSize = 3;
                [SerializeField]
                private float orbitDistance = 10f;
                [SerializeField]
                private int orbitPointsCount = 6;
                [SerializeField]
                private float planetSpawnRate = 1f;

                [SerializeField]
                private SystemManager systemManager = default;
                [SerializeField]
                private SelectionManager selectionManager = default;

                private void Awake()
                {
                        instance = this;
                }

                private void Start()
                {
                        this.BeginNewGame();
                }

                private void Update()
                {
                        this.HandleInput();
                }

                private void HandleInput()
                {
                        if (Input.GetKeyDown(this.newGameKey))
                        {
                                this.ChangeGameState(GameState.PlayerTurn);
                                this.BeginNewGame();
                        }
                        else if (Input.GetKeyDown(this.endTurnKey) && this.State == GameState.PlayerTurn)
                        {
                                this.ChangeGameState(GameState.ProcessingTurn);
                        }
                        else if (Input.GetMouseButtonDown(0))
                        {
                                this.selectionManager.SelectObject();
                        }
                }

                /// <summary>
                /// Changes the current game state.
                /// </summary>
                /// <param name="gameState">The new game state.</param>
                public void ChangeGameState(GameState gameState)
                {
                        this.State = gameState;

                        switch (gameState)
                        {
                                case GameState.PlayerTurn:
                                        // Enable player controls
                                        break;
                                case GameState.ProcessingTurn:
                                        this.ProcessTurn();
                                        break;
                                default:
                                        break;
                        };
                }

                /// <summary>
                /// Begins a new game.
                /// </summary>
                private void BeginNewGame()
                {
                        this.systemManager.Initialise(
                                this.systemSize,
                                this.orbitDistance,
                                this.orbitPointsCount,
                                this.planetSpawnRate);
                }

                /// <summary>
                /// Processes a turn.
                /// </summary>
                private void ProcessTurn()
                {
                        this.systemManager.ProgressTime();
                }

                /// <summary>
                /// Performs validation on the set game values.
                /// </summary>
                private void OnValidate()
                {
                        // Clamp size of system to be between 1 and 10
                        if (this.systemSize < 1)
                        {
                                this.systemSize = 1;
                        }
                        else if (this.systemSize > 10)
                        {
                                this.systemSize = 10;
                        }
                }
        }
}
