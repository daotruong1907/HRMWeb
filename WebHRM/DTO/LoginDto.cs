﻿namespace WebHRM.DTO
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class ResponseLogin
    {
        public string Name { get; set; }
        public bool IsSuccess { get; set; }
    }
}
