using System.Collections.Generic;
using Source.InteractiveObjects;
using Source.Tutorial.DialogueLine;
using Source.Tutorial.TutorialElements;
using UnityEngine;

namespace Source.Tutorial
{
    public class TutorialZone : InteractiveObject
    {
        [SerializeField] private List<TutorialText> _referenceTexts;
        [SerializeField] private List<TutorialText> _congratTexts;
        [SerializeField] private DialogueHandler _dialogueHandler;
        [SerializeField] private bool _isInteractedOnStart;
        [SerializeField] private BaseTutorialElement _tutorialElement;
        [SerializeField] private TutorialZone _nextTutorialZone;
        [SerializeField] private ParticleSystem _tutorialEffectTemplate;

        private List<ParticleSystem> _effects = new List<ParticleSystem>();
        private bool _isExecuted;
        private int _currentIndexText = 0;
        private int _currentIndexCongratText = -1;

        public BaseTutorialElement Element => _tutorialElement;

        private void OnEnable()
        {
            _tutorialElement.TutorialZoneComplete += OnTutorialZoneComplete;
        }

        private void Start()
        {
            if (_isInteractedOnStart)
                Interact();
        }

        public override void Interact()
        {
            _dialogueHandler.WriteDialogue(this);
            HideTutorialZoneCells();
            _isExecuted = true;
        }

        public TutorialText GetText()
        {
            return _referenceTexts[_currentIndexText];
        }

        public TutorialText GetNextText()
        {
            _currentIndexText++;

            if (_currentIndexText > _referenceTexts.Count - 1)
                return null;

            return _referenceTexts[_currentIndexText];
        }

        public TutorialText GetNextCongratText()
        {
            _currentIndexCongratText++;

            if (_currentIndexCongratText > _congratTexts.Count - 1)
                return null;

            return _congratTexts[_currentIndexCongratText];
        }

        public override void Prepare()
        {
            if (!_isExecuted)
            {
                if (CheckInteractionPossibility())
                {
                    Interact();
                }
            }
        }

        public void ShowTutorialZoneCells()
        {
            foreach (var cell in CellsInInteractibleRange)
            {
                ParticleSystem effect = Instantiate(_tutorialEffectTemplate, cell.transform.position, cell.transform.rotation);
                effect.Play();
                _effects.Add(effect);
            }
        }

        public void HideTutorialZoneCells()
        {
            if (_effects.Count == 0)
                return;

            foreach (var effect in _effects)
                effect.Stop();
        }

        protected override void Disable()
        {
        }

        private void OnTutorialZoneComplete()
        {
            _tutorialElement.TutorialZoneComplete -= OnTutorialZoneComplete;

            if (_nextTutorialZone != null)
                _nextTutorialZone.ShowTutorialZoneCells();

            _dialogueHandler.WriteCongratDialogue();
        }
    }
}
