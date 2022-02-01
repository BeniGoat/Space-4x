using Space4x.GameManagement.Managers;
using Space4x.Models.Factories;
using Space4x.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace Space4x.Models
{
        public class SolarSystem : MonoBehaviour
        {
                private Transform Transform;
                private IFactory<Orbit> orbitFactory;
                private IFactory<Sun> sunFactory;
                private IFactory<Planet> planetFactory;
                private List<Orbit> mapOrbits;
                private SystemSettings systemSettings;
                private Sun sun;

                public bool IsSelected { get; set; }
                public float Radius => this.systemSettings.SystemSize * this.systemSettings.OrbitalSeperationDistance;

                private void Awake()
                {
                        this.Transform = this.transform;
                        this.sunFactory = new PrefabFactory<Sun>(GameManager.Instance.SunPrefab);
                        this.orbitFactory = new PrefabFactory<Orbit>(GameManager.Instance.OrbitPrefab);
                        this.planetFactory = new PrefabFactory<Planet>(GameManager.Instance.PlanetPrefab);

                        this.mapOrbits = new List<Orbit>();

                        this.IsSelected = false;
                }

                /// <summary>
                /// Initialises the system map.
                /// </summary>
                /// <param name="systemSettings">The systemsettings.</param>
                public void Initialise(SystemSettings systemSettings)
                {
                        this.sun = this.sunFactory.Create();
                        this.sun.transform.parent = this.Transform;

                        for (int index = 1; index < systemSettings.SystemSize + 1; index++)
                        {
                                // Configure the orbit object
                                Orbit orbit = this.orbitFactory.Create();
                                SystemBody body = systemSettings.PlanetSpawnRate > Random.Range(0.0f, 0.99f)
                                        ? this.planetFactory.Create()
                                        : null;

                                orbit.Initialise(index, systemSettings.OrbitalSeperationDistance, systemSettings.OrbitPointsCount * index, body);
                                orbit.transform.parent = this.Transform;
                                this.mapOrbits.Add(orbit);
                        }

                        this.systemSettings = systemSettings;
                }

                /// <summary>
                /// Progresses time within the <see cref="SolarSystem"/> for a turn.
                /// </summary>
                public void ProgressTime()
                {
                        this.StopAllCoroutines();

                        foreach (Orbit orbit in this.mapOrbits)
                        {
                                this.StartCoroutine(orbit.TravelOrbit());
                        }
                }
        }
}
