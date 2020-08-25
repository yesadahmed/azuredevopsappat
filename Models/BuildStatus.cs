using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace azuredevopsappat.Models
{
    public class BuildStatusModel
    {
        public string status { get; set; }

        public string BuildResult { get; set; }

        public DateTime? QueueTime { get; set; }
        //
        // Summary:
        //     The time that the build was started.
        [DataMember(EmitDefaultValue = false)]
        public DateTime? StartTime { get; set; }
        //
        // Summary:
        //     The time that the build was completed.
        [DataMember(EmitDefaultValue = false)]
        public DateTime? FinishTime { get; set; }


    }
    public enum BuildResult
    {
        //
        // Summary:
        //     No result
        None = 0,
        //
        // Summary:
        //     The build completed successfully.
        Succeeded = 2,
        //
        // Summary:
        //     The build completed compilation successfully but had other errors.
        PartiallySucceeded = 4,
        //
        // Summary:
        //     The build completed unsuccessfully.
        Failed = 8,
        //
        // Summary:
        //     The build was canceled before starting.
        Canceled = 32
    }
    public enum BuildStatus
    {
        //
        // Summary:
        //     No status.
        None = 0,
        //
        // Summary:
        //     The build is currently in progress.
        InProgress = 1,
        //
        // Summary:
        //     The build has completed.
        Completed = 2,
        //
        // Summary:
        //     The build is cancelling
        Cancelling = 4,
        //
        // Summary:
        //     The build is inactive in the queue.
        Postponed = 8,
        //
        // Summary:
        //     The build has not yet started.
        NotStarted = 32,
        //
        // Summary:
        //     All status.
        All = 47
    }
}
