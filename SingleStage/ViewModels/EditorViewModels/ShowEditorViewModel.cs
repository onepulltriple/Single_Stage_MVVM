using SingleStage.Entities;
using SingleStage.Infrastructure;

namespace SingleStage.ViewModels.EditorViewModels
{
    // manages/represents the show currently being edited, i.e. owns the working copy
    // checks validity for the working copy only (internal validity)
    public class ShowEditorViewModel : ViewModelBase
    {
        private Show? _workingCopyShow;
        public Show? WorkingCopyShow
        {
            get => _workingCopyShow;
            set
            {
                if (_workingCopyShow == value)
                    return;

                _workingCopyShow = value;

                OnPropertyChanged(nameof(WorkingCopyShow));

                // sync textual/date fields from working copy to this view model
                if (_workingCopyShow is not null)
                {
                    StartDate = _workingCopyShow.StartTime.Date;
                    StartTimeText = _workingCopyShow.StartTime.ToString("HH:mm");
                    EndDate = _workingCopyShow.EndTime.Date;
                    EndTimeText = _workingCopyShow.EndTime.ToString("HH:mm");
                }
                else
                {
                    StartDate = null;
                    StartTimeText = string.Empty;
                    EndDate = null;
                    EndTimeText = string.Empty;
                }

                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(TicketPrice));
                OnPropertyChanged(nameof(SoldOut));
                Validate();
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public bool IsEditing => WorkingCopyShow is not null;

        public string Name
        {
            get => WorkingCopyShow?.Name ?? string.Empty;
            set
            {
                if (WorkingCopyShow is null)
                    return;

                if (WorkingCopyShow.Name == value)
                    return;

                WorkingCopyShow.Name = value;
                OnPropertyChanged(nameof(Name));
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
        public DateTime? StartTime => WorkingCopyShow?.StartTime;
        public DateTime? EndTime => WorkingCopyShow?.EndTime;

        public decimal? TicketPrice
        {
            get => WorkingCopyShow?.TicketPrice;
            set
            {
                if (WorkingCopyShow is null)
                    return;

                if (WorkingCopyShow.TicketPrice == value)
                    return;

                WorkingCopyShow.TicketPrice = value;
                OnPropertyChanged(nameof(TicketPrice));
            }
        }

        public bool SoldOut
        {
            get => WorkingCopyShow?.SoldOut ?? false;
            set
            {
                if (WorkingCopyShow is null)
                    return;

                if (WorkingCopyShow.SoldOut == value)
                    return;

                WorkingCopyShow.SoldOut = value;
                OnPropertyChanged(nameof(SoldOut));
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

        // Try to parse date + textual times, set WorkingCopyShow.StartTime/EndTime when valid
        private void Validate()
        {
            ErrorMessage = string.Empty;
            IsValid = false;

            if (WorkingCopyShow is null)
            {
                RaiseValidityChanged();
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkingCopyShow.Name))
            {
                ErrorMessage = "Name is required.";
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
                ErrorMessage = "Start must be before end. Enter times in 24-hour format.";
                RaiseValidityChanged();
                return;
            }

            // valid: write back to working copy
            WorkingCopyShow.StartTime = combinedStart;
            WorkingCopyShow.EndTime = combinedEnd;

            var scheduleValidator = new ShowScheduleValidator();
            if (!scheduleValidator.IsWithinOpeningHours(WorkingCopyShow))
            {
                ErrorMessage = "Show must be entirely within opening hours (10:00 - 24:00).";
                RaiseValidityChanged();
                return;
            }

            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));

            ErrorMessage = string.Empty;
            IsValid = true;
            RaiseValidityChanged();
        }

        private void UpdateWorkingCopyDateTimes()
        {
            if (WorkingCopyShow is null)
                return;

            if (DateTimeHelper.TryCombineDateAndTime(_startDate, _startTimeText, out DateTime start))
            {
                WorkingCopyShow.StartTime = start;
            }

            if (DateTimeHelper.TryCombineDateAndTime(_endDate, _endTimeText, out DateTime end))
            {
                WorkingCopyShow.EndTime = end;
            }

            OnPropertyChanged(nameof(WorkingCopyShow));
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

            WorkingCopyShow = new Show
            {
                StartTime = defaultStart,
                EndTime = defaultEnd,
                TicketPrice = 0m,
                SoldOut = false,
                Name = string.Empty
            };
        }

        // shallow clone to avoid editing the original instance directly
        public void BeginEdit(Show show)
        {
            WorkingCopyShow = new Show
            {
                Id = show.Id,
                Name = show.Name,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TicketPrice = show.TicketPrice,
                SoldOut = show.SoldOut
            };
        }

        public void Cancel()
        {
            WorkingCopyShow = null;
            ErrorMessage = string.Empty;
            IsValid = false;
        }
    }
}