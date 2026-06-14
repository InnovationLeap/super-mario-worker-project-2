using Godot;
using System;

public static class CursorPositionProvider {
    public static Vector2 GetCursorPosition(Node contextNode) {
        var viewport = contextNode.GetViewport();
        var mousePosition = viewport.GetMousePosition();

        // 优先使用 Camera2D（自动处理所有画布变换）
        var camera = viewport.GetCamera2D();
        if (camera != null) return camera.GetGlobalMousePosition();

        // 无相机时，使用 CanvasTransform 逆矩阵手动转换
        return viewport.CanvasTransform.AffineInverse() * mousePosition;
    }
}
