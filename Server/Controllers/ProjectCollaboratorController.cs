
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aon_PMS.Server.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using Aon_PMS.Shared.Models;
using Newtonsoft.Json;

namespace Aon_PMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectCollaboratorController : ControllerBase
    {
        private readonly SqlService _sql;

        public ProjectCollaboratorController(SqlService _sqlS)
        {
            _sql = _sqlS;
        }

        [HttpGet("{ProjectID}")]
        public ActionResult<IEnumerable<ProjectCollaboratorsM>> GetCollabes(Guid ProjectID)
        {
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="ALL"},
                new SqlParameter{ParameterName="@ProjectId",Direction=ParameterDirection.Input,Value=ProjectID},
            };
            return _sql.getDatas<ProjectCollaboratorsM>("sp_projectCollaboratord", @params);
        }

        [HttpGet("AssginedProject/{ID}")]
        public ActionResult<ProjectCollaboratorsM> SelectPorjectByID(Guid ID)
        {
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="SEl"},
                new SqlParameter{ParameterName="@ProjectId",Direction=ParameterDirection.Input,Value=ID},
            };
            return _sql.getData<ProjectCollaboratorsM>("sp_projectCollaboratord", @params);
        }

        [HttpPost]
        public ActionResult<IEnumerable<ProjectCollaboratorsM>> PostProject(ProjectCollaboratorsM value)
        {
            string json = JsonConvert.SerializeObject(value);
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="INS"},
                new SqlParameter{ParameterName="@PostData",Direction=ParameterDirection.Input,Value=json},
            };
            _sql.postData("sp_projectCollaboratord", @params);
            return Ok("Success");
        }
    }
}
