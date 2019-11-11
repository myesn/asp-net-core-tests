using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheckTests
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(""))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException)
                {
                    return HealthCheckResult.Healthy();
                }
            }

            return HealthCheckResult.Healthy();
        }
    }
}
