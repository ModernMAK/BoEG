using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class StatWatcher : MonoBehaviour
{
    private Slider mSlider;

    private Statable mStat;

    // Use this for initialization
    private void Start()
    {
        mStat = GetComponentInParent<Statable>();
        mSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (mSlider != null && mStat != null)
            mSlider.value = mStat.ValueNormalized;
    }
}