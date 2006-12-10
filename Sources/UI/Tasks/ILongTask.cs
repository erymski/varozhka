using System.ComponentModel;

namespace Varozhka.UI.Tasks
{
    /// <summary>
    /// A long processing.
    /// </summary>
    internal interface ILongTask
    {
        /// <summary>
        /// Status message to show during processing
        /// </summary>
        string ProcessingStatusMessage { get; }

        /// <summary>
        /// If the task must be run from the begin to the end.
        /// App will exit if it's false.
        /// </summary>
        bool CanBeStopped { get; }
        
        /// <summary>
        /// Message to show on stopped processing.
        /// </summary>
        string ProcessingStoppedMessage { get; }

        /// <summary>
        /// Run the processing.
        /// </summary>
        void Run(BackgroundWorker worker, DoWorkEventArgs e);
    }
}