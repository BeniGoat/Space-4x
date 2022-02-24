using Space4x.Models;
using UnityEngine;

namespace Space4x.GameManagement.Managers
{
        public class SelectionManager : MonoBehaviour
        {
                [SerializeField] private Texture topLeftBorder;
                [SerializeField] private Texture bottomLeftBorder;
                [SerializeField] private Texture topRightBorder;
                [SerializeField] private Texture bottomRightBorder;

                [SerializeField] private SystemBody hoveredSystemBody;
                [SerializeField] private SystemBody selectedSystemBody;
                private Camera sceneCamera;

                void Start()
                {
                        this.sceneCamera = Camera.main;
                }

                private void Update()
                {
                        this.SelectObject();
                }

                public void SelectObject()
                {
                        Ray ray = this.sceneCamera.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                                SystemBody currentSystemBodyPointedAt = this.GetObject<SystemBody>(hit.transform);
                                if (currentSystemBodyPointedAt != null)
                                {
                                        if (this.hoveredSystemBody == null || currentSystemBodyPointedAt.transform.position != this.hoveredSystemBody.transform.position)  //TODO: Add proper system body comparer
                                        {
                                                this.hoveredSystemBody = currentSystemBodyPointedAt;
                                                this.hoveredSystemBody.ToggleHighlight(true);
                                        }

                                        if (Input.GetMouseButtonDown(0))
                                        {
                                                if (this.selectedSystemBody == null || currentSystemBodyPointedAt.transform.position != this.selectedSystemBody.transform.position)  //TODO: Add proper system body comparer
                                                {
                                                        this.selectedSystemBody = currentSystemBodyPointedAt;
                                                        this.selectedSystemBody.ToggleSelect(true);
                                                }
                                        }
                                }
                                else
                                {
                                        if (this.hoveredSystemBody != null)
                                        {
                                                this.hoveredSystemBody.ToggleHighlight(false);
                                                this.hoveredSystemBody = null;
                                        }
                                }
                        }
                        else
                        {
                                if (this.hoveredSystemBody != null)
                                {
                                        this.hoveredSystemBody.ToggleHighlight(false);
                                        this.hoveredSystemBody = null;
                                }

                                if (Input.GetMouseButtonDown(0))
                                {
                                        if (this.selectedSystemBody != null)
                                        {
                                                this.selectedSystemBody.ToggleSelect(false);
                                                this.selectedSystemBody = null;
                                        }
                                }
                        }
                }

                private void OnGUI()
                {
                        if (this.selectedSystemBody != null)
                        {
                                Collider collider = this.selectedSystemBody.GetComponent<Collider>();

                                Vector3 boundPoint1 = collider.bounds.min;
                                Vector3 boundPoint2 = collider.bounds.max;
                                Vector3 boundPoint3 = new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z);
                                Vector3 boundPoint4 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z);
                                Vector3 boundPoint5 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z);
                                Vector3 boundPoint6 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z);
                                Vector3 boundPoint7 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z);
                                Vector3 boundPoint8 = new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z);

                                Vector2[] screenPoints = new Vector2[8];
                                screenPoints[0] = this.sceneCamera.WorldToScreenPoint(boundPoint1);
                                screenPoints[1] = this.sceneCamera.WorldToScreenPoint(boundPoint2);
                                screenPoints[2] = this.sceneCamera.WorldToScreenPoint(boundPoint3);
                                screenPoints[3] = this.sceneCamera.WorldToScreenPoint(boundPoint4);
                                screenPoints[4] = this.sceneCamera.WorldToScreenPoint(boundPoint5);
                                screenPoints[5] = this.sceneCamera.WorldToScreenPoint(boundPoint6);
                                screenPoints[6] = this.sceneCamera.WorldToScreenPoint(boundPoint7);
                                screenPoints[7] = this.sceneCamera.WorldToScreenPoint(boundPoint8);

                                Vector2 topLeftPosition = Vector2.zero;
                                Vector2 topRightPosition = Vector2.zero;
                                Vector2 bottomLeftPosition = Vector2.zero;
                                Vector2 bottomRightPosition = Vector2.zero;

                                for (int a = 0; a < screenPoints.Length; a++)
                                {
                                        //Top Left
                                        if (topLeftPosition.x == 0 || topLeftPosition.x > screenPoints[a].x)
                                        {
                                                topLeftPosition.x = screenPoints[a].x;
                                        }
                                        if (topLeftPosition.y == 0 || topLeftPosition.y > Screen.height - screenPoints[a].y)
                                        {
                                                topLeftPosition.y = Screen.height - screenPoints[a].y;
                                        }
                                        //Top Right
                                        if (topRightPosition.x == 0 || topRightPosition.x < screenPoints[a].x)
                                        {
                                                topRightPosition.x = screenPoints[a].x;
                                        }
                                        if (topRightPosition.y == 0 || topRightPosition.y > Screen.height - screenPoints[a].y)
                                        {
                                                topRightPosition.y = Screen.height - screenPoints[a].y;
                                        }
                                        //Bottom Left
                                        if (bottomLeftPosition.x == 0 || bottomLeftPosition.x > screenPoints[a].x)
                                        {
                                                bottomLeftPosition.x = screenPoints[a].x;
                                        }
                                        if (bottomLeftPosition.y == 0 || bottomLeftPosition.y < Screen.height - screenPoints[a].y)
                                        {
                                                bottomLeftPosition.y = Screen.height - screenPoints[a].y;
                                        }
                                        //Bottom Right
                                        if (bottomRightPosition.x == 0 || bottomRightPosition.x < screenPoints[a].x)
                                        {
                                                bottomRightPosition.x = screenPoints[a].x;
                                        }
                                        if (bottomRightPosition.y == 0 || bottomRightPosition.y < Screen.height - screenPoints[a].y)
                                        {
                                                bottomRightPosition.y = Screen.height - screenPoints[a].y;
                                        }
                                }

                                GUI.DrawTexture(new Rect(topLeftPosition.x - 16, topLeftPosition.y - 16, 16, 16), this.topLeftBorder);
                                GUI.DrawTexture(new Rect(topRightPosition.x, topRightPosition.y - 16, 16, 16), this.topRightBorder);
                                GUI.DrawTexture(new Rect(bottomLeftPosition.x - 16, bottomLeftPosition.y, 16, 16), this.bottomLeftBorder);
                                GUI.DrawTexture(new Rect(bottomRightPosition.x, bottomRightPosition.y, 16, 16), this.bottomRightBorder);
                        }
                }

                private T GetObject<T>(Transform transformHit) where T : class
                {
                        return transformHit.GetComponent<T>() is T obj ? obj : null;
                }
        }
}