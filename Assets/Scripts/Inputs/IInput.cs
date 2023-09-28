using System;
using Zenject;

public interface IInput : ITickable
{
    event Action<float> OnVerticalInput;
    event Action<float> OnHorizontalInput;
    event Action<string> OnKeyDown;
}
