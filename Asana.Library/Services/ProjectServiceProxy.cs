using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asana.Library.Models;

namespace Asana.Library.Services
{
    public class ProjectServiceProxy
    {
        private static ProjectServiceProxy? instance;

        public static ProjectServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectServiceProxy();
                }
                return instance;
            }
        }

        private List<Project> _projects;

        public List<Project> Projects
        {
            get { return _projects.ToList(); }
            private set { _projects = value; }
        }

        private ProjectServiceProxy()
        {
            _projects = new List<Project>
            {
                new Project { Id = 1, Name = "Example Project", Description = "Demo", CompletePercent = 0 }
            };
        }

        public void AddOrUpdateProject(Project project)
        {
            if (project == null) return;

            if (project.Id == 0)
            {
                project.Id = _projects.Any() ? _projects.Max(p => p.Id) + 1 : 1;
                _projects.Add(project);
            }
            else
            {
                var existing = _projects.FirstOrDefault(p => p.Id == project.Id);
                if (existing != null)
                {
                    existing.Name = project.Name;
                    existing.Description = project.Description;
                    existing.CompletePercent = project.CompletePercent;
                }
            }
        }

        public void DeleteProject(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                _projects.Remove(project);
            }
        }
        public Task<List<Project>> GetAllProjectsAsync()
        {
            return Task.FromResult(Projects);
        }

        public Task SaveProjectAsync(Project project)
        {
            AddOrUpdateProject(project);
            return Task.CompletedTask;
        }

    }
}
