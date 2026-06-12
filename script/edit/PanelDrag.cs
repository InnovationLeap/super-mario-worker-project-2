using Godot;
using System;

[GlobalClass]
public partial class PanelDrag : Panel {
    private bool _dragging = false;
    private Vector2 _dragOffset = Vector2.Zero;

    public override void _GuiInput(InputEvent @event) {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left) {
            if (mouseEvent.Pressed) {
                _dragging = true;
                _dragOffset = GetGlobalMousePosition() - Position;
                AcceptEvent(); // 阻止事件继续传递
            }
            else {
                _dragging = false;
                AcceptEvent();
            }
        }
        else if (@event is InputEventMouseMotion motionEvent && _dragging) {
            Position = GetGlobalMousePosition() - _dragOffset;
            AcceptEvent();
        }
    }
}