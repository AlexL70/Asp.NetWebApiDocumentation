using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cms.Data.Repository.Models;
using Cms.Data.Repository.Repositories;
using Cms.WebApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cms.WebApi.Controllers
{
    /// <summary>
    /// This controller exposes methods for working with courses and maintaining
    /// a list of students for each course.  
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICmsRepository cmsRepository;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="cmsRepository">Courses repository</param>
        /// <param name="mapper">Mapper</param>
        public CoursesController(ICmsRepository cmsRepository, IMapper mapper)
        {
            this.cmsRepository = cmsRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets full list of courses
        /// </summary>
        /// <remarks>This web method returns collection of all courses available in the CMS system</remarks>
        /// <response code="200">Successfully executed; returns a collection of CourseDto structures</response> 
        /// <response code="500">Internal error; returns error text</response>
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCourses()
        {
            try
            {
                IEnumerable<Course> courses = cmsRepository.GetAllCourses();
                var result = mapper.Map<CourseDto[]>(courses);
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<CourseDto> AddCourse([FromBody] CourseDto course)
        {
            try
            {
                var newCourse = mapper.Map<Course>(course);
                newCourse = cmsRepository.AddCourse(newCourse);
                return mapper.Map<CourseDto>(newCourse);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseDto> GetCourse(int courseId)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Course course = cmsRepository.GetCourse(courseId);
                var result = mapper.Map<CourseDto>(course);
                return result;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{courseId}")]
        public ActionResult<CourseDto> UpdateCourse(int courseId, CourseDto course)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Course updatedCourse = mapper.Map<Course>(course);
                updatedCourse = cmsRepository.UpdateCourse(courseId, updatedCourse);
                var result = mapper.Map<CourseDto>(updatedCourse);

                return result;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public ActionResult<CourseDto> DeleteCourse(int courseId)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Course course = cmsRepository.DeleteCourse(courseId);

                if (course == null)
                    return BadRequest();

                var result = mapper.Map<CourseDto>(course);
                return result;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET ../courses/1/students
        [HttpGet("{courseId}/students")]
        public ActionResult<IEnumerable<StudentDto>> GetStudents(int courseId)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                IEnumerable<Student> students = cmsRepository.GetStudents(courseId);
                var result = mapper.Map<StudentDto[]>(students);
                return result;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST ../courses/1/students
        [HttpPost("{courseId}/students")]
        public ActionResult<StudentDto> AddStudent(int courseId, StudentDto student)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Student newStudent = mapper.Map<Student>(student);

                // Assign course
                Course course = cmsRepository.GetCourse(courseId);
                newStudent.Course = course;

                newStudent = cmsRepository.AddStudent(newStudent);
                var result = mapper.Map<StudentDto>(newStudent);

                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}