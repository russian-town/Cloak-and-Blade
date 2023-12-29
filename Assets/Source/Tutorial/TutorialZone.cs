using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialZone : InteractiveObject
{
    [SerializeField] private List<TutorialText> _referenceTexts;
    [SerializeField] private List<TutorialText> _congratTexts;
    [SerializeField] private DialogueHandler _dialogueHandler;
    [SerializeField] private bool _isInteractedOnStart;
    [SerializeField] private BaseTutorialElement _tutorialElement;
    [SerializeField] private TutorialZone _nextTutorialZone;

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

        return _congratTexts[_currentIndexCongratText];
    }

    public override void Prepare()
    {
        if (!_isExecuted)
            if (CheckInteractionPossibility())
                Interact();
    }

    public void ShowTutorialZoneCells()
    {
        foreach (var cell in CellsInInteractibleRange)
            cell.View.PlayAbilityRangeEffect();
    }

    public void HideTutorialZoneCells()
    {
        foreach (var cell in CellsInInteractibleRange)
            cell.View.StopAbilityRangeEffect();
    }

    protected override void Disable() { }

    private void OnTutorialZoneComplete()
    {
        _tutorialElement.TutorialZoneComplete -= OnTutorialZoneComplete;

        if(_nextTutorialZone != null)
            _nextTutorialZone.ShowTutorialZoneCells();

        _dialogueHandler.WriteCongratDialogue();
    }
}
