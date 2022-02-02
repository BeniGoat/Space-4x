using Space4x.Models;
using UnityEngine;

namespace Space4x.GameManagement.Managers
{
        public class SelectionManager : MonoBehaviour
        {
                [SerializeField] private SystemBody hoveredSystemBody;
                [SerializeField] private SystemBody selectedSystemBody;

                private void Update()
                {
                        this.SelectObject();
                }

                public void SelectObject()
                {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                                SystemBody currentSystemBodyPointedAt = this.GetObject<SystemBody>(hit.transform);
                                if (currentSystemBodyPointedAt != null)
                                {
                                        if (this.hoveredSystemBody == null || currentSystemBodyPointedAt.transform.position != this.hoveredSystemBody.transform.position)  //TODO: Add proper system body comparer
                                        {
                                                this.hoveredSystemBody = currentSystemBodyPointedAt;
                                        }

                                        if (Input.GetMouseButtonDown(0))
                                        {
                                                if (this.selectedSystemBody == null || currentSystemBodyPointedAt.transform.position != this.selectedSystemBody.transform.position)  //TODO: Add proper system body comparer
                                                {
                                                        this.selectedSystemBody = currentSystemBodyPointedAt;
                                                }
                                        }
                                }
                                else
                                {
                                        this.hoveredSystemBody = null;
                                }
                        }
                        else
                        {
                                this.hoveredSystemBody = null;
                        }
                }

                private T GetObject<T>(Transform transformHit) where T : class
                {
                        return transformHit.GetComponent<T>() is T obj ? obj : null;
                }
        }
}