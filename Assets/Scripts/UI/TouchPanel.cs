using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPanel : MonoBehaviour
{
    [SerializeField] private List<Image> _touchLights = new List<Image>();
    [SerializeField] private TouchController _tocuhController;

    public void AddLight()
    {
        if (_tocuhController.BallSide == Team.Right)
        {
            if (_tocuhController.TeamTouchRight - 1 < 0)
                _touchLights[0].color = Color.red;
            else
                _touchLights[_tocuhController.TeamTouchRight - 1].color = Color.red;
        }
        else
        {
            if (_tocuhController.TeamTouchLeft - 1 < 0)
                _touchLights[0].color = Color.red;
            else
                _touchLights[_tocuhController.TeamTouchLeft - 1].color = Color.red;
        }
    }

    public void ResetLights()
    {
        foreach (var light in _touchLights)
            light.color = Color.white;
    }
}
