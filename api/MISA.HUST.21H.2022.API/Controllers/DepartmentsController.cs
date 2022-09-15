using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.HUST._21H._2022.API.Entities;
using MySqlConnector;
using Dapper;

namespace MISA.HUST._21H._2022.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách tất cả phòng ban
        /// </summary>
        /// <returns>
        /// Danh sách tất cả phòng ban
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            try
            {
                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo truy vấn SQL
                string query = "SELECT * FROM department;";

                // Thực thi truy vấn
                List<Department> departments = connection.Query<Department>(query).ToList();

                // Đóng kết nối với Database
                connection.Close();

                return StatusCode(StatusCodes.Status200OK, departments);
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }

        /// <summary>
        /// API lấy thông tin chi tiết của một phòng ban
        /// </summary>
        /// <param name="departmentId">Id phòng ban</param>
        /// <returns>
        /// Thông tin chi tiết nếu có
        /// </returns>
        [HttpGet]
        [Route("{departmentId}")]
        public IActionResult GetDepartmentById([FromRoute] Guid departmentId)
        {
            try
            {
                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo truy vấn SQL
                string query = $"SELECT * FROM department WHERE DepartmentId = '{departmentId}';";

                // Thực thi truy vấn
                Department department = connection.QueryFirstOrDefault<Department>(query);

                // Đóng kết nối với Database
                connection.Close();

                // Xử lý kết quả
                if (department != null)
                {
                    return StatusCode(StatusCodes.Status200OK, department);
                }
                else
                {
                    ErrorInfo errorInfo = new(errorCode: "e002", errorMessage: "Bản ghi muốn lấy không tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }
    }
}
