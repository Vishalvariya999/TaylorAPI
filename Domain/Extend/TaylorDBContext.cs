using Domain.Extend;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Domain.Data
{
    public partial class TaylorDBContext
    {
        private IHttpContextAccessor _httpContextAccessor;
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleAuditEntries();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            HandleAuditEntries();
            return base.SaveChanges();
        }

        #region Private

        private void HandleAuditEntries()
        {
            var addedAuditedEntities = ChangeTracker.Entries<IAuditable>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            var addedAuditedEntities1 = ChangeTracker.Entries()
               .Where(p => p.State == EntityState.Added)
               .Select(p => p.Entity);


            var modifiedAuditedEntities = ChangeTracker.Entries<IAuditable>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

            _httpContextAccessor ??= new HttpContextAccessor();

            var currentUserDetails = !string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value) ? _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value : "1";
            int.TryParse(currentUserDetails, out var currentUser);
            //var userId = 1;

            var now = DateTime.UtcNow;
            foreach (var added in addedAuditedEntities)
            {
                added.Created = now;
                added.Updated = now;
                added.CreatedBy = currentUser;
                added.UpdatedBy = currentUser;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.Updated = now;
                modified.UpdatedBy = currentUser;
            }
        }
        #endregion
    }
}
