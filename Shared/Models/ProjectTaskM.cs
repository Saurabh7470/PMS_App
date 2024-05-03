using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aon_PMS.Shared.Models
{
    public class ProjectTaskM
    {
        public ProjectTaskM Clone() => (ProjectTaskM)this.MemberwiseClone();
        public Guid Id { get; set; }
        public Guid? ProjectID { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? AssignedTo { get; set; }
        public DateTime? AssignedOn { get; set; }
        public string? TaskName { get; set; }
        public string? Type { get; set; }
        public string? Details { get; set; }
        public DateTime? CompletedOn { get; set; }
        public DateTime? UATOn { get; set; }
        public DateTime? PublishOn { get; set; }
        public string? CompletedRemark { get; set; }
        public string? UATRemark { get; set; }
        public string? PublishRemark { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? Status { get; set; }

        public HashSet<ProjectTaskM> child { get; set; } = new HashSet<ProjectTaskM>();

    }
}
