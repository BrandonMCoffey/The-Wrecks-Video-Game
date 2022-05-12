using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UserInput.MobileInput
{
    public abstract class MobileInputBase : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField, Tooltip("The unique name of the Button in UI")]
        private string _buttonName = "";
        public string ButtonName => _buttonName;

        protected RectTransform Rt;
        protected Vector3 StartPos;
        protected Vector2 ClickPos;

        protected Vector2 InputAxis = Vector2.zero;
        protected bool Holding;
        protected bool Clicked;

        private void Start()
        {
            MobileControls.Instance.AddButton(this);

            Rt = GetComponent<RectTransform>();
            StartPos = Rt.anchoredPosition3D;
        }

        public abstract void OnPointerDown(PointerEventData eventData);

        public abstract void OnDrag(PointerEventData eventData);

        public abstract void OnPointerUp(PointerEventData eventData);

        public Vector2 GetInputAxis()
        {
            return InputAxis;
        }

        public bool GetClickedStatus()
        {
            return Clicked;
        }

        public bool GetHoldStatus()
        {
            return Holding;
        }
    }
}