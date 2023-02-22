namespace Cl9Backup.CLI.Models
{
    public class RunBackupRequestOptionsDto
    {
        public bool SkipAlreadyRunning { get; set; } = true;
        public bool ReduceDiskConcurrency { get; set; } = false;
        public bool UseOnDiskIndexes { get; set; } = false;
        public bool AllowZeroFilesSuccess { get; set; } = false;
        public int StopAfter { get; set; } = 0;
        public int LimitVaultSpeedBps { get; set; } = 0;
    }
}
