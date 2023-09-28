using System;

public class MovementHandler : IDisposable
{
    private IInput _input;
    private IMovable _movable;
    public MovementHandler(IInput input, IMovable movable)
    {
        _input = input;
        _movable = movable;
        _input.OnVerticalInput += OnVerticalInput;
        _input.OnHorizontalInput += OnHorizontalInput;
    }

    private void OnHorizontalInput(float obj)
    {
        _movable.TurnDirection = obj;
    }

    private void OnVerticalInput(float obj)
    {
        if(obj > 0)
            _movable.Thrusting = true;
        else
            _movable.Thrusting = false;
    }

    public void Dispose()
    {
        _input.OnVerticalInput -= OnVerticalInput;
        _input.OnHorizontalInput -= OnHorizontalInput;
    }
}
