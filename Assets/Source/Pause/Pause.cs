using System.Collections.Generic;

public class Pause
{
    private List<IPauseHandler> _handlers = new();

    public Pause(List<IPauseHandler> pauseHandlers)
        => _handlers.AddRange(pauseHandlers);

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
    }

    public void Disable()
    {
        if (_handlers.Count == 0)
            return;

        foreach (IPauseHandler handler in _handlers)
            handler.Unpause();
    }
}
