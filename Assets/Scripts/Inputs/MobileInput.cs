using System;
using UnityEngine;

public class MobileInput : IInput
{
    public event Action<float> OnVerticalInput;
    public event Action<float> OnHorizontalInput;
    public event Action<string> OnKeyDown;

    public void Tick()
    {
        throw new NotImplementedException();
    }
}
