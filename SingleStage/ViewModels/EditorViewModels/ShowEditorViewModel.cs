using System;
using SingleStage.Entities;

namespace SingleStage.ViewModels.EditorViewModels
{
    // manages/represents the show currently being edited, i.e. owns the working copy
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

                // working copy changed, so notify the properties that expose its values, i.e. properties derived from Show
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(TicketPrice));
                OnPropertyChanged(nameof(SoldOut));

                // keep the textual time fields in sync with the working copy
                if (_workingCopyShow is not null)
                {
                    StartTimeText = _workingCopyShow.StartTime.ToString("HH:mm");
                    EndTimeText = _workingCopyShow.EndTime.ToString("HH:mm");
                }
                else
                {
                    StartTimeText = string.Empty;
                    EndTimeText = string.Empty;
                }

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

        // use nullable DateTime for easy binding with DatePicker controls
        public DateTime? StartTime
        {
            get => WorkingCopyShow?.StartTime;
            set
            {
                if (WorkingCopyShow is null || value is null)
                    return;

                if (WorkingCopyShow.StartTime == value.Value)
                    return;

                WorkingCopyShow.StartTime = value.Value;
                OnPropertyChanged(nameof(StartTime));
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
                Validate();
            }
        }

        public DateTime? EndTime
        {
            get => WorkingCopyShow?.EndTime;
            set
            {
                if (WorkingCopyShow is null || value is null)
                    return;

                if (WorkingCopyShow.EndTime == value.Value)
                    return;

                WorkingCopyShow.EndTime = value.Value;
                OnPropertyChanged(nameof(EndTime));
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
                Validate();
            }
        }

        // read-only accessors reflecting the combined DateTime values
        public DateTime? DisplayedStartTime => WorkingCopyShow?.StartTime;
        public DateTime? DisplayedEndTime => WorkingCopyShow?.EndTime;

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

        // Try to parse textual times, set WorkingCopyShow.StartTime/EndTime when valid
        private void Validate()
        {
            ErrorMessage = string.Empty;
            IsValid = false;

            if (WorkingCopyShow is null)
            {
                // nothing to validate
                RaiseValidityChanged();
                return;
            }

            // Basic name check
            if (string.IsNullOrWhiteSpace(WorkingCopyShow.Name))
            {
                ErrorMessage = "Name is required.";
                RaiseValidityChanged();
                return;
            }

            // Parse start time
            if (!TryParseTime(StartTimeText, WorkingCopyShow.StartTime.Date, out DateTime parsedStart))
            {
                ErrorMessage = "Invalid start time format.";
                RaiseValidityChanged();
                return;
            }

            // Parse end time
            if (!TryParseTime(EndTimeText, WorkingCopyShow.EndTime.Date, out DateTime parsedEnd))
            {
                ErrorMessage = "Invalid end time format.";
                RaiseValidityChanged();
                return;
            }

            // Ensure start is before end
            if (!(parsedStart < parsedEnd))
            {
                ErrorMessage = "Start time must be before end time.";
                RaiseValidityChanged();
                return;
            }

            // All good: apply parsed values to working copy and clear error
            WorkingCopyShow.StartTime = parsedStart;
            WorkingCopyShow.EndTime = parsedEnd;

            // notify read-only DateTime properties changed
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));

            ErrorMessage = string.Empty;
            IsValid = true;
            RaiseValidityChanged();
        }

        private bool TryParseTime(string timeText, DateTime baseDate, out DateTime result)
        {
            result = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(timeText))
                return false;

            // Try parse user input as a time or date time
            if (!DateTime.TryParse(timeText, out DateTime parsed))
                return false;

            // Use base date with parsed time part
            result = baseDate.Date + parsed.TimeOfDay;
            return true;
        }

        private void RaiseValidityChanged()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        public void BeginCreate()
        {
            // Default show spans tonight 19:00-21:00
            DateTime defaultStart = DateTime.Today.AddHours(19);
            DateTime defaultEnd = DateTime.Today.AddHours(21);

            this.WorkingCopyShow = new Show
            {
                Name = string.Empty,
                StartTime = defaultStart,
                EndTime = defaultEnd,
                TicketPrice = 0m,
                SoldOut = false
            };

            // StartTimeText/EndTimeText are set by WorkingCopyShow setter (and Validate called)
        }

        public void BeginEdit(Show show)
        {
            this.WorkingCopyShow = new Show
            {
                Id = show.Id,
                Name = show.Name,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TicketPrice = show.TicketPrice,
                SoldOut = show.SoldOut
                // related collections intentionally not copied
            };

            // StartTimeText/EndTimeText are set by WorkingCopyShow setter (and Validate called)
        }

        public void Cancel()
        {
            this.WorkingCopyShow = null;
            ErrorMessage = string.Empty;
            IsValid = false;
        }
    }
}
