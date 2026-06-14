using Godot;
using System;

public partial class UiScaleManager : Node {
    [Export] public Button? UiScaleUpButton;
    [Export] public Button? UiScaleDownButton;
    [Export] public CanvasLayer? UiCanvas;

    private const float ZoomStep = 0.2f;

    public override void _Ready() {
        base._Ready();
        if (UiScaleUpButton == null) {
            GD.PushError("CameraScaleUpButton is null");
            return;
        }
        if (UiScaleDownButton == null) {
            GD.PushError("CameraScaleDownButton is null");
            return;
        }
        UiScaleUpButton.Pressed += OnScaleUpButtonPressed;
        UiScaleDownButton.Pressed += OnScaleDownButtonPressed;
    }

    public void OnScaleUpButtonPressed() {
        if (UiCanvas == null) {
            GD.PushError("Camera is null");
            return;
        }
        UiCanvas.Scale += new Vector2(ZoomStep, ZoomStep);
    }

    public void OnScaleDownButtonPressed() {
        if (UiCanvas == null) {
            GD.PushError("Camera is null");
            return;
        }
        UiCanvas.Scale -= new Vector2(ZoomStep, ZoomStep);
    }
}
