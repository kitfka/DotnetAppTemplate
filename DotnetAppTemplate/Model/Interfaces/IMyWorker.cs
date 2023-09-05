using System.ComponentModel;

namespace DotnetAppTemplate.Model.Interfaces
{

    /// <summary>
    /// This is an Interface we use for the wrapped Backgroundworker.
    /// Because of this we can use an Fake backgroundworker when writing tests!
    /// https://stackoverflow.com/questions/848179/designing-an-interface-for-backgroundworker
    /// </summary>
    public interface IMyWorker
    {
        bool WorkerReportsProgress { get; set; }
        bool WorkerSupportsCancellation { get; set; }
        event DoWorkEventHandler DoWork;
        event ProgressChangedEventHandler ProgressChanged;
        event RunWorkerCompletedEventHandler RunWorkerCompleted;

        void RunWorkerAsync();
        void RunWorkerAsync(object? argument);
        bool IsBusy { get; }
        void ReportProgress(int percentProgress);
        void CancelAsync();
        bool CancellationPending { get; }

#nullable enable
        void ReportProgress(int percentProgress, object? userState);
#nullable disable
    }
}