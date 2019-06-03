using System;
using System.Linq;
using System.Reflection;
using Ustn.Frameworks.Fx.System;
using Xunit;

namespace Ustn.Frameworks.Fx.Tests.System
{
    public class FxObjectTest
    {
        [Fact]
        public void AddChildGivesChildCorrectParent()
        {
            // arrange
            var child = new FxObject();

            var type = typeof(FxObject);
            var parent = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("AddChild", BindingFlags.NonPublic | BindingFlags.Instance);

            // act
            method.Invoke(parent, new object[] { child });

            // assert
            Assert.Same(child.Parent, parent);
        }

    }
}
