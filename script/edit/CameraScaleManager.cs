using Godot;
using System;

public partial class CameraScaleManager : Node {
    [Export] public Button? CameraScaleUpButton;
    [Export] public Button? CameraScaleDownButton;
    [Export] public Camera2D? Camera;

    private const float ZoomStep = 0.2f;

    public override void _Ready() {
        base._Ready();
        if (CameraScaleUpButton == null) {
            GD.PushError("CameraScaleUpButton is null");
            return;
        }
        if (CameraScaleDownButton == null) {
            GD.PushError("CameraScaleDownButton is null");
            return;
        }
        CameraScaleUpButton.Pressed += OnScaleUpButtonPressed;
        CameraScaleDownButton.Pressed += OnScaleDownButtonPressed;
    }

    public void OnScaleUpButtonPressed() {
        if (Camera == null) {
            GD.PushError("Camera is null");
            return;
        }
        Camera.Zoom += new Vector2(ZoomStep, ZoomStep);
    }

    public void OnScaleDownButtonPressed() {
        if (Camera == null) {
            GD.PushError("Camera is null");
            return;
        }
        Camera.Zoom -= new Vector2(ZoomStep, ZoomStep);
    }
}
