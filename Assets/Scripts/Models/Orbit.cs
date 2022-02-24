using Space4x.GameManagement.Managers;
using Space4x.Models.Factories;
using System.Collections;
using System.Collections.Generic;
using Unity.Space4x.Assets.Scripts.MathFunction;
using UnityEngine;

namespace Space4x.Models
{
        [RequireComponent(typeof(LineRenderer))]
        public class Orbit : MonoBehaviour
        {
                private IFactory<MapLine> mapLineFactory;
                private IFactory<Planet> planetFactory;
                private Transform orbitTransform;
                private LineRenderer line;
                private List<GameObject> spots;
                private float orbitalDistance;

                private SystemBody body;
                private int orbitPositionIndex;

                private Coordinate[] coordinates;

                private void Awake()
                {
                        this.orbitTransform = this.transform;
                        this.spots = new List<GameObject>();
                        this.planetFactory = new PrefabFactory<Planet>(GameManager.Instance.PlanetPrefab);

                        this.line = this.GetComponent<LineRenderer>();
                        this.line.useWorldSpace = true;
                        this.line.startWidth = 0.1f;
                        this.line.endWidth = 0.1f;
                        this.line.startColor = Color.white;
                        this.line.endColor = Color.white;
                }

                /// <summary>
                /// Moves the orbital body along the orbit line.
                /// </summary>
                /// <returns></returns>
                public IEnumerator TravelOrbit()
                {
                        int nextIndex = (this.orbitPositionIndex + 1) % this.coordinates.Length;
                        yield return this.body.Move(this.coordinates[nextIndex].Position);
                        this.orbitPositionIndex = nextIndex;
                }

                /// <summary>
                /// Configures the orbit object.
                /// </summary>
                /// <param name="orbitalIndex">The index of the orbit.</param>
                public void Initialise(int orbitalIndex)
                {
                        int numCoordinates = GameManager.Instance.SystemSettings.OrbitPointsCount * orbitalIndex;
                        this.orbitalDistance = GameManager.Instance.SystemSettings.OrbitalSeperationDistance * orbitalIndex;

                        this.coordinates = this.GetOrbitCoordinates(numCoordinates);
                        for (int i = 0; i < this.coordinates.Length; i++)
                        {
                                var coordinate = this.coordinates[i];
                                GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                spot.name = $"Coordinate_{i}";
                                spot.transform.localScale = new Vector3(spot.transform.localScale.x, 0.1f, spot.transform.localScale.z);
                                spot.transform.position = coordinate.Position;
                                spot.transform.parent = this.orbitTransform;
                                this.spots.Add(spot);
                        }

                        if (GameManager.Instance.SystemSettings.PlanetSpawnRate > Random.Range(0.0f, 0.99f))
                        {
                                this.orbitPositionIndex = (int)(Random.Range(0.0f, 0.99f) * numCoordinates);

                                this.body = this.planetFactory.Create();
                                this.body.transform.localScale *= this.GetPlanetSize();
                                this.body.Initialise(this.coordinates[this.orbitPositionIndex].Position, 360f / numCoordinates);
                                this.body.transform.parent = this.orbitTransform;
                        }

                        this.DrawOrbitLine(orbitalIndex);
                }

                private float GetPlanetSize()
                {
                        return NumberGeneration.BellCurve(
                                GameManager.Instance.SystemSettings.MinPlanetSize,
                                GameManager.Instance.SystemSettings.MaxPlanetSize,
                                GameManager.Instance.SystemSettings.MaxPlanetSize * 0.5f);
                }

                public List<MapLine> GetMapLines()
                {
                        if (this.mapLineFactory == null)
                        {
                                this.mapLineFactory = new PrefabFactory<MapLine>(GameManager.Instance.MapLinePrefab);
                        }

                        var mapLines = new List<MapLine>();

                        float lineWidth;
                        for (int i = 0; i < this.coordinates.Length; i++)
                        {
                                lineWidth = i % (GameManager.Instance.SystemSettings.OrbitPointsCount - 1) == 0 ? 0.25f : 0.05f;

                                MapLine mapLine = this.mapLineFactory.Create();
                                mapLine.Configure(lineWidth, this.coordinates[i].Position);
                                mapLines.Add(mapLine);
                        }

                        return mapLines;
                }

                /// <summary>
                /// Gets the orbit coordinates.
                /// </summary>
                /// <param name="orbitIndex">The index of the orbit.</param>
                /// <returns>The orbit coordinates.</returns>
                private Coordinate[] GetOrbitCoordinates(int numCoordinates)
                {
                        // Get number of coordinates in this orbit
                        var orbitCoordinates = new Coordinate[numCoordinates];
                        float degreeBetweenCoordinates = 360f / numCoordinates;

                        float radians, x, z;
                        for (int i = 0; i < numCoordinates; i++)
                        {
                                // Work out coordinates
                                radians = degreeBetweenCoordinates * i * Mathf.Deg2Rad;

                                x = Mathf.Round(this.orbitalDistance * Mathf.Sin(radians) * 10000f) / 10000f;
                                z = Mathf.Round(this.orbitalDistance * Mathf.Cos(radians) * 10000f) / 10000f;

                                // Add coordinate
                                orbitCoordinates[i] = new Coordinate(x, z, degreeBetweenCoordinates * i);
                        }

                        return orbitCoordinates;
                }

                private void DrawOrbitLine(int orbitDistanceIndex)
                {
                        this.line.positionCount = orbitDistanceIndex * 360 + 1;
                        float degreeBetweenCoordinates = 1f / orbitDistanceIndex;

                        Vector3 pos;
                        float theta;
                        for (int i = 0; i < this.line.positionCount; i++)
                        {
                                theta = i * degreeBetweenCoordinates * Mathf.Deg2Rad;
                                pos = new Vector3(this.orbitalDistance * Mathf.Cos(theta), 0, this.orbitalDistance * Mathf.Sin(theta));
                                this.line.SetPosition(i, pos);
                        }
                }
        }
}