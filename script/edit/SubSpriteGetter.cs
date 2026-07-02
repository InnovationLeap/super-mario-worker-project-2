using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class SubSpriteGetter : Node {
    [Export] public Sprite2D ParentSprite = null!;
    [Export] public Vector2 BaseOffset = Vector2.Zero;

    public override void _EnterTree() {
        base._EnterTree();
        var tex = BuildCompositeTexture();
        if (tex != null) {
            ParentSprite.Texture = tex;
        }
    }

    private Texture2D BuildCompositeTexture() {
        if (ParentSprite == null || ParentSprite.Texture == null) {
            GD.PushError("Parent sprite not found or missing texture.");
            return null;
        }

        // 收集父节点下的所有子 Sprite2D（道具纹理）
        var subSprites = ParentSprite.GetChildren().OfType<Sprite2D>()
                                     .Where(s => s != ParentSprite).ToList();

        if (subSprites.Count == 0) {
            return ParentSprite.Texture;
        }

        var baseImage = ParentSprite.Texture.GetImage();
        if (baseImage == null) return ParentSprite.Texture;

        int baseW = baseImage.GetWidth();
        int baseH = baseImage.GetHeight();

        var targetImage = Image.Create(baseW, baseH, false, baseImage.GetFormat());
        targetImage.BlitRect(baseImage, new Rect2I(0, 0, baseW, baseH), Vector2I.Zero);

        // 父纹理的中心偏移（如果父节点 Centered = true）
        bool parentCentered = ParentSprite.Centered;
        Vector2 parentCenter = parentCentered ? new Vector2(baseW / 2f, baseH / 2f) : Vector2.Zero;

        foreach (var sub in subSprites) {
            if (sub.Texture == null) continue;
            var subImage = sub.Texture.GetImage();
            if (subImage == null) continue;

            // 子纹理原始尺寸
            float subW = subImage.GetWidth();
            float subH = subImage.GetHeight();

            // 应用子节点缩放
            float finalW = subW * sub.Scale.X;
            float finalH = subH * sub.Scale.Y;

            Image finalSubImage = subImage;
            if (sub.Scale != Vector2.One) {
                finalSubImage = (Image)subImage.Duplicate();
                int newW = Mathf.Max(1, Mathf.RoundToInt(finalW));
                int newH = Mathf.Max(1, Mathf.RoundToInt(finalH));
                finalSubImage.Resize(newW, newH, Image.Interpolation.Lanczos);
                finalW = newW;
                finalH = newH;
            }

            // 子纹理中心偏移（如果子节点 Centered = true）
            bool subCentered = sub.Centered;
            Vector2 subCenter = subCentered ? new Vector2(finalW / 2f, finalH / 2f) : Vector2.Zero;

            // 计算绘制位置（局部坐标 → 父纹理坐标系）
            // 子节点的 Position 是相对于父节点的局部坐标
            // 父纹理原点在左上角，但 Centered 会偏移
            Vector2 drawPos = sub.Position + sub.Offset + parentCenter - subCenter + BaseOffset;

            var pos = new Vector2I(
                Mathf.RoundToInt(drawPos.X),
                Mathf.RoundToInt(drawPos.Y)
            );

            // 裁剪避免超出边界
            int blitW = Mathf.Min(finalSubImage.GetWidth(), targetImage.GetWidth() - pos.X);
            int blitH = Mathf.Min(finalSubImage.GetHeight(), targetImage.GetHeight() - pos.Y);

            if (blitW > 0 && blitH > 0) {
                var blitRect = new Rect2I(0, 0, blitW, blitH);
                targetImage.BlendRect(finalSubImage, blitRect, pos);
            }
        }

        return ImageTexture.CreateFromImage(targetImage);
    }
}