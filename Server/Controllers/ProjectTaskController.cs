using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aon_PMS.Server.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using Aon_PMS.Shared.Models;
using Newtonsoft.Json;

namespace Aon_PMSs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTaskController : ControllerBase
    {
        private readonly SqlService _sql;

        public ProjectTaskController(SqlService _sqlS)
        {
            _sql = _sqlS;
        }

        [HttpGet("{ProjectID}")]
        public ActionResult<IEnumerable<ProjectTaskM>> GetProjects(Guid ProjectId)
        {
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="ALL"},
                 new SqlParameter{ParameterName="@ProjectID",Direction=ParameterDirection.Input,Value=ProjectId},
            };
            return _sql.getDatas<ProjectTaskM>("sp_projectTask", @params);
        }

        [HttpGet("SEL/{ID}")]
        public ActionResult<IEnumerable<ProjectTaskM>> SelectPorjectByID(Guid ID)
        {
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="SEL"},
                new SqlParameter{ParameterName="@id",Direction=ParameterDirection.Input,Value=ID},
            };
            return _sql.getDatas<ProjectTaskM>("sp_projectTask", @params);
        }

        [HttpGet("PEN")]
        public ActionResult<IEnumerable<ProjectTaskM>> Pendingtask()
        {
            SqlParameter[] param =
            {
                new SqlParameter { ParameterName = "@Type", Value = "PEN", Direction = ParameterDirection.Input }
            };
            return _sql.getDatas<ProjectTaskM>("sp_projectTask", @param);
        }

        [HttpPost]
        public ActionResult<IEnumerable<ProjectTaskM>> PostProjectTask(ProjectTaskM value)
        {
            string json = JsonConvert.SerializeObject(value);
            SqlParameter[] @params =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input,Value="INS"},
                new SqlParameter{ParameterName="@PostData",Direction=ParameterDirection.Input,Value=json},
            };
            _sql.postData("sp_projectTask", @params);
            return Ok("Success");
        }

        [HttpPost("ACT")]
        public ActionResult<IEnumerable<ProjectTaskM>> PostAction(ProjectTaskM value)
        {
            string json = JsonConvert.SerializeObject(value);
            SqlParameter[] param =
            {
                new SqlParameter{ParameterName="@Type",Direction=ParameterDirection.Input, Value="ACT"},
                new SqlParameter{ParameterName="@PostData",Direction=ParameterDirection.Input, Value=json},
                new SqlParameter{ParameterName="@AssignedOn", Direction=ParameterDirection.Input, Value=value.AssignedOn},
                new SqlParameter{ParameterName="@CompletedOn",Direction=ParameterDirection.Input, Value=value.CompletedOn},
                new SqlParameter{ParameterName="@UATOn",Direction=ParameterDirection.Input, Value=value.UATOn},
                new SqlParameter{ParameterName="@PublishOn",Direction= ParameterDirection.Input, Value=value.PublishOn},
            };
            _sql.postData("sp_projectTask", param);
            return Ok("Action Saved.");
        }
    }
}
