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

        public List<Project> Projects { get; set; }

        private ProjectServiceProxy()
        {
            Projects = new List<Project>
            {
                new Project { Id = 1, Name = "Example Project", Description = "Demo project", CompletePercent = 0 }
            };
        }
    }
}
