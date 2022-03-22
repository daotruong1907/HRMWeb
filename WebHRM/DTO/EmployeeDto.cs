namespace WebHRM.DTO
{
    public class EmployeeDto
    {
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
    }

    public class EmployeeInformationDto
    {
        public EmployeeDto Employee { get; set; }
        public AddAccountDto Account { get; set; }
        public string ResponseFromServer { get;set; }
        public bool isSuccess { get; set; } = true;
    }

    public class ResponseUpdateEmployee
    {
        public UpdateEmployeeDto UpdateEmployeeDto { get; set; } = new UpdateEmployeeDto();
        public string ResponseFromServer { get; set; }
        public bool isSuccess { get; set; } = true;
    }

    public class UpdateEmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string? Sex { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Repairer { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class ListResponseSearchEmployee
    {
        public List<ResponseSearchEmployee> ResponseSearchEmployees { get; set; } = new List<ResponseSearchEmployee>();
        public string ResponseFromServer { get;set; }

    }
    public class ResponseSearchEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string? Sex { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }

    public class RequestSearchEmployee
    {
        public PageDto PageDto{get;set;}
        public ParamSearchEmployee ParamSearchEmployee { get;set;}
    }

    public class ParamSearchEmployee
    {
        public string? NameOrEmail { get; set; }
        public DateTime? FromBirthDay { get; set; }
        public DateTime? ToBirthDay { get; set; }
        public string? Sex { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
