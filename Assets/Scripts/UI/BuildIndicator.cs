using TMPro;
using UnityEngine;

namespace UI
{
    public class BuildIndicator : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI message;

        private void Awake()
        {
            message.text = $"V{Application.version}";
            Destroy(this);
        }
    }
}
