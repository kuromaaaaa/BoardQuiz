using UnityEngine;
using UnityEngine.UI;

public class SliderChangeValue : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Text _text;
    public void ChangeValue()
    {
        _text.text = _slider.value.ToString();
    }
}
