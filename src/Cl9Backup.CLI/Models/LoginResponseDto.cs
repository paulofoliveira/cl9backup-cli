﻿namespace Cl9Backup.CLI.Models
{
    public class LoginResponseDto
    {
        public string Status { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string SessionKey { get; set; } = default!;
    }
}