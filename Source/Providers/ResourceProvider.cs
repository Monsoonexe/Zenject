#if !NOT_UNITY3D

using ModestTree;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    [NoReflectionBaking]
    public class ResourceProvider : IProvider
    {
        private readonly Type _resourceType;
        private readonly string _resourcePath;
        private readonly bool _matchSingle;

        public ResourceProvider(
            string resourcePath, Type resourceType, bool matchSingle)
        {
            _resourceType = resourceType;
            _resourcePath = resourcePath;
            _matchSingle = matchSingle;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _resourceType;
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEmpty(args);

            Assert.IsNotNull(context);

            if (_matchSingle)
            {
                UnityEngine.Object obj = Resources.Load(_resourcePath, _resourceType);

                Assert.That(obj != null,
                "Could not find resource at path '{0}' with type '{1}'", _resourcePath, _resourceType);

                // Are there any resource types which can be injected?
                injectAction = null;
                buffer.Add(obj);
                return;
            }

            UnityEngine.Object[] objects = Resources.LoadAll(_resourcePath, _resourceType);

            Assert.That(objects.Length > 0,
            "Could not find resource at path '{0}' with type '{1}'", _resourcePath, _resourceType);

            // Are there any resource types which can be injected?
            injectAction = null;

            buffer.AllocFreeAddRange(objects);
        }
    }
}

#endif


