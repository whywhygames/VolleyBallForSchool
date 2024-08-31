using UnityEngine;
using UnityEngine.UI;

public abstract class ServeBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillBar;
    [SerializeField] private Gradient _gradient;

    public void Service(float elapsedHold)
    {
        _slider.value = elapsedHold;
        _fillBar.color = _gradient.Evaluate(elapsedHold / 5);
    }
}
