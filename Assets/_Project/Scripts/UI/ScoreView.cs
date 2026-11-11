using TMPro;
using UnityEngine;

namespace SnakeScroller
{
    // Shows the score on screen. It only displays the score; it never changes it.
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private ScoreService _scoreService;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private string _format = "SCORE:{0}";

        // Listens for score changes so the text updates by itself.
        private void OnEnable()
        {
            if (_scoreService != null) _scoreService.OnScoreChanged += ShowScore;
        }

        // Stops listening when disabled, so no events reach a dead object.
        private void OnDisable()
        {
            if (_scoreService != null) _scoreService.OnScoreChanged -= ShowScore;
        }

        // Shows the current score on the first frame, before any food is eaten.
        private void Start()
        {
            ShowScore(_scoreService.Score);
        }

        // Writes the score into the text using the format, e.g. "SCORE:5".
        private void ShowScore(int score)
        {
            _scoreText.text = string.Format(_format, score);
        }
    }
}
