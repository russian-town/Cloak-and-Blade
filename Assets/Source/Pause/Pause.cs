using System;
using System.Collections.Generic;

public class Pause
{
    private readonly List<IPauseHandler> _handlers = new();

    public Pause(List<IPauseHandler> pauseHandlers)
        => _handlers.AddRange(pauseHandlers);

    public event Action Enabled;

    public event Action Disabled;

    public void AddHandler(IPauseHandler pauseHandler)
        => _handlers.Add(pauseHandler);

    public void RemoveHandler(IPauseHandler pauseHandler)
    {
        if (_handlers.Contains(pauseHandler))
            _handlers.Remove(pauseHandler);
    }

    public void Enable()
    {
        if (_handlers.Count == 0)
            return;

        foreach (IPauseHandler handler in _handlers)
            handler.Pause();

        Enabled?.Invoke();
    }

    public void Disable()
    {
        if (_handlers.Count == 0)
            return;

        foreach (IPauseHandler handler in _handlers)
            handler.Unpause();

        Disabled?.Invoke();
    }
}
