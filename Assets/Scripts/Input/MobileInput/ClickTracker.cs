using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.MobileInput
{
    public class ClickTracker : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private string _buttonName = ""; //This should be an unique name of the button
        [SerializeField] private bool _isJoystick;
        [SerializeField] private float _movementLimit = 1;        //How far the joystick can be moved (n x Joystick Width)
        [SerializeField] private float _movementThreshold = 0.1f; //Minimum distance (n x Joystick Width) that the Joystick need to be moved to trigger inputAxis (Must be less than movementLimit)

        public string ButtonName => _buttonName;

        private RectTransform _rt;
        private Vector3 _startPos;
        private Vector2 _clickPos;

        private Vector2 _inputAxis = Vector2.zero;
        private bool _holding;
        private bool _clicked;

        private void Start()
        {
            //Add this button to the list
            MobileControls.Instance.AddButton(this);

            _rt = GetComponent<RectTransform>();
            _startPos = _rt.anchoredPosition3D;
        }

        //Do this when the mouse is clicked over the selectable object this script is attached to.
        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log(this.gameObject.name + " Was Clicked.");

            _holding = true;

            if (!_isJoystick) {
                _clicked = true;
                StartCoroutine(StopClickEvent());
            } else {
                //Initialize Joystick movement
                _clickPos = eventData.pressPosition;
            }
        }

        private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

        //Wait for next update then release the click event
        private IEnumerator StopClickEvent()
        {
            yield return _waitForEndOfFrame;

            _clicked = false;
        }

        //Joystick movement
        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log(this.gameObject.name + " The element is being dragged");

            if (!_isJoystick) return;

            Vector3 movementVector = Vector3.ClampMagnitude((eventData.position - _clickPos) / MobileControls.Instance._canvas.scaleFactor, (_rt.sizeDelta.x * _movementLimit) + (_rt.sizeDelta.x * _movementThreshold));
            Vector3 movePos = _startPos + movementVector;
            _rt.anchoredPosition = movePos;

            //Update inputAxis
            float inputX = 0;
            float inputY = 0;
            if (Mathf.Abs(movementVector.x) > _rt.sizeDelta.x * _movementThreshold) {
                inputX = (movementVector.x - (_rt.sizeDelta.x * _movementThreshold * (movementVector.x > 0 ? 1 : -1))) / (_rt.sizeDelta.x * _movementLimit);
            }
            if (Mathf.Abs(movementVector.y) > _rt.sizeDelta.x * _movementThreshold) {
                inputY = (movementVector.y - (_rt.sizeDelta.x * _movementThreshold * (movementVector.y > 0 ? 1 : -1))) / (_rt.sizeDelta.x * _movementLimit);
            }
            _inputAxis = new Vector2(inputX, inputY);
        }

        //Do this when the mouse click on this selectable UI object is released.
        public void OnPointerUp(PointerEventData eventData)
        {
            //Debug.Log(this.gameObject.name + " The mouse click was released");

            _holding = false;

            if (_isJoystick) {
                //Reset Joystick position
                _rt.anchoredPosition = _startPos;
                _inputAxis = Vector2.zero;
            }
        }

        public Vector2 GetInputAxis()
        {
            return _inputAxis;
        }

        public bool GetClickedStatus()
        {
            return _clicked;
        }

        public bool GetHoldStatus()
        {
            return _holding;
        }
    }
}