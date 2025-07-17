using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game
{
    public abstract class AExUGuiForm : AUGuiForm
    {
        private CancellationTokenSource m_CancellationTokenSource;

        private UIWidgetContainer m_UIWidgetContainer;
        private EventContainer m_EventContainer;
        private ResourceContainer m_ResourceContainer;
        private NetContainer m_NetContainer;

        private void ClearUIForm()
        {
            Log.Error("clear ui ");
            if (m_EventContainer != null)
            {
                ReferencePool.Release(m_EventContainer);
                m_EventContainer = null;
            }
            if (m_UIWidgetContainer != null)
            {
                ReferencePool.Release(m_UIWidgetContainer);
                m_UIWidgetContainer = null;
            }
            if (m_ResourceContainer != null)
            {
                ReferencePool.Release(m_ResourceContainer);
                m_ResourceContainer = null;
            }
            if (m_NetContainer != null)
            {
                ReferencePool.Release(m_NetContainer);
                m_NetContainer = null;
            }
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            m_UIWidgetContainer?.OnRecycle();
        }

        private void OnDestroy()
        {
            RemoveAllUIWidget();
            ClearUIForm();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_UIWidgetContainer?.OnClose(isShutdown, userData);
            UnsubscribeAll();
            UnloadAllAssets();
            CloseAllUIWidgets(userData, isShutdown);
            if (isShutdown)
            {
                RemoveAllUIWidget();
                ClearUIForm();
            }
            base.OnClose(isShutdown, userData);
        }

        protected override void OnPause()
        {
            base.OnPause();
            m_UIWidgetContainer?.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            m_UIWidgetContainer?.OnResume();
        }

        protected override void OnCover()
        {
            base.OnCover();
            m_UIWidgetContainer?.OnCover();
        }

        protected override void OnReveal()
        {
            base.OnReveal();
            m_UIWidgetContainer?.OnReveal();
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
            m_UIWidgetContainer?.OnRefocus(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_UIWidgetContainer?.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            m_UIWidgetContainer?.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }

        public void AddUIWidget(AUIWidget auiWidget, object userData = null)
        {
            if (m_UIWidgetContainer == null)
            {
                m_UIWidgetContainer = UIWidgetContainer.Create(this);
            }
            m_UIWidgetContainer.AddUIWidget(auiWidget, userData);
        }

        public void RemoveUIWidget(AUIWidget auiWidget)
        {
            if (m_UIWidgetContainer == null)
            {
                throw new GameFrameworkException("Container is empty!");
            }
            m_UIWidgetContainer.RemoveUIWidget(auiWidget);
        }

        public void RemoveAllUIWidget()
        {
            if (m_UIWidgetContainer == null)
                return;
            m_UIWidgetContainer.RemoveAllUIWidget();
        }

        /// <summary>
        /// 打开UIWidget，不刷新Depth，一般在UIForm的OnOpen中调用
        /// </summary>
        /// <param name="auiWidget"></param>
        /// <param name="userData"></param>
        /// <exception cref="GameFrameworkException"></exception>
        public void OpenUIWidget(AUIWidget auiWidget, object userData = null)
        {
            if (m_UIWidgetContainer == null)
            {
                throw new GameFrameworkException("Container is empty!");
            }
            m_UIWidgetContainer.OpenUIWidget(auiWidget, userData);
        }

        /// <summary>
        /// 动态打开UIWidget，刷新Depth
        /// </summary>
        /// <param name="auiWidget"></param>
        /// <param name="userData"></param>
        /// <exception cref="GameFrameworkException"></exception>
        public void DynamicOpenUIWidget(AUIWidget auiWidget, object userData = null)
        {
            if (m_UIWidgetContainer == null)
            {
                throw new GameFrameworkException("Container is empty!");
            }
            m_UIWidgetContainer.DynamicOpenUIWidget(auiWidget, userData);
        }

        public void CloseUIWidget(AUIWidget uiWidget, object userData = null, bool isShutdown = false)
        {
            if (m_UIWidgetContainer == null)
            {
                throw new GameFrameworkException("Container is empty!");
            }
            m_UIWidgetContainer.CloseUIWidget(uiWidget, userData, isShutdown);
        }

        public void CloseAllUIWidgets(object userData = null, bool isShutdown = false)
        {
            if (m_UIWidgetContainer == null)
                return;
            m_UIWidgetContainer.CloseAllUIWidgets(userData, isShutdown);
        }

        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (m_EventContainer == null)
            {
                m_EventContainer = EventContainer.Create(this);
            }
            m_EventContainer.Subscribe(id, handler);
        }

        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (m_EventContainer == null)
                return;
            m_EventContainer.Unsubscribe(id, handler);
        }

        public void UnsubscribeAll()
        {
            if (m_EventContainer == null)
                return;
            m_EventContainer.UnsubscribeAll();
        }

        public void LoadAsset<T>(string assetName, Action<T> onLoadSuccess, Action onLoadFailure = null, int priority = 0,
            Action<float> updateEvent = null, Action<string> dependencyAssetEvent = null) where T : UnityEngine.Object
        {
            if (m_ResourceContainer == null)
            {
                m_ResourceContainer = ResourceContainer.Create(m_CancellationTokenSource.Token);
            }
            m_ResourceContainer.LoadAsset(assetName, onLoadSuccess, onLoadFailure, priority, updateEvent, dependencyAssetEvent);
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetName, int priority = 0,
            Action<float> updateEvent = null, Action<string> dependencyAssetEvent = null) where T : UnityEngine.Object
        {
            if (m_ResourceContainer == null)
            {
                m_ResourceContainer = ResourceContainer.Create(m_CancellationTokenSource.Token);
            }
            return await m_ResourceContainer.LoadAssetAsync<T>(assetName, priority, updateEvent, dependencyAssetEvent);
        }

        public void UnloadAsset(UnityEngine.Object asset)
        {
            if (m_ResourceContainer == null)
                return;
            m_ResourceContainer.UnloadAsset(asset);
        }

        public void UnloadAllAssets()
        {
            if (m_ResourceContainer == null)
                return;
            m_ResourceContainer.UnloadAllAssets();
        }

        protected async UniTask<T> SendPacketAsync<T>(Packet packet, string channel = "TcpChannel") where T : Packet
        {
            m_NetContainer ??= NetContainer.Create(channel);
            var p = await m_NetContainer.SendPacketAsync(packet);
            return p as T;
        }
    }
}