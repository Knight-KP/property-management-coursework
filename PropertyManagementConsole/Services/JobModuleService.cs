using System;
using System.Collections.Generic;
using PropertyManagementConsole.Data.Repositories;
using PropertyManagementConsole.Models;

namespace PropertyManagementConsole.Services;

public class JobModuleService
{
    private readonly MaintenanceRepository _maintenanceRepository = new MaintenanceRepository();

    public string ModuleName { get; }
    public string JobType { get; }

    public JobModuleService(string moduleName, string jobType)
    {
        ModuleName = moduleName;
        JobType = jobType;
    }

    public void AddJob(int tenantId, int flatId, DateTime jobDate, decimal cost, string? notes)
    {
        var newJob = new MaintenanceJob
        {
            TenantId = tenantId,
            FlatId = flatId,
            JobType = JobType,
            JobDate = jobDate,
            Cost = cost,
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim()
        };

        _maintenanceRepository.AddJob(newJob);
    }

    public List<MaintenanceJob> GetJobsForTenant(int tenantId)
    {
        return _maintenanceRepository.GetJobsByTenantAndType(tenantId, JobType);
    }

    public List<MaintenanceJob> GetJobsForTenantMonth(int tenantId, int month, int year)
    {
        return _maintenanceRepository.GetJobsByTenantTypeMonth(tenantId, JobType, month, year);
    }
}
