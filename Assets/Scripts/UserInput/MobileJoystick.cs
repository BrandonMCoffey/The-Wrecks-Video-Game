using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UserInput
{
    public class MobileJoystick : MobileInputBase
    {
        [SerializeField, Tooltip("How far the joystick can be moved (n x Joystick Width)")]
        private float _movementLimit = 1;

        [SerializeField, Tooltip("Minimum distance (n x Joystick Width) that the Joystick need to be moved to trigger inputAxis (Must be less than movementLimit)")]
        private float _movementThreshold = 0.1f;

        public override void OnPointerDown(PointerEventData eventData)
        {
            Holding = true;
            ClickPos = eventData.pressPosition;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            Vector3 movementVector = Vector3.ClampMagnitude((eventData.position - ClickPos) / MobileControls.Instance.Canvas.scaleFactor, (Rt.sizeDelta.x * _movementLimit) + (Rt.sizeDelta.x * _movementThreshold));
            Vector3 movePos = StartPos + movementVector;
            Rt.anchoredPosition = movePos;

            float inputX = 0;
            float inputY = 0;
            if (Mathf.Abs(movementVector.x) > Rt.sizeDelta.x * _movementThreshold) {
                inputX = (movementVector.x - (Rt.sizeDelta.x * _movementThreshold * (movementVector.x > 0 ? 1 : -1))) / (Rt.sizeDelta.x * _movementLimit);
            }
            if (Mathf.Abs(movementVector.y) > Rt.sizeDelta.x * _movementThreshold) {
                inputY = (movementVector.y - (Rt.sizeDelta.x * _movementThreshold * (movementVector.y > 0 ? 1 : -1))) / (Rt.sizeDelta.x * _movementLimit);
            }
            InputAxis = new Vector2(inputX, inputY);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            Holding = false;
            Rt.anchoredPosition = StartPos;
            InputAxis = Vector2.zero;
        }
    }
}