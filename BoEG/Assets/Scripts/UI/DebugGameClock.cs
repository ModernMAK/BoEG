using System;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class DebugGameClock : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private void Awake()
        {
            if (_text == null)
                _text = GetComponent<Text>();
            if (_text == null)
                _text = GetComponentInChildren<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            //know that this will break when pause is implimented (as pausing doesn't stop unity time)
            var time = Time.timeSinceLevelLoad;
            var timeSpan = TimeSpan.FromSeconds(time);

            _text.text = $"{timeSpan.Minutes:D}:{timeSpan.Seconds:D2}"; //timeSpan.ToString(@"mm\:ss");
        }
    }
}