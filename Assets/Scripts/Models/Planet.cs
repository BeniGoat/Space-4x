using System.Collections;
using UnityEngine;

namespace Space4x.Models
{
        [RequireComponent(typeof(LineRenderer))]
        public class Planet : SystemBody
        {
                [SerializeField] private Color sunLineColor;
                private Color sunLineColorStart;
                private LineRenderer line;

                private void Start()
                {
                        this.sunLineColorStart = new Color(this.sunLineColor.r, this.sunLineColor.g, this.sunLineColor.b, this.sunLineColor.a * 0.1f);

                        this.ConfigureSunLine();
                        this.DrawSunLine();
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
                                this.line.SetPosition(1, this.Transform.position);
                                angle += this.AngleOfTravelPerTurn * Time.deltaTime;

                                // Yield heres
                                yield return null;
                        }

                        this.Transform.position = destination;
                }

                private void ConfigureSunLine()
                {
                        this.line = this.GetComponent<LineRenderer>();
                        this.line.useWorldSpace = true;
                        this.line.startWidth = 0.2f;
                        this.line.endWidth = 0.2f;
                        this.line.startColor = this.sunLineColorStart;
                        this.line.endColor = this.sunLineColor;
                        this.line.positionCount = 2;
                }

                private void DrawSunLine()
                {
                        this.line.SetPosition(0, Vector3.zero);
                        this.line.SetPosition(1, this.Transform.position);
                }
        }
}