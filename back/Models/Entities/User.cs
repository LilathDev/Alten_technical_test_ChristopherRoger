using System;
using System.ComponentModel.DataAnnotations;

namespace back.Models.Entities
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string Firstname { get; set; } = string.Empty;

        /// <summary>
        /// The username of the user, which must be unique.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The email address of the user. Must be unique and in a valid email format.
        /// </summary>
        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The hashed password of the user. Must be at least 6 characters long.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string PasswordHash { get; set; } = string.Empty;
    }
}