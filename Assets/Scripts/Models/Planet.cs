using Space4x.Models.Components;
using System;
using System.Collections;
using UnityEngine;

namespace Space4x.Models
{
        [RequireComponent(typeof(LineRenderer))]
        public class Planet : SystemBody
        {
                private float orbitRadius;
                private float angleOfTravelPerTurn;
                private Orbiter orbiter;

                [SerializeField] private Color sunLineColor;
                private Color sunLineColorStart;
                private LineRenderer sunLine;

                /// <inheritdoc/>
                protected override void Awake()
                {
                        base.Awake();
                        this.orbiter = this.ParentTransform.GetComponent<Orbiter>();
                        this.Rotator.SetRotationSpeed(24f);

                        this.sunLineColorStart = new Color(this.sunLineColor.r, this.sunLineColor.g, this.sunLineColor.b, this.sunLineColor.a * 0.1f);

                        this.ConfigureSunLine();
                }

                /// <summary>
                /// Method called before any Update calls are made.
                /// </summary>
                private void Start()
                {
                        this.DrawSunLine();
                }

                /// <summary>
                /// Initialises the orbital position of the body.
                /// </summary>
                /// <param name="position">The initial position.</param>
                /// <param name="angleOfTravelPerTurn">The angle of travel per turn.</param>
                public virtual void InitialiseOrbitalPosition(Vector3 position, float angleOfTravelPerTurn)
                {
                        this.orbitRadius = Vector3.Distance(Vector3.zero, position);
                        this.ParentTransform.position = position;
                        this.angleOfTravelPerTurn = angleOfTravelPerTurn;
                }

                public override IEnumerator MoveAlongOrbit(Vector3 destination)
                {
                        yield return this.orbiter.Move(destination, this.angleOfTravelPerTurn, this.orbitRadius, this.sunLine);
                }

                private void ConfigureSunLine()
                {
                        this.sunLine = this.GetComponent<LineRenderer>();
                        this.sunLine.useWorldSpace = true;
                        this.sunLine.startWidth = 0.2f;
                        this.sunLine.endWidth = 0.2f;
                        this.sunLine.startColor = this.sunLineColorStart;
                        this.sunLine.endColor = this.sunLineColor;
                        this.sunLine.positionCount = 2;
                }

                private void DrawSunLine()
                {
                        this.sunLine.SetPosition(0, Vector3.zero);
                        this.sunLine.SetPosition(1, this.ParentTransform.position);
                }
        }
}