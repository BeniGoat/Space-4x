using System.Collections;
using UnityEngine;

namespace Space4x.Models
{
        public class Planet : SystemBody
        {
                private void Start()
                {
                        this.OrbitRadius = Vector3.Distance(Vector3.zero, this.Transform.position);
                }

                public override IEnumerator Move(Vector3 destination)
                {
                        float angle = 0f;
                        this.Direction = (this.Transform.position - Vector3.zero).normalized;

                        while (angle < this.AngleOfTravelPerTurn)
                        {
                                Vector3 orbit = Vector3.forward * this.OrbitRadius;
                                orbit = Quaternion.LookRotation(this.Direction) * Quaternion.Euler(0, angle, 0) * orbit;

                                this.Transform.position = Vector3.zero + orbit;
                                angle += this.AngleOfTravelPerTurn * Time.deltaTime;

                                // Yield here
                                yield return null;
                        }

                        this.Transform.position = destination;
                }
        }
}