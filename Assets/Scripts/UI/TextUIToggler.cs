using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextUIToggler : BaseUIToggler
    {
        [SerializeField]
        private string activeText = null;
        [SerializeField]
        private Color activeColor = Color.black;
        [SerializeField]
        private Color disableColor = Color.red;
        [SerializeField]
        private string disableText = null;

        private TextMeshProUGUI _message;

        private void Awake()
        {
            _message = GetComponent<TextMeshProUGUI>();
        }

        public override bool Toggle()
        {
            IsActive = !IsActive;
            UpdateAppearance();

            return IsActive;
        }

        public override void SetTo(bool value)
        {
            IsActive = value;
            UpdateAppearance();
        }
        
        private void UpdateAppearance()
        {
            _message.text = IsActive ? activeText : disableText;
            _message.color = IsActive ? activeColor : disableColor;
        }
    }
}