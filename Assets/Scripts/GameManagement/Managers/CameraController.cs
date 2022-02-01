using Space4x.Models.Factories;
using Space4x.Settings;
using UnityEngine;

namespace Space4x.GameManagement.Managers
{
        public class CameraController : MonoBehaviour
        {
                [SerializeField] private GameObject cameraRigPrefab;
                private IFactory<CameraRig> cameraRigFactory;
                private CameraRig cameraRig;

                private bool isCameraRigInitialised;

                private void Awake()
                {
                        this.cameraRigFactory = new PrefabFactory<CameraRig>(this.cameraRigPrefab);
                }

                private void Start()
                {
                        this.cameraRig = this.cameraRigFactory.Create();
                }

                private void Update()
                {
                        if (this.isCameraRigInitialised)
                        {
                                this.MoveCamera();
                        }
                }

                public void Initialise(CameraSettings cameraSettings)
                {
                        this.cameraRig.Initialise(cameraSettings);
                        this.isCameraRigInitialised = true;
                }

                private void MoveCamera()
                {
                        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
                        if (zoomDelta != 0f)
                        {
                                this.cameraRig.AdjustZoom(zoomDelta);
                        }

                        float rotationDelta = Input.GetAxis("Rotation");
                        if (rotationDelta != 0f)
                        {
                                this.cameraRig.AdjustRotation(rotationDelta);
                        }

                        float xDelta = Input.GetAxis("Horizontal");
                        float zDelta = Input.GetAxis("Vertical");
                        if (xDelta != 0f || zDelta != 0f)
                        {
                                this.cameraRig.AdjustPosition(xDelta, zDelta);
                        }
                }
        }
}