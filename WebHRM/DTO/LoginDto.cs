namespace WebHRM.DTO
{
    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class Data
    {
        public int Id { get; set; }

        public bool IsSuccess { get; set; } = true;

        public string? ResponseFromServerr { get; set; }

        public int Status { get; set; } = 1;
    }

    public class ResponseLoginClone
    {
        public int Id { get; set; }

        public int ResponseCode { get; set; } = 0;

        public string? ResponseFromServer { get; set; }
        public int Status { get; set; } = 1;
        public string? UserMessage { get; set; } = "";
        public string? InternalMessage { get; set; } = "";
        public string? MessageCode { get; set; } = null;
        
        public Data Data { get; set; }
    }
}
