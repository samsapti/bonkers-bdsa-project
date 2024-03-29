﻿using Microsoft.EntityFrameworkCore;
using static ProjectBank.Server.Entities.Response;

namespace ProjectBank.Server.Entities
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly ProjectBankContext _context;

        public ApplicantRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public async Task<Response> ApplyToProjectAsync(int projectId, int userId)
        {
            var conflict =
                await _context.Applicants
                .Where(a => a.ProjectId == projectId)
                .Where(a => a.StudentId == userId)
                .Select(a => new ApplicantCreateDTO(a.ProjectId, a.StudentId))
                .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Conflict);
            }

            var entity = new Applicant { ProjectId = projectId, StudentId = userId };

            _context.Applicants.Add(entity);

            await _context.SaveChangesAsync();

            return (Created);
        }
        public async Task<Response> HasAlreadyAppliedToProjectAsync(int projectId, int userId)
        {
            var conflict =
            await _context.Applicants
            .Where(a => a.ProjectId == projectId)
            .Where(a => a.StudentId == userId)
            .Select(a => new ApplicantCreateDTO(a.ProjectId, a.StudentId))
            .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Exists);
            }
            else
            {
                return (NotFound);
            }
        }
        public async Task<List<SimplifiedProjectDTO>> GetAppliedProjectsAsync(int userId)
        {
            var projects =
                await (from Applicant in _context.Applicants
                       join Projects in _context.Projects
                       on Applicant.ProjectId equals Projects.Id
                       where Applicant.StudentId == userId
                       select new SimplifiedProjectDTO(Projects.Id, Projects.Name)).ToListAsync();

            return projects;
        }
        public async Task<int> GetNrOfProjectApplicationsAsync(int projectId)
        {
            var projects = await (from a in _context.Applicants
                                  where a.ProjectId == projectId
                                  select a).CountAsync();
            return projects;
        }
        public async Task<Response> DeleteApplicationsAsync(int projectId)
        {
            var entities =
                    await (from a in _context.Applicants
                           where a.ProjectId == projectId
                           select new ApplicantCreateDTO(a.ProjectId, a.StudentId)).ToListAsync();


            if (entities.Count == 0)
            {
                return NotFound;
            }

            _context.Applicants.RemoveRange(_context.Applicants.Where(a => a.ProjectId == projectId));

            await _context.SaveChangesAsync();

            return Deleted;
        }
    }
}
