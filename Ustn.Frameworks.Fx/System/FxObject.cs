using System;
using System.Collections.Generic;
using System.Reflection;
using Ustn.Frameworks.Fx.System.Enums;

namespace Ustn.Frameworks.Fx.System
{
    public class FxObject
    {
        public FxObject Parent { get; private set; }

        protected static IServiceProvider FxServiceProvider { get; set; }

        public static void SetServiceProvider(IServiceProvider provider)
        {
            FxServiceProvider = provider;
        }

        private List<FxObject> _children;

        public Guid Id { get; }

        public DelegationMode DelegationMode { get; set; } = DelegationMode.AutoDelegate;

        public FxObject()
        {
            Id = Guid.NewGuid();
        }

        protected void MarkAsChild(FxObject obj)
        {
            obj.Parent = this;

            // deferred creation of Children...
            if (_children == null)
            {
                _children = new List<FxObject>();
            }

            _children.Add(obj);
        }

        protected void AddChild(FxObject obj)
        {
            MarkAsChild(obj);
        }

        protected void RemoveChild(FxObject obj)
        {
            _children.Remove(obj);
        }

        public void Send(string method, params object[] args)
        {
            Type type = GetType();
            MethodInfo info = type.GetMethod(method); // with one string-argument Getmethod is only looking for public methods

            if (info != null && DelegationMode != DelegationMode.DelegateToParent)
            {
                info.Invoke(this, args);
            }
            else if (DelegationMode != DelegationMode.NoDelegate)
            {
                Parent?.Send(method, args);
            }
        }

        public void DelegateSend(string method, params object[] args)
        {
            Parent?.Send(method, args);
        }

        public void Broadcast(string method, params object[] args)
        {
            foreach (FxObject child in _children)
            {
                child.Send(method, args);
                if (child._children != null)
                {
                    child.Broadcast(method, args);
                }
            }
        }
    }
}