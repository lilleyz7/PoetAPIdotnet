﻿namespace PoetAPI.DTOs
{
    public class UserRegistrationDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmation {  get; set; } = string.Empty;
    }
}
