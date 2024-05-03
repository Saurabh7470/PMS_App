using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aon_PMS.Shared.Models
{
    public class ProjectMasterM
    {
        public ProjectMasterM Clone() => (ProjectMasterM)this.MemberwiseClone();
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? Status { get; set; }
        public string? Details { get; set; }
        public DateTime? Deadline { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public string? PublishBy { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? ProjectLink { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
