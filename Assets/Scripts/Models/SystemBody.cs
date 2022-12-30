using Space4x.Models.Components;
using System.Collections;
using UnityEngine;

namespace Space4x.Models
{
        /// <summary>
        /// The basic system body class.
        /// </summary>
        [RequireComponent(typeof(SphereCollider))]
        public abstract class SystemBody : MonoBehaviour
        {
                // Fields available to child objects
                protected Rotator Rotator;
                protected Canvas InfoCanvas;
                protected Transform ParentTransform;

                // Private fields
                private Material material;
                private Color originalColor;
                private Color highlightedColor;
                private bool isHighlighted;
                private bool isSelected;

                /// <summary>
                /// Method called when the script instance is loaded.
                /// </summary>
                protected virtual void Awake()
                {
                        // Set the initial components and their values.
                        this.Rotator = this.GetComponent<Rotator>();
                        this.InfoCanvas = this.transform.parent.GetComponentInChildren<Canvas>();
                        this.ParentTransform = this.transform.parent.transform;

                        this.material = this.GetComponent<Renderer>().material;
                        this.originalColor = this.material.color;
                        this.highlightedColor = new Color(this.originalColor.r * 0.6f, this.originalColor.g * 0.6f, this.originalColor.b * 0.6f);
                }

                /// <summary>
                /// Method called every frame.
                /// </summary>
                private void Update()
                {
                        this.HandleSelection();
                }

                /// <summary>
                /// Sets the parent of the system body;
                /// </summary>
                /// <param name="transform">The parent transform.</param>
                public void SetParent(Transform transform)
                {
                        this.ParentTransform.parent = transform;
                }

                /// <summary>
                /// Moves the system body.
                /// </summary>
                /// <param name="destination">The location the body is moved to.</param>
                /// <returns>An enumerator.</returns>
                public virtual IEnumerator MoveAlongOrbit(Vector3 destination)
                {
                        yield return null;
                }

                /// <summary>
                /// Toggles the highlighted flag.
                /// </summary>
                /// <param name="isHighlighted">Whether to set the flag to on/off.</param>
                public void ToggleHighlight(bool isHighlighted)
                {
                        this.isHighlighted = isHighlighted;
                }

                /// <summary>
                /// Toggles the selected flag.
                /// </summary>
                /// <param name="isSelected">Whether to set the flag to on/off.</param>
                public void ToggleSelect(bool isSelected)
                {
                        this.isSelected = isSelected;
                }

                /// <summary>
                /// Handles the selection/highlighting of the system body.
                /// </summary>
                private void HandleSelection()
                {
                        // Enable the system body UI if it is either highlighted or selected
                        this.InfoCanvas.enabled = this.isSelected || this.isHighlighted;

                        if (this.isHighlighted)
                        {
                                // If highlighted, change to highlighted colour
                                this.material.color = this.material.color != this.highlightedColor
                                        ? this.highlightedColor
                                        : this.material.color;
                        }
                        else
                        {
                                // If not, revert to original colour
                                this.material.color = this.material.color != this.originalColor
                                        ? this.originalColor
                                        : this.material.color;
                        }
                }
        }
}