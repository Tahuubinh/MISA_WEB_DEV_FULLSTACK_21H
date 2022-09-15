using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.HUST._21H._2022.API.Entities;
using MySqlConnector;
using Dapper;

namespace MISA.HUST._21H._2022.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách tất cả vị trí
        /// </summary>
        /// <returns>
        /// Danh sách tất cả vị trí
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        [HttpGet]
        public IActionResult GetAllPosition()
        {
            try
            {   
                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo truy vấn SQL
                string query = "SELECT * FROM positions;";

                // Thực thi truy vấn
                List<Position> positions = connection.Query<Position>(query).ToList();

                // Đóng kết nối với Database
                connection.Close();

                return StatusCode(StatusCodes.Status200OK, positions);
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }

        /// <summary>
        /// API lấy thông tin chi tiết của một vị trí
        /// </summary>
        /// <param name="positionId">Id vị trí</param>
        /// <returns>
        /// Thông tin chi tiết nếu có
        /// </returns>
        [HttpGet]
        [Route("{positionId}")]
        public IActionResult GetPositionById([FromRoute] Guid positionId)
        {
            try
            {
                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo truy vấn SQL
                string query = $"SELECT * FROM positions WHERE PositionId = '{positionId}';";

                // Thực thi truy vấn
                Position position = connection.QueryFirstOrDefault<Position>(query);

                // Đóng kết nối với Database
                connection.Close();

                // Xử lý kết quả
                if (position != null)
                {
                    return StatusCode(StatusCodes.Status200OK, position);
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
