using Unity.Space4x.Assets.Scripts.Managers;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
        private new Light light;
        private Transform swivel;
        private Transform stick;
        private float rotationAngle;
        private float zoom = 1f;

        public SystemManager map;
        public float moveSpeedMinZoom;
        public float moveSpeedMaxZoom;
        public float rotationSpeed;
        public float stickMinZoom;
        public float stickMaxZoom;
        public float swivelMinZoom;
        public float swivelMaxZoom;

        void Awake()
        {
                this.swivel = this.transform.GetChild(0);
                this.stick = this.swivel.GetChild(0);
                this.light = GetComponentInChildren<Light>();
        }

        void Update()
        {
                float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
                if (zoomDelta != 0f)
                {
                        this.AdjustZoom(zoomDelta);
                }

                float rotationDelta = Input.GetAxis("Rotation");
                if (rotationDelta != 0f)
                {
                        this.AdjustRotation(rotationDelta);
                }

                float xDelta = Input.GetAxis("Horizontal");
                float zDelta = Input.GetAxis("Vertical");
                if (xDelta != 0f || zDelta != 0f)
                {
                        this.AdjustPosition(xDelta, zDelta);
                }
        }

        private void AdjustZoom(float delta)
        {
                this.zoom = Mathf.Clamp01(this.zoom + delta);

                float distance = Mathf.Lerp(this.stickMinZoom, this.stickMaxZoom, this.zoom);
                this.stick.localPosition = new Vector3(0f, 0f, distance);

                float angle = Mathf.Lerp(this.swivelMinZoom, this.swivelMaxZoom, this.zoom);
                this.swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }

        private void AdjustRotation(float delta)
        {
                this.rotationAngle += delta * this.rotationSpeed * Time.deltaTime;
                if (this.rotationAngle < 0f)
                {
                        this.rotationAngle += 360f;
                }
                else if (this.rotationAngle >= 360f)
                {
                        this.rotationAngle -= 360f;
                }

                this.transform.localRotation = Quaternion.Euler(0f, this.rotationAngle, 0f);

                this.light.transform.LookAt(Vector3.zero);
        }

        private void AdjustPosition(float xDelta, float zDelta)
        {
                Vector3 direction = this.transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
                float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
                float moveSpeed = Mathf.Lerp(this.moveSpeedMinZoom, this.moveSpeedMaxZoom, this.zoom);
                float distance = moveSpeed * damping * Time.deltaTime;

                Vector3 position = this.transform.localPosition;
                position += direction * distance;
                this.transform.localPosition = this.ClampPosition(position);

                this.light.transform.LookAt(Vector3.zero);
        }

        private Vector3 ClampPosition(Vector3 position)
        {
                float radius = this.map.Radius;
                position.x = Mathf.Clamp(position.x, -radius, radius);
                position.z = Mathf.Clamp(position.z, -radius, radius);

                return position;
        }
}