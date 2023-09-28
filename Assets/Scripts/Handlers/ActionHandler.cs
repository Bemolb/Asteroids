using System;

public class ActionHandler : IDisposable
{
    private IInput _input;
    private IAttackable _attackable;
    public ActionHandler(IInput input, IAttackable attackable)
    {
        _input = input;
        _attackable = attackable;
        _input.OnKeyDown += OnKeyDown;
    }

    private void OnKeyDown(string obj)
    {
        switch(obj)
        {
            case "Fire":
                _attackable.Attack();
                return;
        }
    }

    public void Dispose()
    {
        _input.OnKeyDown -= OnKeyDown;
    }
}
