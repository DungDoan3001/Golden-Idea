using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Services.DepartmentService;

namespace Web.Api.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IMapper mapper, IDepartmentService departmentService)
        {
            _mapper = mapper;
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get all departments.
        /// </summary>
        /// <returns>List of Department objects</returns>
        /// <response code="200">Successfully get all departments</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<DepartmentResponseModel>>> GetAll()
        {
            try
            {
                IEnumerable<Department> departments = await _departmentService.GetAllAsync();
                IEnumerable<DepartmentResponseModel> departmentRespones = _mapper.Map<IEnumerable<DepartmentResponseModel>>(departments);
                return Ok(departmentRespones);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message , StatusCode = (int)HttpStatusCode.BadRequest});
            }
        }

        /// <summary>
        /// Get a department by Id.
        /// </summary>
        /// <param name="id">Id of the department</param>
        /// <returns>A department with the given Id</returns>
        /// <response code="200">Successfully get the department with the given Id</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no department with the given Id</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentResponseModel>> GetById([FromRoute] Guid id)
        {
            try
            {
                Department department = await _departmentService.GetByIdAsync(id);
                if (department == null)
                {
                    return NotFound(new MessageResponseModel { Message = "Not found.", StatusCode = (int)HttpStatusCode.NotFound});
                }
                DepartmentResponseModel departmentRespone = _mapper.Map<DepartmentResponseModel>(department);
                return Ok(departmentRespone);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest});
            }
        }

        /// <summary>
        /// Create a department
        /// </summary>
        /// <param name="requestModel">Request model for department</param>
        /// <returns>A department just created</returns>
        /// <response code="201">Successfully created the department</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while create a department</response>
        [HttpPost("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<DepartmentResponseModel>> Create([FromBody] DepartmentRequestModel requestModel)
        {
            try
            {
                bool check = await CheckExist(requestModel.Name);
                if (check)
                    Conflict(new MessageResponseModel { Message = "The name already existed", StatusCode = (int)HttpStatusCode.Conflict });
                Department department = _mapper.Map<Department>(requestModel);
                Department createdDepartment = await _departmentService.CreateAsync(department);
                if (createdDepartment == null)
                    return Conflict(new MessageResponseModel { Message = "Error while create new.", StatusCode = (int)HttpStatusCode.Conflict});
                return Created(createdDepartment.Id.ToString(), _mapper.Map<DepartmentResponseModel>(createdDepartment));
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest});
            }
        }

        /// <summary>
        /// Update a department
        /// </summary>
        /// <param name="requestModel">Request model for department.</param>
        /// <param name="id">Id of the department to be updated.</param>
        /// <returns>A department just updated</returns>
        /// <response code="200">Successfully updated the department</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while update a department</response>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<DepartmentResponseModel>> Update([FromRoute] Guid id, [FromBody] DepartmentRequestModel requestModel)
        {
            try
            {
                bool check = await CheckExist(requestModel.Name);
                if (check)
                    Conflict(new MessageResponseModel { Message = "The name already existed", StatusCode = (int)HttpStatusCode.Conflict });
                Department department = await _departmentService.GetByIdAsync(id);
                if (department == null) return NotFound(new MessageResponseModel { Message = "Not found.", StatusCode = (int)HttpStatusCode.NotFound});
                _mapper.Map<DepartmentRequestModel, Department>(requestModel, department);
                Department updatedDepartment = await _departmentService.UpdateAsync(department);
                return Ok(_mapper.Map<DepartmentResponseModel>(updatedDepartment));
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest});
            }
        }

        /// <summary>
        /// Delete a department
        /// </summary>
        /// <param name="id">Id of the department to be deleted.</param>
        /// <returns>null</returns>
        /// <response code="200">Successfully deleted the department</response>
        /// <response code="204">Successfully deleted the department</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no department with the given Id</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                Department department = await _departmentService.GetByIdAsync(id);
                if (department == null) return NotFound(new MessageResponseModel { Message = "Not found.", StatusCode = (int)HttpStatusCode.NotFound });
                bool isDelete = await _departmentService.DeleteAsync(id);
                if (!isDelete)
                    return NotFound(new MessageResponseModel { Message = "Error while update.", StatusCode = (int)HttpStatusCode.NotFound });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        private async Task<bool> CheckExist(string name)
        {
            IEnumerable<Department> checkDepartments = await _departmentService.GetByNameAsync(name);
            if (checkDepartments.Any())
                return true;
            return false;
        }
    }
}
