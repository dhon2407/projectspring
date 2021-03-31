using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LeaderBoardScoreCard : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI playerName = null;
        [SerializeField]
        private TextMeshProUGUI score = null;
        [SerializeField]
        private DOTweenAnimation dotAnimator;

        public void SetValue(string pName, int pScore)
        {
            playerName.text = pName;
            score.text = pScore.ToString("0");
        }

        public void HighLight(bool active)
        {
            StartCoroutine(DelayHighLightAction(active));
        }

        private IEnumerator DelayHighLightAction(bool active)
        {
            if (!dotAnimator)
                yield break;

            yield return null;

            if (active)
                dotAnimator.DOPlay();
            else
                dotAnimator.DORewind();
        }

        private void Awake()
        {
            playerName.maxVisibleCharacters = 6;
            dotAnimator.DORewind();
        }
    }
}