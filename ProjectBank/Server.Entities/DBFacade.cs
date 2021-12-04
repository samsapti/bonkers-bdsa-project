using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static ProjectBank.Server.Entities.Response;
using ProjectBank.Server;

namespace ProjectBank.Server.Entities
{
    public class DBFacade
    {
        private readonly ProjectBankContext _context;
        private readonly ProjectRepository _projectRepo;
        private readonly ApplicantRepository _applicantRepo;
        private readonly ViewRepository _viewRepo;
        public DBFacade(ProjectBankContext context)
        {
            _context = context;
            _projectRepo = new ProjectRepository(context);
            _applicantRepo = new ApplicantRepository(context);
            _viewRepo = new ViewRepository(context);
        }

        public Task<(Response, ProjectDTO)> CreateProject(string name, string description, int authorId)
        {
            var project = new ProjectCreateDTO(name, description, authorId);

            return _projectRepo.CreateAsync(project);
        }

        public Task<ProjectDTO> SelectProject(int projectId)
        {
            return _projectRepo.ReadAsync(projectId);
        }

        public Task<Response> DeleteProject(int projectId)
        {
            return _projectRepo.DeleteAsync(projectId);
        }

        public Task<List<SimplifiedProjectDTO>> ShowAllProjects()
        {
            return _projectRepo.ListAllProjectsAsync();
        }

        public Task<List<SimplifiedProjectDTO>> ShowCreatedProjects(int authorId)
        {
            return _projectRepo.ShowCreatedProjectsAsync(authorId);
        }

        public Task<Response> ApplyToProject(int projectId, int studentId)
        {
            return _applicantRepo.ApplyToProjectAsync(projectId, studentId);
        }

        public Task<Response> HasAlreadyAppliedToProject(int projectid, int studentId)
        {
            return _applicantRepo.HasAlreadyAppliedToProjectAsync(projectid,studentId);
        }

        public Task<List<SimplifiedProjectDTO>> ShowListOfAppliedProjects(int studentId)
        {
            return _applicantRepo.ShowListOfAppliedProjectsAsync(studentId);
        }

        public Task<int> SelectNrOfProjectApplications(int projectId)
        {
            return _applicantRepo.SelectNrOfProjectApplicationsAsync(projectId);
        }

        public Task<Response> DeleteApplications(int projectId)
        {
            return _applicantRepo.DeleteApplicationsAsync(projectId);
        }

        public Task<Response> AddView(int projectId, int studentId)
        {
            return _viewRepo.AddViewAsync(projectId,studentId);
        }

        public Task<int> GetViewsOfProject(int projectId)
        {
            return _viewRepo.GetViewsOfProjectAsync(projectId);
        }

        public Task<Response> DeleteViews(int projectId)
        {
            return _viewRepo.DeleteViewAsync(projectId);
        }
    }

}