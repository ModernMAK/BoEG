using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterDuration : MonoBehaviour
{
    [SerializeField] private float _duration;
    private float _startTime;
    private bool _started;
    public float ElapsedTime => _started ? Time.time - _startTime : 0f;

    public float SetDuration(float duration) => _duration = duration;
    
    public void StartTimer()
    {
        _startTime = Time.time;
        _started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_started)
            if (ElapsedTime >= _duration)
            {
                var go = gameObject;
                go.SetActive(false);
                Destroy(go);
            }
    }
}
