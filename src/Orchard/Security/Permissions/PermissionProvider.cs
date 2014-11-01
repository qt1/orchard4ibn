using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Orchard.Environment.Extensions.Models;

namespace Orchard.Security.Permissions {

    /// <summary>
    /// An abstract base class implementing IPermissionProvider
    /// The class automatically exports all Permission fields defined in the derived class
    /// Permission defintion convention:
    ///    public static readonly Permission ViewGuestLists = new Permission { Description = "View Guest Lists", Name = "ViewGuestLists" };
    /// </summary>
    public abstract class PermissionProvider : IPermissionProvider {
        // Optinal: override if this provider should be feature-specific
        public virtual Feature Feature { get; set; }

        /// <summary>
        /// By default: Expose all Permission fields that are defined by this provider
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Permission> GetPermissions() {
            return this.GetType().GetMembers()
                .OfType<FieldInfo>()
                .Where(i => i.FieldType.IsAssignableFrom(typeof(Permission)))
                .Select(i => (Permission)(i.GetValue(this)));
        }

        public abstract IEnumerable<PermissionStereotype> GetDefaultStereotypes();
    }

}