using Space4x.GameManagement.Managers;
using Space4x.Models.Factories;
using System.Collections.Generic;
using UnityEngine;

namespace Space4x.Models
{
        public class SolarSystem : MonoBehaviour
        {
                private Transform systemTransform;
                private IFactory<Orbit> orbitFactory;
                private IFactory<Sun> sunFactory;
                private IFactory<MapGrid> mapGridFactory;
                private List<Orbit> orbits;
                private MapGrid mapGrid;
                private Sun sun;

                public bool IsSelected { get; set; }

                private void Awake()
                {
                        this.systemTransform = this.transform;
                        this.sunFactory = new PrefabFactory<Sun>(GameManager.Instance.SunPrefab);
                        this.orbitFactory = new PrefabFactory<Orbit>(GameManager.Instance.OrbitPrefab);
                        this.mapGridFactory = new PrefabFactory<MapGrid>(GameManager.Instance.MapGridPrefab);

                        this.orbits = new List<Orbit>();

                        this.IsSelected = false;
                }

                /// <summary>
                /// Initialises the system map.
                /// </summary>
                public void Initialise()
                {
                        this.IsSelected = true;

                        this.mapGrid = this.mapGridFactory.Create();
                        this.mapGrid.transform.parent = this.systemTransform;
                        this.sun = this.sunFactory.Create();
                        this.sun.SetParent(this.systemTransform);

                        for (int index = 1; index < GameManager.Instance.SystemSettings.SystemSize + 1; index++)
                        {
                                // Configure the orbit object
                                Orbit orbit = this.orbitFactory.Create();

                                orbit.Initialise(index);
                                orbit.transform.parent = this.systemTransform;
                                this.orbits.Add(orbit);
                        }

                        List<MapLine> mapLines = this.orbits[GameManager.Instance.SystemSettings.SystemSize - 1].GetMapLines();
                        this.mapGrid.SetLines(mapLines);
                }
        }
}
