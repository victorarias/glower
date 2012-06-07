using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlowerFramework;
using NUnit.Framework;
using SharpTestsEx;

namespace Tests
{
    [TestFixture]
    public class GlowerTests
    {
        [Test]
        public void ShouldBindGenericTypes()
        {
            var glower = new Glower();

            glower.Bind<IFooDependency, FooDependency>();

            var resolved = glower.Resolve<IFooDependency>();
            resolved.Should().Not.Be.Null();
            resolved.Should().Be.OfType<FooDependency>();
        }

        [Test]
        public void ShouldResolveTypesWithoutDependencies()
        {
            var glower = new Glower();

            var resolved = glower.Resolve<FooDependency>();

            resolved.Should().Not.Be.Null();
        }

        [Test]
        public void ShouldResolveTypeWithDependencies()
        {
            var glower = new Glower();

            glower.Bind<IFooDependency, FooDependency>();
            glower.Bind<INestedDependency, NestedDependency>();
            glower.Bind<IAnotherFooDependency, AnotherFooDependency>();

            var foo = glower.Resolve<Foo>();

            foo.Should().Not.Be.Null();
        }

        [Test]
        public void ShouldResolveBindedInstance()
        {
            var glower = new Glower();
            var instance = new FooDependency();

            glower.Bind(instance);

            var resolved = glower.Resolve<FooDependency>();

            resolved.Should().Be.SameInstanceAs(instance);
        }

        interface IFoo { void DoSomething(); }
        class Foo : IFoo
        {
            public Foo(IFooDependency dependency, IAnotherFooDependency anotherDependency)
            {
                if (null == dependency) throw new ArgumentNullException();
                if (null == anotherDependency) throw new ArgumentNullException();
            }

            public void DoSomething()
            {
                throw new NotImplementedException();
            }
        }

        interface IFooDependency { void DoSomethingDifferent(); }
        public class FooDependency : IFooDependency
        {
            public void DoSomethingDifferent()
            {
                throw new NotImplementedException();
            }
        }

        interface INestedDependency { }
        public class NestedDependency : INestedDependency { }

        interface IAnotherFooDependency { void Blofs();}
        class AnotherFooDependency : IAnotherFooDependency
        {
            public AnotherFooDependency(INestedDependency nestedDependency)
            {
                if (null == nestedDependency) throw new ArgumentNullException();
            }
            public void Blofs()
            {
                throw new NotImplementedException();
            }
        }


    }
}
