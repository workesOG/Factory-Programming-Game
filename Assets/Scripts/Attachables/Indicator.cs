using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    private Image image;

    private static Color minimized = new(0.5f, 0.5f, 0.5f, 1.0f);
    private static Color opened = new(0.4f, 0.55f, 1.0f, 1.0f);
    private State state = State.Unopened;

    public int ID;
    public enum State
    {
        Unopened,
        Minimized,
        Opened
    }

    void Start()
    {
        image = this.GetComponent<Image>();

        SetState(State.Unopened);
    }

    public void OnClick()
    {
        switch (state)
        {
            case State.Unopened:
                SetState(State.Opened);
                break;
            case State.Minimized:
                SetState(State.Opened);
                break;
            case State.Opened:
                SetState(State.Minimized);
                break;
        }
    }

    public void SetState(State state)
    {
        Color withAlpha;
        Color nonAlpha;
        switch (state)
        {
            case State.Unopened:
                state = State.Unopened;
                withAlpha = image.color;
                nonAlpha = withAlpha;
                nonAlpha.a = 0.0f;
                image.color = nonAlpha;
                AppManager.Instance.CloseApp(ID);
                break;
            case State.Minimized:
                state = State.Minimized;
                nonAlpha = image.color;
                withAlpha = nonAlpha;
                withAlpha = minimized;
                withAlpha.a = 1.0f;
                AppManager.Instance.MinimizeApp(ID);
                image.color = withAlpha;
                break;
            case State.Opened:
                state = State.Opened;
                nonAlpha = image.color;
                withAlpha = nonAlpha;
                withAlpha = opened;
                withAlpha.a = 1.0f;
                image.color = withAlpha;
                AppManager.Instance.ShowApp(ID);
                break;
        }
    }
}
