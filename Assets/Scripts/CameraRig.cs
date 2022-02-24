using Space4x.Settings;
using UnityEngine;

namespace Space4x
{
        public class CameraRig : MonoBehaviour
        {
                [SerializeField] private Transform swivel;
                [SerializeField] private Transform stick;
                private float rotationAngle;
                private float zoom = 1f;

                private CameraSettings cameraSettings;

                private void Awake()
                {
                        this.swivel = this.transform.GetChild(0);
                        this.stick = this.swivel.GetChild(0);
                }

                public void Initialise(CameraSettings cameraSettings)
                {
                        this.cameraSettings = cameraSettings;
                }

                public void AdjustZoom(float delta)
                {
                        this.zoom = Mathf.Clamp01(this.zoom + delta);

                        float distance = Mathf.Lerp(this.cameraSettings.StickMinZoom, this.cameraSettings.StickMaxZoom, this.zoom);
                        this.stick.localPosition = new Vector3(0f, 0f, distance);

                        float angle = Mathf.Lerp(this.cameraSettings.SwivelMinZoom, this.cameraSettings.SwivelMaxZoom, this.zoom);
                        this.swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
                }

                public void AdjustRotation(float delta)
                {
                        this.rotationAngle += delta * this.cameraSettings.RotationSpeed * Time.deltaTime;
                        if (this.rotationAngle < 0f)
                        {
                                this.rotationAngle += 360f;
                        }
                        else if (this.rotationAngle >= 360f)
                        {
                                this.rotationAngle -= 360f;
                        }

                        this.transform.localRotation = Quaternion.Euler(0f, this.rotationAngle, 0f);
                }

                public void AdjustPosition(float xDelta, float zDelta)
                {
                        Vector3 direction = this.transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
                        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
                        float moveSpeed = Mathf.Lerp(this.cameraSettings.MoveSpeedMinZoom, this.cameraSettings.MoveSpeedMaxZoom, this.zoom);
                        float distance = moveSpeed * damping * Time.deltaTime;

                        Vector3 position = this.transform.localPosition;
                        position += direction * distance;
                        this.transform.localPosition = this.ClampPosition(position);
                }

                private Vector3 ClampPosition(Vector3 position)
                {
                        position.x = Mathf.Clamp(position.x, -this.cameraSettings.Limit, this.cameraSettings.Limit);
                        position.z = Mathf.Clamp(position.z, -this.cameraSettings.Limit, this.cameraSettings.Limit);

                        return position;
                }
        }
}