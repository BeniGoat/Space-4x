using System.Collections;
using UnityEngine;

namespace Space4x.Models
{
        [RequireComponent(typeof(SphereCollider))]
        public abstract class SystemBody : MonoBehaviour
        {
                protected Vector3 Direction;
                protected Transform Transform;
                protected float OrbitRadius;
                protected float AngleOfTravelPerTurn;

                private void Awake()
                {
                        this.Transform = this.transform;
                }

                public virtual IEnumerator Move(Vector3 destination)
                {
                        yield return null;
                }

                public void Initialise(Vector3 position, float angleOfTravelPerTurn)
                {
                        this.transform.position = position;
                        this.AngleOfTravelPerTurn = angleOfTravelPerTurn;
                }
        }
}