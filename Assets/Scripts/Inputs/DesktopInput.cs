using System;
using UnityEngine;
using Zenject;

public class DesktopInput : IInput, ITickable
{
    public event Action<float> OnVerticalInput;
    public event Action<float> OnHorizontalInput;
    public event Action<string> OnKeyDown;
    public void Tick()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            OnVerticalInput?.Invoke(1);
        else
            OnVerticalInput?.Invoke(0);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            OnHorizontalInput?.Invoke(1);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            OnHorizontalInput?.Invoke(-1);
        else
            OnHorizontalInput?.Invoke(0);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            OnKeyDown?.Invoke("Fire");
    }
}
