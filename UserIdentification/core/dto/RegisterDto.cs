﻿namespace UserIdentification.core.dto
{
    public class RegisterDto
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Type { get; set; }
    }
}