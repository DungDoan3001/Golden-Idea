using Microsoft.AspNetCore.Identity;

namespace Web.Api.Entities.Configuration
{
    public static class IdentityRoles
    {
        public const string Administrator = "Administrator";
        public const string QAManager = "QA Manager";
        public const string QACoordinator = "QA Coordinator";
        public const string Staff = "Staff";

        public static IdentityRole Administrator_Identity = new IdentityRole
        {
            Name = Administrator,
            NormalizedName = Administrator.ToUpper()
        };
        public static IdentityRole QAManager_Identity = new IdentityRole
        {
            Name = QAManager,
            NormalizedName = QAManager.ToUpper()
        };
        public static IdentityRole QACoordinator_Identity = new IdentityRole
        {
            Name = QACoordinator,
            NormalizedName = QACoordinator.ToUpper()
        };
        public static IdentityRole Staff_Identity = new IdentityRole
        {
            Name = Staff,
            NormalizedName = Staff.ToUpper()
        };
    }
}
