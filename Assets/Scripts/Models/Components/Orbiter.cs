using System.Collections;
using UnityEngine;

namespace Space4x.Models.Components
{
        public class Orbiter : MonoBehaviour
        {
                /// <summary>
                /// Moves a body along an orbit.
                /// </summary>
                /// <param name="destination">The location the body is moved to.</param>
                /// <param name="angleOfTravelPerTurn">The angle of travel per turn.</param>
                /// <param name="orbitRadius">The radius of the orbit.</param>
                /// <param name="sunLine">The line connecting the body to the sun.</param>
                /// <returns>An enumerator.</returns>
                public IEnumerator Move(Vector3 destination, float angleOfTravelPerTurn, float orbitRadius, LineRenderer sunLine = null)
                {
                        float angle = 0f;
                        Vector3 direction = (this.transform.position - Vector3.zero).normalized;

                        while (angle < angleOfTravelPerTurn)
                        {
                                Vector3 orbit = Vector3.forward * orbitRadius;
                                orbit = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * orbit;

                                this.transform.position = Vector3.zero + orbit;
                                if (sunLine != null)
                                {
                                        sunLine.SetPosition(1, this.transform.position);
                                }

                                angle += angleOfTravelPerTurn * Time.deltaTime;

                                // Yield heres
                                yield return null;
                        }

                        this.transform.position = destination;
                }
        }
}
