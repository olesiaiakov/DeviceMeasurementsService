using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace DeviceMessagesConsumer.Processing
{
    public static class DbUpdateExceptionExtension
    {
        private static readonly int[] KeyViolationSqlErrorNumbers =
        {
            2601, // Unique index concurrency exception (2601)
        };

        public static bool IsKeyViolation(this DbUpdateException exception)
        {
            var sqlException = exception.InnerException?.InnerException as SqlException;

            if (sqlException == null)
            {
                return false;
            }

            return KeyViolationSqlErrorNumbers.Contains(sqlException.Number);
        }
    }
}