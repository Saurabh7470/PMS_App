using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aon_PMS.Shared.Models
{
    public class ProjectCollaboratorsM
    {
        public ProjectCollaboratorsM Clone() => (ProjectCollaboratorsM)this.MemberwiseClone();
        public Guid Id { get; set; }
        public Guid? ProjectID { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? Status { get; set; }

    }
}
