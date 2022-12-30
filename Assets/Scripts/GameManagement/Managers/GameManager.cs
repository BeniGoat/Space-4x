using Space4x.Models;
using Space4x.Models.Factories;
using Space4x.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace Space4x.GameManagement.Managers
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
                                if (instance == null)
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

                [SerializeField] private int systemSize = 25;
                [SerializeField] private float orbitDistance = 25f;
                [SerializeField] private int orbitPointsCount = 6;
                [SerializeField] private float planetSpawnRate = 1f;
                [SerializeField] private float maxPlanetSize = 10f;
                [SerializeField] private float minPlanetSize = 2f;

                [SerializeField] private float rotationSpeed = 180f;
                [SerializeField] private float moveSpeedMinZoom = 400f;
                [SerializeField] private float moveSpeedMaxZoom = 1f;
                [SerializeField] private float stickMinZoom = -250f;
                [SerializeField] private float stickMaxZoom = -45f;
                [SerializeField] private float swivelMinZoom = 90f;
                [SerializeField] private float swivelMaxZoom = 45f;

                private IFactory<SolarSystem> systemFactory;
                private List<SolarSystem> systems;

                public CameraController CameraController = default;
                public InputManager InputManager = default;
                public SelectionManager selectionManager = default;
                public GameObject SystemPrefab;
                public GameObject OrbitPrefab;
                public GameObject SunPrefab;
                public GameObject PlanetPrefab;
                public GameObject MapGridPrefab;
                public GameObject MapLinePrefab;

                public SystemSettings SystemSettings { get; set; }
                public GameState State { get; set; }

                private void Awake()
                {
                        instance = this;
                        this.systems = new List<SolarSystem>();
                }

                private void Start()
                {
                        this.State = GameState.Start;
                        this.StartNewGame();
                }

                /// <summary>
                /// Starts a new game.
                /// </summary>
                private void StartNewGame()
                {
                        this.systemFactory = new PrefabFactory<SolarSystem>(this.SystemPrefab);
                        this.SystemSettings = new SystemSettings
                        {
                                OrbitalSeparationDistance = this.orbitDistance,
                                OrbitPointsCount = this.orbitPointsCount,
                                PlanetSpawnRate = this.planetSpawnRate,
                                MaxBodySize = this.maxPlanetSize,
                                MinBodySize = this.minPlanetSize,
                                SystemSize = this.systemSize
                        };
                        SolarSystem startingSystem = this.systemFactory.Create();
                        startingSystem.Initialise();
                        this.systems.Add(startingSystem);

                        var cameraSettings = new CameraSettings
                        {
                                Limit =  this.SystemSettings.SystemSize * this.SystemSettings.OrbitalSeparationDistance,
                                RotationSpeed = this.rotationSpeed,
                                MoveSpeedMaxZoom = this.moveSpeedMaxZoom,
                                MoveSpeedMinZoom = this.moveSpeedMinZoom,
                                StickMaxZoom = this.stickMaxZoom,
                                StickMinZoom = this.stickMinZoom,
                                SwivelMaxZoom = this.swivelMaxZoom,
                                SwivelMinZoom = this.swivelMinZoom
                        };
                        this.CameraController.Initialise(cameraSettings);

                        this.State = GameState.PlayerTurn;
                }
        }
}
