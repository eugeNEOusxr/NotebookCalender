using UnityEngine;

namespace WorldWeaver.Interaction
{
    /// <summary>
    /// Third-person movement with camera follow (mobile + editor WASD).
    /// </summary>
    public class ThirdPersonController : MonoBehaviour
    {
        public Transform cameraTransform;
        public float moveSpeed = 4.2f;
        public Vector3 cameraOffset = new Vector3(0f, 3.2f, 5.5f);
        public Vector3 lookOffset = new Vector3(0f, 1.2f, 0f);

        Vector2 _moveInput;

        public void SetMoveInput(Vector2 input) => _moveInput = input;

        void Update()
        {
            var dt = Time.deltaTime;
            if (_moveInput.sqrMagnitude > 0.01f)
            {
                var dir = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
                transform.rotation = Quaternion.LookRotation(dir);
                transform.position += dir * (moveSpeed * dt);
            }

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -18f, 18f),
                transform.position.y,
                Mathf.Clamp(transform.position.z, -6f, 32f));

            if (cameraTransform == null) return;
            var target = transform.position + cameraOffset;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, target, 1f - Mathf.Pow(0.001f, dt));
            cameraTransform.LookAt(transform.position + lookOffset);
        }

        void OnEnable()
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }
    }
}
