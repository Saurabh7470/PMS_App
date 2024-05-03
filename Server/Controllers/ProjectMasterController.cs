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
    public class ProjectMasterController : ControllerBase
    {
        private readonly SqlService _sql;

        public ProjectMasterController(SqlService _sqlS)
        {
            _sql = _sqlS;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProjectMasterM>> GetProjects()
        {
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="ALL"},
            };
            return _sql.getDatas<ProjectMasterM>("sp_ProjectMaster", @params);
        }

        [HttpGet("COL/{Collaborator}")]
        public ActionResult<IEnumerable<ProjectMasterM>> GetProjectsByCollaborator(Guid Collaborator)
        {
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="COL"},
                new SqlParameter{ParameterName="@id",Direction=ParameterDirection.Input,Value=Collaborator},
            };
            return _sql.getDatas<ProjectMasterM>("sp_ProjectMaster", @params);
        }

        [HttpGet("{ID}")]
        public ActionResult<ProjectMasterM> SelectPorjectByID(Guid ID)
        {
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="SEL"},
                new SqlParameter{ParameterName="@Id",Direction=ParameterDirection.Input,Value=ID},
            };
            return _sql.getData<ProjectMasterM>("sp_ProjectMaster", @params);
        }
        [HttpPost]
        public ActionResult<IEnumerable<ProjectMasterM>> PostProject(ProjectMasterM value)
        {
            string json = JsonConvert.SerializeObject(value);
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="INS"},
                new SqlParameter{ParameterName="@PostData",Direction=ParameterDirection.Input,Value=json},
            };
            _sql.postData("sp_ProjectMaster", @params);
            return Ok("Success");
        }
    }
}
