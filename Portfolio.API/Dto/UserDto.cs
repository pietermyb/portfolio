using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The user name.
        /// </value>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        [Required]
        public string Surname { get; set; }
        
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        public string Password { get; set; }

    }
}
