using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aon_PMS.Shared.Models
{
    public class UsersM
    {
        public Guid Id { get; set; }
        public string? employeeCode { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public int? Status { get; set; }
    }
}
