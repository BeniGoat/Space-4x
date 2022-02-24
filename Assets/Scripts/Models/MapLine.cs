using UnityEngine;

namespace Space4x.Models
{
        [RequireComponent(typeof(LineRenderer))]
        public class MapLine : MonoBehaviour
        {
                private LineRenderer lineRenderer;

                private void Awake()
                {
                        this.lineRenderer = this.GetComponent<LineRenderer>();
                        this.lineRenderer.positionCount = 2;
                        this.lineRenderer.useWorldSpace = true;
                        this.lineRenderer.startColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);
                        this.lineRenderer.endColor = this.lineRenderer.startColor;
                }

                public void Configure(float lineWidth, Vector3 position)
                {
                        this.lineRenderer.startWidth = lineWidth;
                        this.lineRenderer.endWidth = lineWidth;
                        this.lineRenderer.SetPosition(0, Vector3.zero);
                        this.lineRenderer.SetPosition(1, position);
                }
        }
}