using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Aon_PMS.Server.Services;
using Aon_PMS.Services;
using Aon_PMS.Shared.Models;
using Microsoft.Data.SqlClient;

namespace PMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMasterController : ControllerBase
    {
        private readonly SqlService _sql;

        public UserMasterController(SqlService sql)
        {
            _sql = sql;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UsersM>> Get()
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter{ ParameterName = "@Type", Value= "ALL"},
            };
            return _sql.getDatas<UsersM>("sp_UserMaster", parameters);
        }

        [HttpGet("SEL/{ID}")]
        public ActionResult<UsersM> GetById(Guid id)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter { ParameterName = "@Type", Value = "SEL" },
                new SqlParameter { ParameterName = "@Id", Value = id },
            };
            return _sql.getData<UsersM>("sp_UserMaster", param);
        }

        [HttpPost]
        public ActionResult<IEnumerable<UsersM>> PostUser(UsersM user)
        {
            string json = JsonConvert.SerializeObject(user);
            SqlParameter[] param = new[]
            {
                new SqlParameter { ParameterName = "@Type", Value="INS"},
                new SqlParameter { ParameterName = "@PostData", Value=json},
            };
            _sql.postData("sp_UserMaster", param);
            return Ok("Success");
        }

        [HttpPost("Login")]
        public ActionResult<UsersM> Login(UsersM user)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter { ParameterName = "@Type", Value="Log"},
                new SqlParameter { ParameterName = "@Useranme", Value=user.Username},
                new SqlParameter { ParameterName = "@Password", Value=user.Password}
            };
            var data = _sql.getData<UsersM>("sp_UserMaster", param);
            if (data != null)
                return Ok(data);
            else
                return BadRequest("User Not found!");
        }
    }
}
