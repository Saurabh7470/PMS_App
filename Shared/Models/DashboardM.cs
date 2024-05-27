using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aon_PMS.Shared.Models
{
    public class DashboardM
    {
        public int TotalProject {get; set;}
        public int Completedproject { get; set;}
        public int PendingProject { get; set;}
        public int TotalTask { get; set;}
        public int PendingTask { get; set;}
        public int CompletedTask { get; set;}
    }
}
