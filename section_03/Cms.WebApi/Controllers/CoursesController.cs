using System.Collections.Generic;
using System.Linq;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Add new course to the system
        /// </summary>
        /// <param name="course">Course name</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Gets one course by id 
        /// </summary>
        /// <param name="courseId">Compares to CourseId field in CourseDto</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Updates course by ID
        /// </summary>
        /// <param name="courseId">Used to find the course</param>
        /// <param name="course">New value of course name</param>
        [HttpPut("{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes course from the database
        /// </summary>
        /// <param name="courseId">Used to find course</param>
        [HttpDelete("{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Returns list of students taking the course
        /// </summary>
        /// <param name="courseId">Used to find the course in DB</param>
        [HttpGet("{courseId}/students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Adds new student to the course
        /// </summary>
        /// <param name="courseId">Used to find the course in DB</param>
        /// <param name="student">Student to add</param>
        [HttpPost("{courseId}/students")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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