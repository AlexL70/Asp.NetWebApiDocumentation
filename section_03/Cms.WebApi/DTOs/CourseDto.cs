using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cms.WebApi.DTOs
{
    /// <summary>
    /// Data transformation object for Course
    /// </summary>
    public class CourseDto
    {
        /// <summary>
        /// Auto-generated unique identity
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// The official name of the course 
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string CourseName { get; set; }

        /// <summary>
        /// How long does in take in years
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int CourseDuration { get; set; }

        /// <summary>
        /// Area this course belongs to (multi-choice) 
        /// </summary>
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public COURSE_TYPE CourseType { get; set; }
    }

    public enum COURSE_TYPE
    {
        ENGINEERING,
        MEDICAL,
        MANAGEMENT
    }
}