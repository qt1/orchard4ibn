using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Orchard.Caching;
using Orchard.Environment.Extensions.Models;
using Orchard.Roles.Models;
using Orchard.Roles.Services;
using Orchard.Security.Permissions;
using Orchard.Tests.Stubs;

namespace Orchard.Tests.Modules.Roles.Services {

    /// <summary>
    /// This is a variant that uses PermissionProvider base class instead of direct implementation of IPermissionProvider
    /// </summary>
    [TestFixture]
    public class RoleServiceTests2 : DatabaseEnabledTestsBase {
        public override void Register(ContainerBuilder builder) {
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<StubCacheManager>().As<ICacheManager>();
            builder.RegisterType<Signals>().As<ISignals>();
            builder.RegisterType<TestPermissionProviderClass>().As<IPermissionProvider>();
        }

        public class TestPermissionProviderClass : PermissionProvider {
            public static readonly Permission Alpha = new Permission { Description = "alpha permission", Name = "alpha" };
            public static readonly Permission Beta = new Permission { Description = "alpha permission", Name = "beta" };
            public static readonly Permission Gamma = new Permission { Description = "alpha permission", Name = "gamma" };
            public static readonly Permission Delta = new Permission { Description = "alpha permission", Name = "delta" };

            public override Feature Feature {
                get { return new Feature { Descriptor = new FeatureDescriptor { Id = "RoleServiceTests" } }; }
            }

            public override IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
                return Enumerable.Empty<PermissionStereotype>();
            }
        }

        protected override IEnumerable<Type> DatabaseTypes {
            get {
                return new[] { typeof(RoleRecord), typeof(PermissionRecord), typeof(RolesPermissionsRecord) };
            }
        }

        [Test]
        public void CreateRoleShouldAddToList() {
            var service = _container.Resolve<IRoleService>();
            service.CreateRole("one");
            service.CreateRole("two");
            service.CreateRole("three");

            ClearSession();

            var roles = service.GetRoles();
            Assert.That(roles.Count(), Is.EqualTo(3));
            Assert.That(roles, Has.Some.Property("Name").EqualTo("one"));
            Assert.That(roles, Has.Some.Property("Name").EqualTo("two"));
            Assert.That(roles, Has.Some.Property("Name").EqualTo("three"));
        }

        [Test]
        public void PermissionChangesShouldBeVisibleImmediately() {

            var service = _container.Resolve<IRoleService>();

            ClearSession();
            {
                service.CreateRole("test");
                var roleId = service.GetRoleByName("test").Id;
                service.UpdateRole(roleId, "test", new[] { "alpha", "beta", "gamma" });
            }

            ClearSession();
            {
                var result = service.GetPermissionsForRoleByName("test");
                Assert.That(result.Count(), Is.EqualTo(3));
            }

            ClearSession();
            {
                var roleId = service.GetRoleByName("test").Id;
                service.UpdateRole(roleId, "test", new[] { "alpha", "beta", "gamma", "delta" });
            }

            ClearSession();
            {
                var result = service.GetPermissionsForRoleByName("test");
                Assert.That(result.Count(), Is.EqualTo(4));
            }
        }

        [Test]
        public void ShouldNotCreateARoleTwice() {
            var service = _container.Resolve<IRoleService>();
            service.CreateRole("one");
            service.CreateRole("two");
            service.CreateRole("one");

            ClearSession();

            var roles = service.GetRoles();
            Assert.That(roles.Count(), Is.EqualTo(2));
            Assert.That(roles, Has.Some.Property("Name").EqualTo("one"));
            Assert.That(roles, Has.Some.Property("Name").EqualTo("two"));
        }
    }
}
