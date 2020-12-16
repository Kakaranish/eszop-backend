using System;
using System.ComponentModel.DataAnnotations;
using Common.Types;

namespace Identity.API.Domain
{
    public class User : EntityBase, IAggregateRoot
    {
        public string Email { get; private set; }
        public HashedPassword HashedPassword { get; private set; }
        public Role Role { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected User()
        {
        }

        public User(string email, HashedPassword hashedPassword, Role role, string firstName, string lastName)
        {
            ValidateEmail(email);
            Email = email;

            HashedPassword = hashedPassword ?? throw new DomainException($"'{nameof(hashedPassword)}' cannot be null");

            Role = role ?? throw new DomainException($"'{nameof(hashedPassword)}' cannot be null");

            ValidateFirstName(firstName);
            FirstName = firstName;

            ValidateLastName(lastName);
            LastName = lastName;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(HashedPassword newPassword)
        {
            HashedPassword = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
        }

        private void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Trim().Length < 3)
            {
                throw new DomainException($"'{nameof(firstName)}' must have at least 3 characters");
            }
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Trim().Length < 3)
            {
                throw new DomainException($"'{nameof(lastName)}' must have at least 3 characters");
            }
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            {
                throw new DomainException("Invalid email");
            }
        }
    }
}
