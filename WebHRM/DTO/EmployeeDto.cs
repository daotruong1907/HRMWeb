namespace WebHRM.DTO
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string? Sex { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string Creator { get; set; }
        public DateTime CreateAt { get; set; }
        public string? Repairer { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}
