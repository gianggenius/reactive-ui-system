using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace UISystem
{
    /// <summary>
    /// The base class for all UI layers.
    /// </summary>
    /// <typeparam name="T">The type of the derived layer.</typeparam>
    public abstract class UIBaseLayer<T> : UILayer<T> where T : UIBaseLayer<T>
    {
        [SerializeField] protected UIContext[] _contexts;

        protected readonly Dictionary<Type, UIContext> _contextRuntimeMap = new();
        protected readonly Dictionary<Type, UIContext> _contextPrefabMap  = new();

        public UIContext CurrentView { get; protected set; }

        protected virtual void Awake()
        {
            InitializeContexts();
        }

        protected void InitializeContexts()
        {
            foreach (var context in _contexts)
            {
                _contextPrefabMap.Add(context.GetType(), context);
                if (context.SpawnOnAwake)
                {
                    GetOrCreateContextInstance(context);
                }
            }
        }

        /// <summary>
        /// Tries to get an instance of the specified UI context type.
        /// </summary>
        /// <typeparam name="TContext">The type of the UI context.</typeparam>
        /// <param name="contextInstance">The instance of the UI context.</param>
        /// <returns><c>true</c> if the instance is found or created successfully; otherwise, <c>false</c>.</returns>
        protected bool TryGetContextInstance<TContext>(out TContext contextInstance) where TContext : UIContext
        {
            contextInstance = null;
            if (!_contextPrefabMap.TryGetValue(typeof(TContext), out var context))
            {
                return false;
            }

            if (!_contextRuntimeMap.TryGetValue(typeof(TContext), out var instance))
            {
                instance = GetOrCreateContextInstance(context);
            }

            contextInstance = instance as TContext;
            return true;
        }

        /// <summary>
        /// Shows the specified UI context asynchronously.
        /// </summary>
        /// <typeparam name="TContext">The type of the UI context.</typeparam>
        /// <typeparam name="TViewModel">The type of the UI view model.</typeparam>
        /// <param name="contextInstance">The instance of the UI context.</param>
        /// <param name="viewModel">The view model to initialize the UI context.</param>
        /// <param name="onPreInitialize">The action to perform before initializing the UI context.</param>
        /// <param name="onPostInitialize">The action to perform after initializing the UI context.</param>
        /// <returns>An instance of TContext if the instance is found or created successfully; otherwise, null.</returns>
        protected async UniTask<TContext> ShowAsyncInternal<TContext, TViewModel>(
            TContext         contextInstance,
            TViewModel       viewModel,
            Action<TContext> onPreInitialize,
            Action<TContext> onPostInitialize)
            where TContext : BaseUIContext<TViewModel>
            where TViewModel : UIModel
        {
            if (IsCurrentViewBusy())
            {
                return null;
            }

            SetupContextInstance(contextInstance, viewModel, onPreInitialize, onPostInitialize);

            await CurrentView.ShowAsync();

            return CurrentView as TContext;
        }

        protected bool IsCurrentViewBusy()
        {
            return CurrentView != null &&
                   (CurrentView.EVisibleState == EVisibleState.Appearing ||
                    CurrentView.EVisibleState == EVisibleState.Disappearing);
        }

        /// <summary>
        /// Sets up an instance of a UI context with the provided parameters.
        /// </summary>
        /// <typeparam name="TContext">The type of the UI context.</typeparam>
        /// <typeparam name="TViewModel">The type of the UI view model.</typeparam>
        /// <param name="contextInstance">The instance of the UI context to be setup.</param>
        /// <param name="viewModel">The view model to be assigned to the UI context (can be null).</param>
        /// <param name="onPreInitialize">An optional callback to be invoked before the UI context appears.</param>
        /// <param name="onPostInitialize">An optional callback to be invoked after the UI context appears.</param>
        protected void SetupContextInstance<TContext, TViewModel>(
            TContext contextInstance,
            TViewModel viewModel,
            Action<TContext> onPreInitialize,
            Action<TContext> onPostInitialize)
            where TContext : BaseUIContext<TViewModel>
            where TViewModel : UIModel
        {
            contextInstance.OnPreAppear.Subscribe(_ => onPreInitialize?.Invoke(contextInstance)).AddTo(contextInstance);

            if (viewModel != null)
            {
                contextInstance.Initialize(viewModel);
            }

            contextInstance.OnPostAppear.Subscribe(_ => onPostInitialize?.Invoke(contextInstance)).AddTo(contextInstance);

            contextInstance.UILayer = this;
            CurrentView = contextInstance;
        }

        /// <summary>
        /// Gets or creates an instance of the specified UI context type.
        /// </summary>
        /// <typeparam name="TContext">The type of the UI context.</typeparam>
        /// <param name="context">The UI context to get or create an instance of.</param>
        /// <returns>The instance of the UI context.</returns>
        protected TContext GetOrCreateContextInstance<TContext>(TContext context) where TContext : UIContext
        {
            if (!_contextRuntimeMap.TryGetValue(typeof(TContext), out var contextInstance))
            {
                context.gameObject.SetActive(false);
                contextInstance = Instantiate(context, transform);
                context.gameObject.SetActive(true);
                _contextRuntimeMap.Add(context.GetType(), contextInstance);
            }

            return contextInstance as TContext;
        }
    }
}