using CodeBind;
using UnityEngine;

namespace Game
{
    [CodeBind]
    [DisallowMultipleComponent]
    public abstract class AUIElement: MonoBehaviour
    {
        private Transform m_CachedTransform = null;

        public Transform CachedTransform
        {
            get
            {
                if (m_CachedTransform == null)
                    m_CachedTransform = transform;
                return m_CachedTransform;
            }
        }
        
        private GameObject m_CachedGameObject = null;

        public GameObject CachedGameObject
        {
            get
            {
                if (m_CachedGameObject == null)
                    m_CachedGameObject = gameObject;
                return m_CachedGameObject;
            }
        }

        private AUGuiForm m_OwnerForm;

        public AUGuiForm OwnerForm => m_OwnerForm;

        private bool m_Visible;

        public bool Visible
        {
            get => m_Visible;
            set
            {
                if (m_Visible == value)
                {
                    return;
                }
                m_Visible = value;
                CachedGameObject.SetActive(m_Visible);
            }
        }

        public void Add(AUGuiForm owner)
        {
            m_OwnerForm = owner;
            Visible = true;
            OnAdd();
        }

        public void Remove()
        {
            OnRemove();
            m_OwnerForm = null;
        }
        protected abstract void OnAdd();
        protected abstract void OnRemove();
        
        protected internal virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }
    }
}