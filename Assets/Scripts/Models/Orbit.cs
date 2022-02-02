using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space4x.Models
{
        [RequireComponent(typeof(LineRenderer))]
        public class Orbit : MonoBehaviour
        {
                private LineRenderer line;
                private List<GameObject> spots;
                private float orbitalDistance;

                private Coordinate[] coordinates;
                private SystemBody body;
                private int orbitPositionIndex;

                private void Awake()
                {
                        this.ConfigureOrbitLine();
                        this.spots = new List<GameObject>();
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
                /// Configures the orbit object and body, if one exists on it.
                /// </summary>
                /// <param name="orbitalIndex">The index of the orbit.</param>
                /// <param name="orbitalSeperationDistance">The orbital separation distance.</param>
                /// <param name="orbitCoordinates">The coordinates of the orbit.</param>
                public void Initialise(int orbitalIndex, float orbitalSeperationDistance, int numCoordinates, SystemBody body)
                {
                        this.DrawOrbitLine(orbitalIndex);

                        this.coordinates = this.GetOrbitCoordinates(numCoordinates);
                        foreach (Coordinate coordinate in this.coordinates)
                        {
                                GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                spot.transform.localScale = new Vector3(spot.transform.localScale.x, 0.1f, spot.transform.localScale.z);
                                spot.transform.position = coordinate.Position;
                                spot.transform.parent = this.transform;
                                this.spots.Add(spot);
                        }

                        if (body != null)
                        {
                                this.orbitPositionIndex = (int)(Random.Range(0.0f, 0.99f) * numCoordinates);

                                body.Initialise(this.coordinates[this.orbitPositionIndex].Position, 360f / numCoordinates);
                                body.transform.parent = this.transform;
                        }

                        this.orbitalDistance = orbitalIndex * orbitalSeperationDistance;
                        this.body = body;
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

                private void ConfigureOrbitLine()
                {
                        this.line = this.GetComponent<LineRenderer>();
                        this.line.useWorldSpace = true;
                        this.line.startWidth = 0.1f;
                        this.line.endWidth = 0.1f;
                        this.line.startColor = Color.white;
                        this.line.endColor = Color.white;
                }
        }
}