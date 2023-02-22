namespace Cl9Backup.CLI.Shared
{
    public class ProfileResponseDto : ResponseDto
    {
        public Dictionary<string, DestinationDto> Destinations { get; set; } = new Dictionary<string, DestinationDto>();
        public Dictionary<string, SourceDto> Sources { get; set; } = new Dictionary<string, SourceDto>();
        public Dictionary<string, DeviceDto> Devices { get; set; } = new Dictionary<string, DeviceDto>();
    }
}
