using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    public void Pauser()
    {
        if(Time.timeScale > 0)
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);
        }
        else if(Time.timeScale < 1)
        {
            Time.timeScale = 1;
            _pausePanel.SetActive(false);
        }
    }
}
