using SingleStage.Entities;
using SingleStage.Infrastructure;

namespace SingleStage.ViewModels.EditorViewModels
{
    public class PerformanceEditorViewModel : ViewModelBase
    {
        private Performance? _workingCopyPerformance;
        public Performance? WorkingCopyPerformance
        {
            get => _workingCopyPerformance;
            set
            {
                if (_workingCopyPerformance == value)
                    return;

                _workingCopyPerformance = value;

                OnPropertyChanged(nameof(WorkingCopyPerformance));

                // sync textual/date fields from working copy to this view model
                if (_workingCopyPerformance is not null)
                {
                    StartDate = _workingCopyPerformance.StartTime.Date;
                    StartTimeText = _workingCopyPerformance.StartTime.ToString("HH:mm");
                    EndDate = _workingCopyPerformance.EndTime.Date;
                    EndTimeText = _workingCopyPerformance.EndTime.ToString("HH:mm");
                }
                else
                {
                    StartDate = null;
                    StartTimeText = string.Empty;
                    EndDate = null;
                    EndTimeText = string.Empty;
                }

                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(ShowId));
                Validate();
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public bool IsEditing => WorkingCopyPerformance is not null;

        public string Description
        {
            get => WorkingCopyPerformance?.Description ?? string.Empty;
            set
            {
                if (WorkingCopyPerformance is null) 
                    return;

                if (WorkingCopyPerformance.Description == value) 
                    return;

                WorkingCopyPerformance.Description = value;

                OnPropertyChanged(nameof(Description));
                Validate();
            }
        }

        // Date parts (DatePicker bound here)
        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value)
                    return;

                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                UpdateWorkingCopyDateTimes();
                Validate();
            }
        }

        // textual time inputs (user types time like "19:00" or "7:30 PM")
        private string _startTimeText = string.Empty;
        public string StartTimeText
        {
            get => _startTimeText;
            set
            {
                if (_startTimeText == value)
                    return;

                _startTimeText = value;
                OnPropertyChanged(nameof(StartTimeText));
                UpdateWorkingCopyDateTimes();
                Validate();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate == value)
                    return;

                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                UpdateWorkingCopyDateTimes();
                Validate();
            }
        }

        private string _endTimeText = string.Empty;
        public string EndTimeText
        {
            get => _endTimeText;
            set
            {
                if (_endTimeText == value)
                    return;

                _endTimeText = value;
                OnPropertyChanged(nameof(EndTimeText));
                UpdateWorkingCopyDateTimes();
                Validate();
            }
        }

        // read-only accessors reflecting the combined DateTime values (updated by Validate when valid)
        public DateTime? StartTime => WorkingCopyPerformance?.StartTime;
        public DateTime? EndTime => WorkingCopyPerformance?.EndTime;

        public int ShowId
        {
            get => WorkingCopyPerformance?.ShowId ?? 0;
            set
            {
                if (WorkingCopyPerformance is null) 
                    return;

                if (WorkingCopyPerformance.ShowId == value) 
                    return;

                WorkingCopyPerformance.ShowId = value;
                OnPropertyChanged(nameof(ShowId));
                Validate();
            }
        }

        // validation state & message
        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                if (_errorMessage == value)
                    return;

                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsValid { get; private set; } = false;

        // Try to parse date + textual times, set WorkingCopyPerformance.StartTime/EndTime when valid
        private void Validate()
        {
            ErrorMessage = string.Empty;
            IsValid = false;

            if (WorkingCopyPerformance is null)
            {
                RaiseValidityChanged();
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkingCopyPerformance.Description))
            {
                ErrorMessage = "Description is required.";
                RaiseValidityChanged();
                return;
            }

            if (ShowId == 0)
            {
                ErrorMessage = "A show must be selected, since a performance is always part of a show.";
                RaiseValidityChanged();
                return;
            }

            if (StartDate is null)
            {
                ErrorMessage = "Start date is required.";
                RaiseValidityChanged();
                return;
            }

            if (EndDate is null)
            {
                ErrorMessage = "End date is required.";
                RaiseValidityChanged();
                return;
            }

            if (!DateTimeHelper.TryCombineDateAndTime(StartDate, StartTimeText, out DateTime combinedStart))
            {
                ErrorMessage = "Invalid start time format.";
                RaiseValidityChanged();
                return;
            }

            if (!DateTimeHelper.TryCombineDateAndTime(EndDate, EndTimeText, out DateTime combinedEnd))
            {
                ErrorMessage = "Invalid end time format.";
                RaiseValidityChanged();
                return;
            }

            if (!(combinedStart < combinedEnd))
            {
                ErrorMessage = "Start must be before End.";
                RaiseValidityChanged();
                return;
            }

            // valid: write back to working copy
            WorkingCopyPerformance.StartTime = combinedStart;
            WorkingCopyPerformance.EndTime = combinedEnd;

            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));

            ErrorMessage = string.Empty;
            IsValid = true;
            RaiseValidityChanged();
        }
        
        private void UpdateWorkingCopyDateTimes()
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
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        private void RaiseValidityChanged()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        public void BeginCreate()
        {
            // default: tonight 19:00 - next 21:00
            DateTime defaultStart = DateTime.Today.AddHours(19);
            DateTime defaultEnd = DateTime.Today.AddHours(21);

            WorkingCopyPerformance = new Performance
            {
                StartTime = defaultStart,
                EndTime = defaultEnd,
                Description = string.Empty,
                ShowId = 0
            };
        }

        public void BeginCreate(DateTime startTime, DateTime endTime, int showId)
        {
            WorkingCopyPerformance = new Performance
            {
                StartTime = startTime,
                EndTime = endTime,
                Description = string.Empty,
                ShowId = showId
            };
        }


        public void BeginEdit(Performance performance)
        {
            // shallow clone to avoid editing the original instance directly
            WorkingCopyPerformance = new Performance
            {
                Id = performance.Id,
                Description = performance.Description,
                StartTime = performance.StartTime,
                EndTime = performance.EndTime,
                ShowId = performance.ShowId
            };
        }

        public void Cancel()
        {
            WorkingCopyPerformance = null;
            ErrorMessage = string.Empty;
            IsValid = false;
        }
    }
}