using UnityEngine;
using UnityEngine.EventSystems;

namespace WorldWeaver.Interaction
{
    /// <summary>
    /// Attach to a UI joystick rect. Feeds ThirdPersonController on mobile builds.
    /// Wire in Inspector: joystickBackground, knob, player.
    /// </summary>
    public class MobileInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public RectTransform background;
        public RectTransform knob;
        public ThirdPersonController player;
        public float maxRadius = 48f;

        Vector2 _pointerOrigin;
        bool _active;

        public void OnPointerDown(PointerEventData eventData)
        {
            _active = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out _pointerOrigin);
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_active || player == null) return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out var local);
            var delta = local - _pointerOrigin;
            if (delta.magnitude > maxRadius) delta = delta.normalized * maxRadius;
            if (knob != null) knob.anchoredPosition = delta;
            player.SetMoveInput(new Vector2(delta.x / maxRadius, -delta.y / maxRadius));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _active = false;
            if (knob != null) knob.anchoredPosition = Vector2.zero;
            player?.SetMoveInput(Vector2.zero);
        }
    }
}
