using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider _slider;

    public int Min
    {
        get => (int)_slider.minValue;
        set => _slider.minValue = (int)value;
    }
    public int Max
    {
        get => (int)_slider.maxValue;
        set => _slider.maxValue = (int)value;
    }

    public int Value
    {
        get => (int)_slider.value;
        set => _slider.value = (int)value;
    }

    // Start is called before the first frame update
    void Start() { _slider = GetComponent<Slider>(); }
}
