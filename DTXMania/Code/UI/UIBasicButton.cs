using System;
using System.Drawing;

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
            normalText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Top);
            
            selectedText = AddChild(new UIText(font));
            selectedText.SetText(text);
            selectedText.gradationTopColor = Color.Yellow;
            selectedText.gradationBottomColor = Color.OrangeRed;
            selectedText.drawMode = CPrivateFont.DrawMode.Edge | CPrivateFont.DrawMode.Gradation;
            selectedText.RenderTexture();
            selectedText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Top);
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