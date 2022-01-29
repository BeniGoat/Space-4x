using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Space4x.Assets.Scripts.GameObjects;
using Unity.Space4x.Assets.Scripts.GameObjects.Factories;
using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.Managers
{
        public class SystemManager : MonoBehaviour
        {
                private int systemSize;
                private float orbitDistance;
                private int orbitPointsCount;
                private float planetSpawnRate;

                private IFactory<Orbit> orbitFactory;
                private IFactory<Sun> sunFactory;
                private IFactory<Planet> planetFactory;
                private Sun sun;
                private List<Orbit> mapOrbits;
                private List<GameObject> spots;

                public float Radius => this.systemSize * this.orbitDistance;
                private List<Vector3> MapCoordinates
                {
                        get
                        {
                                if (this.mapOrbits != null && this.mapOrbits.Count > 0)
                                {
                                        IEnumerable<Vector3> orbitCoordinates = this.mapOrbits.SelectMany(x => x.Coordinates);
                                        return new List<Vector3> { Vector3.zero }
                                                .Concat(orbitCoordinates)
                                                .ToList();
                                }

                                return new List<Vector3>();
                        }
                }

                public GameObject OrbitPrefab; 
                public GameObject SunPrefab;
                public GameObject PlanetPrefab;

                /// <summary>
                /// Initialises the system map.
                /// </summary>
                /// <param name="size">The system size (number of orbits).</param>
                /// <param name="orbitDistance">The distance between orbits.</param>
                /// <param name="orbitPointsCount">The number of positions on the orbit.</param>
                /// <param name="planetSpawnRate">The chance that a planet will spawn in an orbit.</param>
                public void Initialise(int size, float orbitDistance, int orbitPointsCount, float planetSpawnRate)
                {
                        this.ClearAllObjects();

                        this.systemSize = size;
                        this.orbitDistance = orbitDistance;
                        this.orbitPointsCount = orbitPointsCount;
                        this.planetSpawnRate = planetSpawnRate;

                        this.orbitFactory = new PrefabFactory<Orbit>(this.OrbitPrefab);
                        this.sunFactory = new PrefabFactory<Sun>(this.SunPrefab);
                        this.planetFactory = new PrefabFactory<Planet>(this.PlanetPrefab);
                        this.mapOrbits = new List<Orbit>();
                        this.spots = new List<GameObject>();

                        this.CreateSystemMap();
                }

                public void ProgressTime()
                {
                        this.StopAllCoroutines();

                        foreach (Orbit orbit in this.mapOrbits)
                        {
                                this.StartCoroutine(orbit.TravelOrbit());
                        }
                }

                /// <summary>
                /// Destroys all gameobjects in the scene.
                /// </summary>
                private void ClearAllObjects()
                {
                        if (this.sun != null)
                        {
                                Destroy(this.sun.gameObject);
                        }

                        if (this.mapOrbits != null)
                        {
                                foreach (var orbit in this.mapOrbits)
                                {
                                        Destroy(orbit.gameObject);
                                }

                                this.mapOrbits.Clear();
                        }

                        if (this.spots != null)
                        {
                                foreach (var spot in this.spots)
                                {
                                        Destroy(spot.gameObject);
                                }

                                this.spots.Clear();
                        }
                }

                /// <summary>
                /// Creates the system map.
                /// </summary>
                private void CreateSystemMap()
                {
                        this.sun = this.sunFactory.Create();
                        this.sun.transform.parent = this.transform;

                        for (int index = 1; index < this.systemSize + 1; index++)
                        {
                                // Gets the orbit coordinates
                                IEnumerable<Vector3> orbitCoordinates = this.GetOrbitCoordinates(index);

                                // Configure the orbit object
                                Orbit orbit = this.CreateOrbit(index, orbitCoordinates);
                                this.mapOrbits.Add(orbit);
                        }
                }

                /// <summary>
                /// Gets the orbit coordinates.
                /// </summary>
                /// <param name="orbitIndex">The index of the orbit.</param>
                /// <returns>The orbit coordinates.</returns>
                private IEnumerable<Vector3> GetOrbitCoordinates(int orbitIndex)
                {
                        var orbitCoordinates = new List<Vector3>();
                        float radians, x, z;

                        // Get number of coordinates in this orbit
                        int numCoordinates = this.GetCoordinateCount(orbitIndex);

                        float degreeBetweenCoordinates = 360f / numCoordinates;
                        for (int i = 0; i < numCoordinates; i++)
                        {
                                // Work out coordinates
                                radians = i * degreeBetweenCoordinates * Mathf.Deg2Rad;

                                x = Mathf.Round(orbitIndex * this.orbitDistance * Mathf.Sin(radians) * 10000f) / 10000f;
                                z = Mathf.Round(orbitIndex * this.orbitDistance * Mathf.Cos(radians) * 10000f) / 10000f;

                                // Add coordinate
                                var coordinate = new Vector3(x, 0f, z);
                                var spot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                spot.transform.localScale = new Vector3(spot.transform.localScale.x, 0.1f, spot.transform.localScale.z);
                                spot.transform.position = coordinate;
                                this.spots.Add(spot);

                                orbitCoordinates.Add(coordinate);
                        }

                        return orbitCoordinates;
                }

                /// <summary>
                /// Configures the orbit object.
                /// </summary>
                /// <param name="orbitalIndex">The index of the orbit.</param>
                /// <param name="orbitCoordinates">The coordinates of the orbit.</param>
                private Orbit CreateOrbit(int orbitalIndex, IEnumerable<Vector3> orbitCoordinates)
                {
                        Orbit orbit = this.orbitFactory.Create();

                        orbit.Coordinates = orbitCoordinates.ToArray();
                        orbit.OrbitDistanceIndex = orbitalIndex;
                        orbit.SetOrbitLine(this.orbitDistance);
                        foreach(GameObject spot in this.spots)
                        {
                                spot.transform.parent = orbit.transform;
                        }

                        if (this.planetSpawnRate > UnityEngine.Random.Range(0.0f, 0.99f))
                        {
                                orbit.OrbitPositionIndex = (int)(UnityEngine.Random.Range(0.0f, 0.99f) * orbit.Coordinates.Length);

                                orbit.Body = this.planetFactory.Create();
                                orbit.Body.AngleOfTravelPerTurn = 360f / orbit.Coordinates.Length;
                                orbit.Body.transform.position = orbit.Coordinates[orbit.OrbitPositionIndex];
                                orbit.Body.transform.parent = orbit.transform;
                        }

                        return orbit;
                }

                /// <summary>
                /// Gets the coordinate count of the orbit specified by the distance from the centre.
                /// </summary>
                /// <param name="orbitalDistance">The distance from the centre.</param>
                /// <returns>The coordinate count.</returns>
                private int GetCoordinateCount(int orbitalDistance)
                {
                        return orbitalDistance > 0
                                ? orbitalDistance * orbitPointsCount
                                : 0;
                }
        }
}