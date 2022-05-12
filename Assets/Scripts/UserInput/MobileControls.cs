using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.UserInput
{
    public class MobileControls : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        private List<MobileInputBase> _buttons = new List<MobileInputBase>();

        public static MobileControls Instance;

        public Canvas Canvas => _canvas;

        private void Awake()
        {
            Instance = this;

            if (_canvas == null) _canvas = GetComponent<Canvas>();
        }

        public int AddButton(MobileInputBase button)
        {
            _buttons.Add(button);

            return _buttons.Count - 1;
        }

        public Vector2 GetJoystick(string joystickName)
        {
            foreach (var tracker in _buttons.Where(tracker => tracker.ButtonName == joystickName)) {
                return tracker.GetInputAxis();
            }

            Debug.LogError("Joystick with a name '" + joystickName + "' not found. Make sure SC_ClickTracker is assigned to the button and the name is matching.");

            return Vector2.zero;
        }

        public bool GetMobileButton(string buttonName)
        {
            foreach (var tracker in _buttons.Where(tracker => tracker.ButtonName == buttonName)) {
                return tracker.GetHoldStatus();
            }

            Debug.LogError("Button with a name '" + buttonName + "' not found. Make sure SC_ClickTracker is assigned to the button and the name is matching.");

            return false;
        }

        public bool GetMobileButtonDown(string buttonName)
        {
            foreach (var tracker in _buttons.Where(tracker => tracker.ButtonName == buttonName)) {
                return tracker.GetClickedStatus();
            }

            Debug.LogError("Button with a name '" + buttonName + "' not found. Make sure SC_ClickTracker is assigned to the button and the name is matching.");

            return false;
        }
    }
}