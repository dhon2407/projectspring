using TMPro;
using UnityEngine;

namespace UI
{
    public class BuildIndicator : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI message;
        [SerializeField]
        private bool inEditorOnly;

        private void Awake()
        {
            message.text = $"V{Application.version}";
#if !UNITY_EDITOR
            if (inEditorOnly)
                Destroy(gameObject);
            else
                Destroy(this);
#else
            Destroy(this);
#endif
        }

        private void OnValidate()
        {
            if (message)
                message.text = $"V{Application.version}";
        }
    }
}
