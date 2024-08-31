using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Points:")]
    [SerializeField] private TMP_Text _leftPointsText;
    [SerializeField] private TMP_Text _rightPointsText;

    [Header("UI:")]
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TMP_Text _winText;

    private int _rightPoints;
    private int _leftPoints;
    public void AddPoints(Team team)
    {
        if (team == Team.Right)
        {
            _rightPoints++;
            _rightPointsText.text = $"{_rightPoints}";
        }
        else
        {
            _leftPoints++;
            _leftPointsText.text = $"{_leftPoints}";
        }
    }
    public void Win()
    {
        if (_rightPoints >= 5)
        {
            _winPanel.SetActive(true);
            _winText.text = "Right side wins!";
        }
        if (_leftPoints >= 5)
        {
            _winPanel.SetActive(true);
            _winText.text = "Left side wins!";
        }
    }
}
