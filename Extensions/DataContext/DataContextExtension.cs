using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace WebBanHang.Extensions.DataContext
{
    public static class DataContextExtension
    {
        public static async Task<int> SaveChangeWithValidationAsync(this DbContext context, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            Validate(context);

            return await context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public static async Task<int> SaveChangeWithValidationAsync(this DbContext context, CancellationToken cancellationToken = default)
        {

            Validate(context);
            return await context.SaveChangesAsync(cancellationToken);
        }

        private static void Validate(DbContext context)
        {
            var records = context.ChangeTracker.Entries();

            foreach (var record in records)
            {
                var entity = record.Entity;
                var validationContext = new ValidationContext(entity);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(entity, validationContext, results, true))
                {
                    var messages = results.Select(r => r.ErrorMessage).ToList().Aggregate((message, nextMessage) => message + ", " + nextMessage);
                    throw new ApplicationException($"Unable to save changes for {entity.GetType().FullName} due to error(s): {messages}");
                }
            }
        }
    }
}
