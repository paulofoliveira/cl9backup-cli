namespace Cl9Backup.CLI.FakeApi.Models
{
    public class UserProfileDto
    {
        public Dictionary<Guid, DestinationDto> Destinations { get; set; } = new Dictionary<Guid, DestinationDto>();
        public Dictionary<Guid, SourceDto> Sources { get; set; } = new Dictionary<Guid, SourceDto>();
        public Dictionary<Guid, DeviceDto> Devices { get; set; } = new Dictionary<Guid, DeviceDto>();
    }
}
