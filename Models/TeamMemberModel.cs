using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.Models
{
    public class TeamMemberModel
    {

        public string MemberName { get; set; }//displayName

        public string MemberId { get; set; }

        public string Email { get; set; }
        public string MemberUrl { get; set; }


    }
}
