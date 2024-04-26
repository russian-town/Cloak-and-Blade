using System;
using UnityEngine;

namespace Source.CutScene
{
    [Serializable]
    public class PhraseWithPause : MonoBehaviour
    {
        [SerializeField] private string _phrase;
        [SerializeField] private float _pause;

        public string Phrase => _phrase;

        public float Pause => _pause;
    }
}
