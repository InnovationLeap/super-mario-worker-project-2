using Godot;
using System;

[Tool]
public partial class PaintObjectButtonSet : Node {

    [Export] public EditManager.EditModeType PaintMode = EditManager.EditModeType.None;
    [Export] public PackedScene PaintObjectScene { get; set; } = null!;

    [Export] public bool CanBeLighted {
        get => _canBeLighted;
        set {
            _canBeLighted = value;
            LightNode.Visible = value;
        }
    }
    private bool _canBeLighted = false;
    
    [Export] public Control LightNode = null!;

    public EditManager? EditNode;
    private Button? _paintObjectButton;

    public override void _Ready() {
        if (Engine.IsEditorHint()) {
            LightNode.Visible = _canBeLighted;
            return;
        }
        _paintObjectButton = GetNode<Button>("..");
        _paintObjectButton.Pressed += OnPaintObjectButtonPressed;
        EditNode = GetTree().GetFirstNodeInGroup("edit_node") as EditManager;
        
        LightNode.Visible = _canBeLighted;
    }

    public void OnPaintObjectButtonPressed() {
        if (EditNode == null) {
            GD.PushError("Edit Node not set!");
            return;
        }
        EditNode.CurrentSpawnerObjectScene = PaintObjectScene;
    }
}
