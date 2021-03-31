using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField]
        private LeaderBoardScoreCard scoreCardPrefab = null;

        private CanvasGroup _canvasGroup;

        public void Show(List<(string playerName, int score)> scores)
        {
            ClearItems();
            foreach (var scoreCard in scores)
            {
                var card = Instantiate(scoreCardPrefab, transform);
                card.SetValue(scoreCard.playerName, scoreCard.score);
            }

            _canvasGroup.DOFade(1, 0.5f);
        }

        public void Hide()
        {
            _canvasGroup.DOFade(0, 0.2f);
        }
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Hide();
            ClearItems();
        }

        private void ClearItems()
        {
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var scoreCard = transform.GetChild(i).GetComponent<LeaderBoardScoreCard>();
                if (scoreCard != null)
                    Destroy(scoreCard.gameObject);
            }
        }
    }
}