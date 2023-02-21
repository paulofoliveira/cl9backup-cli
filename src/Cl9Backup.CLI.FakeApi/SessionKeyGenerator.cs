namespace Cl9Backup.CLI.FakeApi
{
    public class SessionKeyGenerator
    {
        private List<SessionKeyDto> _sessionKeysInMemory = new List<SessionKeyDto>();

        public SessionKeyDto GenerateSessionKey()
        {
            var sessionKey = new SessionKeyDto() { SessionKey = Guid.NewGuid().ToString(), ValidDate = DateTime.Now.AddMinutes(5) };
            _sessionKeysInMemory.Add(sessionKey);
            return sessionKey;
        }

        public bool SessionKeyIsValid(string sessionKey) => _sessionKeysInMemory.Any(x => x.SessionKey == sessionKey && x.ValidDate >= DateTime.Now);
    }
    public class SessionKeyDto
    {
        public string SessionKey { get; set; } = default!;
        public DateTime ValidDate { get; set; }
    }
}
