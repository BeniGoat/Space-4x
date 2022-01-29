using System.Collections;
using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.GameObjects
{
        [RequireComponent(typeof(SphereCollider))]
        public abstract class SystemBody : MonoBehaviour
        {
                private Vector3 direction;
                private float orbitRadius;
                public float AngleOfTravelPerTurn { get; set; }


                private void Start()
                {
                        this.orbitRadius = Vector3.Distance(Vector3.zero, this.transform.position);
                }

                public IEnumerator GoToNextOrbitPosition(Vector3 destination)
                {
                        yield return this.Travel(destination);
                }

                private IEnumerator Travel(Vector3 destination)
                {
                        float angle = 0f;
                        this.direction = (this.transform.position - Vector3.zero).normalized;

                        while (angle < this.AngleOfTravelPerTurn)
                        {
                                Vector3 orbit = Vector3.forward * this.orbitRadius;
                                orbit = Quaternion.LookRotation(this.direction) * Quaternion.Euler(0, angle, 0) * orbit;

                                this.transform.position = Vector3.zero + orbit;
                                angle += this.AngleOfTravelPerTurn * Time.deltaTime;

                                // Yield here
                                yield return null;
                        }

                        this.transform.position = destination;
                        //yield return null;
                }
        }
}