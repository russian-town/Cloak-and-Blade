using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialZone : InteractiveObject
{
    [SerializeField] private List<TutorialText> _referenceTexts;
    [SerializeField] private DialogueHandler _dialogueHandler;
    [SerializeField] private bool _isInteractedOnStart;
    [SerializeField] private BaseTutorialElement _tutorialElement;

    private bool _isExecuted;

    public IReadOnlyList<TutorialText> ReferenceTexts => _referenceTexts;
    public BaseTutorialElement Element => _tutorialElement;
    
    private void Start()
    {
        if (_isInteractedOnStart)
            Interact();
    }

    public override void Interact()
    {
        _dialogueHandler.WriteDialogue(this);
        _isExecuted = true;
    }

    public override void Prepare()
    {
        if (!_isExecuted)
            if (CheckInteractionPossibility())
                Interact();
    }

    protected override void Disable() { }
}
