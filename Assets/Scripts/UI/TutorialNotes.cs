using TMPro;
using UnityEngine;

namespace UI
{
    public class TutorialNotes : MonoBehaviour
    {
        private TextMeshProUGUI _notes;
        
        public void Show(string message)
        {
            _notes.text = message;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void Start()
        {
            _notes = GetComponent<TextMeshProUGUI>();
            Hide();
        }
    }
}
