using System.Collections;
using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.GameObjects
{
        [RequireComponent(typeof(LineRenderer))]
        public class Orbit : MonoBehaviour
        {
                private LineRenderer line;

                public Vector3[] Coordinates { get; set; }
                public SystemBody Body { get; set; }
                public int OrbitDistanceIndex { get; set; }
                public int OrbitPositionIndex { get; set; }

                private void Awake()
                {
                        this.line = this.GetComponent<LineRenderer>();
                        this.line.useWorldSpace = true;
                        this.line.startWidth = 0.1f;
                        this.line.endWidth = 0.1f;
                        this.line.startColor = Color.white;
                        this.line.endColor = Color.white;
                }

                public IEnumerator TravelOrbit()
                {
                        int nextIndex = (this.OrbitPositionIndex + 1) % this.Coordinates.Length;
                        yield return this.Body.GoToNextOrbitPosition(this.Coordinates[nextIndex]);
                        this.OrbitPositionIndex = nextIndex;

                        Managers.GameManager.Instance.ChangeGameState(GameState.PlayerTurn);
                }

                internal void SetOrbitLine(float orbitalDistance)
                {
                        this.line.positionCount = this.OrbitDistanceIndex * 360 + 1;
                        float degreeBetweenCoordinates = 1f / this.OrbitDistanceIndex;

                        Vector3 pos;
                        float theta;
                        for (int i = 0; i < this.line.positionCount; i++)
                        {
                                theta = i * degreeBetweenCoordinates * Mathf.Deg2Rad;
                                pos = new Vector3(
                                        this.OrbitDistanceIndex * orbitalDistance * Mathf.Cos(theta),
                                        0,
                                        this.OrbitDistanceIndex * orbitalDistance * Mathf.Sin(theta));
                                this.line.SetPosition(i, pos);
                        }
                }
        }
}