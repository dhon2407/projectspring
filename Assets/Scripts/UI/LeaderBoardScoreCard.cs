using System;
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

        public void SetValue(string pName, int pScore)
        {
            playerName.text = pName;
            score.text = pScore.ToString("0");
        }

        private void Awake()
        {
            playerName.maxVisibleCharacters = 5;
        }
    }
}