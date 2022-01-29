using Unity.Space4x.Assets.Scripts.GameObjects;
using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.Managers
{
        public class SelectionManager : MonoBehaviour
        {
                [SerializeField]
                private GameObject selectedObject;

                public void SelectObject()
                {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                                GameObject clickedObject = this.GetObject(hit.transform);
                                if (this.selectedObject == null || clickedObject.name != this.selectedObject.name)  //TODO: Add proper system body comparer
                                {
                                        this.selectedObject = clickedObject;
                                }
                        }
                        else
                        {
                                this.selectedObject = null;
                        }
                }

                private GameObject GetObject(Transform transformHit)
                {
                        if (transformHit.GetComponent<SystemBody>() is SystemBody systemBody)
                        {
                                return systemBody.gameObject;
                        }

                        return null;
                }
        }
}