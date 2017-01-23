using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhizTeamServices.NetCoreConsole.RestAPIModels
{
    public class ProjectCreator
    {
        public string name { get; set; }
        public string description { get; set; }
        public Capabilities capabilities { get; set; }

        public ProjectCreator(string projectName, string projectDescription, string versionControlType, string templateId)
        {
            name = projectName;
            description = projectDescription;
            capabilities = new Capabilities()
            {
                versioncontrol = new Versioncontrol()
                {
                    sourceControlType = versionControlType
                },
                processTemplate = new ProcessTemplate()
                {
                    templateTypeId = templateId
                }
            };
        }

    }

    public class Versioncontrol
    {
        public string sourceControlType { get; set; }
    }

    public class ProcessTemplate
    {
        public string templateTypeId { get; set; }
    }

    public class Capabilities
    {
        public Versioncontrol versioncontrol { get; set; }
        public ProcessTemplate processTemplate { get; set; }
    }

}
