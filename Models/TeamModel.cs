using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.Models
{
    public class TeamModel
    {
        public string TeamName { get; set; }

        public Guid TeamId { get; set; }
        public string TeamUrl { get; set; }

        public List<TeamMemberModel> TeamMembers { get; set; }
        public bool IsDefaultTeam { get; set; }
    }


}
