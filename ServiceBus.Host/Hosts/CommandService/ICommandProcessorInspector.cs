namespace ServiceBus.Hosts.CommandService
{
    using System;
    using Contracts;

    /// <summary>
    /// The ICommandProcessorInspector interface, 
    /// used to perform custom actions before and after a command is processed.
    /// </summary>
    public interface ICommandProcessorInspector
    {
        /// <summary>
        /// Called before a Command is processed.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        void BeforeProcessing(Command command);

        /// <summary>
        /// Called after a Command is processed and no exception has occurred.
        /// </summary>
        /// <param name="command">
        /// The command that was processed.
        /// </param>
        void AfterProcessing(Command command);

        /// <summary>
        /// Called when a process fails.
        /// </summary>
        /// <param name="command">
        /// The command that failed to be processed.
        /// </param>
        /// <param name="exception">
        /// The exception that occurred while the specified command tried to be processed.
        /// </param>
        void FailedProcessing(Command command, Exception exception);
    }
}