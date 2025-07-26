using System.Collections.Generic;
using GameFramework;

namespace Game
{
    public class UIElementContainer : IReference
    {
        private readonly List<AUIElement> m_UIElement = new List<AUIElement>();
        public AUGuiForm Owner { get; private set; }

        public void Clear()
        {
            Owner = null;
        }

        public static UIElementContainer Create(AUGuiForm owner)
        {
            UIElementContainer r = ReferencePool.Acquire<UIElementContainer>();
            r.Owner = owner;
            return r;
        }


        public T AddUIElement<T>(AUIElement uiElement) where T : AUIElement
        {
            if (uiElement == null)
            {
                throw new GameFrameworkException("Can't add empty!");
            }
            if (m_UIElement.Contains(uiElement))
            {
                throw new GameFrameworkException(Utility.Text.Format("Can't duplicate add UIElement : '{0}'!", uiElement.CachedTransform.name));
            }
            m_UIElement.Add(uiElement);
            uiElement.Add(Owner);
            return (T)uiElement;
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var uiWidget in m_UIElement)
            {
                if (uiWidget.Visible)
                {
                    uiWidget.OnUpdate(elapseSeconds, realElapseSeconds);
                }
            }
        }

        public void RemoveUIElement(AUIElement uiElement)
        {
            m_UIElement.Remove(uiElement);
            uiElement.Remove();
        }

        public void CloseAllUIElement(bool isShutdown)
        {
            if (m_UIElement.Count > 0)
            {
                foreach (var element in m_UIElement)
                {
                    element.Remove();
                }
                m_UIElement.Clear();
            }
        }
    }
}