using System.ComponentModel.DataAnnotations;

namespace Cms.WebApi.DTOs
{
    /// <summary>
    /// Data transfer object used to pass Student
    /// </summary>
    public class StudentDto
    {
        /// <summary>
        /// Unique auto-generated key 
        /// </summary>
        public int StudentId { get; set; }


        /// <summary>
        /// First name
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name (surname) 
        /// </summary>
        [MaxLength(30)]
        public string LastName { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Full student's address
        /// </summary>
        [MaxLength(100)]
        public string Address { get; set; }
    }
}