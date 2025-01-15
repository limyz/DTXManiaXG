using System;
using SharpDX;
using Color = System.Drawing.Color;

namespace DTXMania.Code.UI
{
    public class UIBasicButton : UIGroup, IUISelectable
    {
        private UIText normalText;
        private UIText selectedText;
        private Action action;
        
        public UIBasicButton(CPrivateFont font, string text, Action action)
        {
            this.action = action;
            
            normalText = AddChild(new UIText(font));
            normalText.SetText(text);
            normalText.RenderTexture();
            normalText.anchor = new Vector2(0.5f, 0f);
            
            selectedText = AddChild(new UIText(font));
            selectedText.SetText(text);
            selectedText.gradationTopColor = Color.Yellow;
            selectedText.gradationBottomColor = Color.OrangeRed;
            selectedText.drawMode = CPrivateFont.DrawMode.Edge | CPrivateFont.DrawMode.Gradation;
            selectedText.RenderTexture();
            selectedText.anchor = new Vector2(0.5f, 0f);
            selectedText.isVisible = false;
        }
        
        public void SetSelected(bool selected)
        {
            normalText.isVisible = !selected;
            selectedText.isVisible = selected;
        }

        public void RunAction()
        {
            action.Invoke();
        }
    }
}