using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialZone : InteractiveObject
{
    [SerializeField] private List<TMPro.TMP_Text> _referenceTexts;
    [SerializeField] private DialogueHandler _dialogueHandler;
    [SerializeField] private bool _isInteractedOnStart;

    private bool _isExecuted;
    
    private void Start()
    {
        if (_isInteractedOnStart)
            Interact();
    }

    public override void Interact()
    {
        _dialogueHandler.WriteDialogue(_referenceTexts);
        _isExecuted = true;
    }

    public override void Prepare()
    {
        if (!_isExecuted)
            if (CheckInteractionPossibility())
                Interact();
    }

    protected override void Disable()
    {
        
    }
}
