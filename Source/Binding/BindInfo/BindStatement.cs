using ModestTree;
using System;
using System.Collections.Generic;
using Zenject.Internal;

namespace Zenject
{
    [NoReflectionBaking]
    public class BindStatement : IDisposable
    {
        private readonly List<IDisposable> _disposables;
        private IBindingFinalizer _bindingFinalizer;

        public BindStatement()
        {
            _disposables = new List<IDisposable>();
            Reset();
        }

        public BindingInheritanceMethods BindingInheritanceMethod
        {
            get
            {
                AssertHasFinalizer();
                return _bindingFinalizer.BindingInheritanceMethod;
            }
        }

        public bool HasFinalizer
        {
            get { return _bindingFinalizer != null; }
        }

        public void SetFinalizer(IBindingFinalizer bindingFinalizer)
        {
            _bindingFinalizer = bindingFinalizer;
        }

        private void AssertHasFinalizer()
        {
            if (_bindingFinalizer == null)
            {
                throw Assert.CreateException(
                    "Unfinished binding!  Some required information was left unspecified.");
            }
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public BindInfo SpawnBindInfo()
        {
            BindInfo bindInfo = ZenPools.SpawnBindInfo();
            AddDisposable(bindInfo);
            return bindInfo;
        }

        public void FinalizeBinding(DiContainer container)
        {
            AssertHasFinalizer();
            _bindingFinalizer.FinalizeBinding(container);
        }

        public void Reset()
        {
            _bindingFinalizer = null;

            int count = _disposables.Count;
            for (int i = 0; i < count; i++)
            {
                _disposables[i].Dispose();
            }

            _disposables.Clear();
        }

        public void Dispose()
        {
            ZenPools.DespawnStatement(this);
        }
    }
}
