namespace Cl9Backup.CLI.Shared
{
    public abstract class ResponseDto
    {
        public string Message { get; set; } = default!;
        public int Status { get; set; }
    }
}
