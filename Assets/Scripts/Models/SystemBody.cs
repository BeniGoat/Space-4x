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
                protected Canvas InfoCanvas;
                protected Material material;
                protected Color originalColor;
                protected Color highlightedColor;
                protected bool isHighlighted;
                protected bool isSelected;

                private void Awake()
                {
                        this.Transform = this.transform;
                        this.InfoCanvas = this.GetComponentInChildren<Canvas>();
                        this.material = this.GetComponent<Renderer>().material;
                        this.originalColor = this.material.color;
                        this.highlightedColor = new Color(this.originalColor.r * 0.6f, this.originalColor.g * 0.6f, this.originalColor.b * 0.6f);
                }

                private void Update()
                {
                        this.InfoCanvas.enabled = this.isSelected || this.isHighlighted;

                        Color newColor = this.isHighlighted 
                                ? this.highlightedColor 
                                : this.originalColor;
                        this.material.color = this.material.color != newColor 
                                ? newColor 
                                : this.material.color;
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

                public void ToggleHighlight(bool isHighlighted)
                {
                        this.isHighlighted = isHighlighted;
                }

                public void ToggleSelect(bool isSelected)
                {
                        this.isSelected = isSelected;
                }
        }
}