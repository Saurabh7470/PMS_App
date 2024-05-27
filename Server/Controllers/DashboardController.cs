using Aon_PMS.Server.Services;
using Aon_PMS.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Aon_PMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        public readonly SqlService _sql;

        public DashboardController(SqlService sql)
        {
            _sql = sql;
        }

        [HttpGet]
        public ActionResult<DashboardM> GetProjects()
        {
            SqlParameter[] parameters =
            {
                new SqlParameter{ ParameterName="@Type", Value="ALL"},
            };
            return _sql.getData<DashboardM>("sp_Dashboard", parameters);
        }
    }
}
