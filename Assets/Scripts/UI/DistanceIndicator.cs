using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DistanceIndicator : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI main = null;
        [SerializeField]
        private TextMeshProUGUI shadow = null;

        private void Awake()
        {
            UpdateValue(0);

            GameManager.OnDistanceUpdate += UpdateValue;
        }

        private void UpdateValue(int value)
        {
            if (main == null || shadow == null)
                return;

            var strValue = value.ToString("0");
            main.text = strValue;
            shadow.text = strValue;
        }
    }
}