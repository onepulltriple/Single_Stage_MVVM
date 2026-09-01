using System.Globalization;
using SingleStage.Entities;
using SingleStage.Infrastructure;

namespace SingleStage.ViewModels.EditorViewModels
{
    public class PerformanceEditorViewModel : ViewModelBase
    {
        public Performance? WorkingCopyPerformance { get; private set; }

        private DateTime? _startDate;
        private string _startTimeText = string.Empty;
        private DateTime? _endDate;
        private string _endTimeText = string.Empty;

        public bool IsEditing => WorkingCopyPerformance is not null;

        public string Description
        {
            get => WorkingCopyPerformance?.Description ?? string.Empty;
            set
            {
                if (WorkingCopyPerformance is null) return;
                if (WorkingCopyPerformance.Description == value) return;
                WorkingCopyPerformance.Description = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                RecomputeTimes();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public string StartTimeText
        {
            get => _startTimeText;
            set
            {
                if (_startTimeText == value) return;
                _startTimeText = value;
                OnPropertyChanged(nameof(StartTimeText));
                RecomputeTimes();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                RecomputeTimes();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public string EndTimeText
        {
            get => _endTimeText;
            set
            {
                if (_endTimeText == value) return;
                _endTimeText = value;
                OnPropertyChanged(nameof(EndTimeText));
                RecomputeTimes();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public int ShowId
        {
            get => WorkingCopyPerformance?.ShowId ?? 0;
            set
            {
                if (WorkingCopyPerformance is null) return;
                if (WorkingCopyPerformance.ShowId == value) return;
                WorkingCopyPerformance.ShowId = value;
                OnPropertyChanged(nameof(ShowId));
            }
        }

        public string ErrorMessage { get; private set; } = string.Empty;

        public bool IsValid
        {
            get
            {
                if (WorkingCopyPerformance is null)
                    return false;

                if (string.IsNullOrWhiteSpace(WorkingCopyPerformance.Description))
                {
                    ErrorMessage = "Description is required.";
                    return false;
                }

                if (!TryGetCombinedDateTime(out DateTime s, out DateTime e))
                {
                    ErrorMessage = "Start/End date or time invalid.";
                    return false;
                }

                if (e <= s)
                {
                    ErrorMessage = "End must be after Start.";
                    return false;
                }

                ErrorMessage = string.Empty;
                return true;
            }
        }

        public void BeginCreate()
        {
            WorkingCopyPerformance = new Performance
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                Description = string.Empty,
                ShowId = 0
            };

            _startDate = WorkingCopyPerformance.StartTime.Date;
            _startTimeText = WorkingCopyPerformance.StartTime.ToString("HH:mm", CultureInfo.InvariantCulture);
            _endDate = WorkingCopyPerformance.EndTime.Date;
            _endTimeText = WorkingCopyPerformance.EndTime.ToString("HH:mm", CultureInfo.InvariantCulture);

            OnAllEditorPropertiesChanged();
        }

        public void BeginEdit(Performance source)
        {
            ArgumentNullException.ThrowIfNull(source);

            // shallow clone to avoid editing the original instance directly
            WorkingCopyPerformance = new Performance
            {
                Id = source.Id,
                Description = source.Description,
                StartTime = source.StartTime,
                EndTime = source.EndTime,
                ShowId = source.ShowId
            };

            _startDate = WorkingCopyPerformance.StartTime.Date;
            _startTimeText = WorkingCopyPerformance.StartTime.ToString("HH:mm", CultureInfo.InvariantCulture);
            _endDate = WorkingCopyPerformance.EndTime.Date;
            _endTimeText = WorkingCopyPerformance.EndTime.ToString("HH:mm", CultureInfo.InvariantCulture);

            OnAllEditorPropertiesChanged();
        }

        public void Cancel()
        {
            WorkingCopyPerformance = null;
            _startDate = null;
            _endDate = null;
            _startTimeText = string.Empty;
            _endTimeText = string.Empty;
            ErrorMessage = string.Empty;
            OnAllEditorPropertiesChanged();
        }

        private void OnAllEditorPropertiesChanged()
        {
            OnPropertyChanged(nameof(WorkingCopyPerformance));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(StartDate));
            OnPropertyChanged(nameof(StartTimeText));
            OnPropertyChanged(nameof(EndDate));
            OnPropertyChanged(nameof(EndTimeText));
            OnPropertyChanged(nameof(ShowId));
            OnPropertyChanged(nameof(IsEditing));
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        private void RecomputeTimes()
        {
            if (WorkingCopyPerformance is null)
                return;

            if (DateTimeHelper.TryCombineDateAndTime(_startDate, _startTimeText, out DateTime start))
            {
                WorkingCopyPerformance.StartTime = start;
            }

            if (DateTimeHelper.TryCombineDateAndTime(_endDate, _endTimeText, out DateTime end))
            {
                WorkingCopyPerformance.EndTime = end;
            }

            OnPropertyChanged(nameof(WorkingCopyPerformance));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        private bool TryGetCombinedDateTime(out DateTime start, out DateTime end)
        {
            start = default;
            end = default;

            if (WorkingCopyPerformance is null)
                return false;

            if (!DateTimeHelper.TryCombineDateAndTime(_startDate, _startTimeText, out start))
                return false;

            if (!DateTimeHelper.TryCombineDateAndTime(_endDate, _endTimeText, out end))
                return false;

            return true;
        }

    }
}