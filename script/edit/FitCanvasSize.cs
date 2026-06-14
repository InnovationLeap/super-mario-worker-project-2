using Godot;

public partial class FitCanvasSize : Control {
    private CanvasLayer? _targetLayer;
    private Vector2 _originalSize;

    public override void _Ready() {
        _targetLayer = GetParentCanvasLayer();
        if (_targetLayer != null) {
            _originalSize = Size;
        }
    }

    public override void _Process(double delta) {
        if (_targetLayer == null) return;
        Vector2 scale = _targetLayer.Scale;
        Vector2 newSize = new Vector2(_originalSize.X / scale.X, _originalSize.Y / scale.Y);
        CustomMinimumSize = newSize;
        Size = newSize;
    }

    private CanvasLayer GetParentCanvasLayer() {
        Node parent = GetParent();
        while (parent != null) {
            if (parent is CanvasLayer layer) return layer;
            parent = parent.GetParent();
        }
        return null;
    }
}