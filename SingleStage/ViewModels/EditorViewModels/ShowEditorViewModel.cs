using SingleStage.Entities;

// manages/represents the show currently being edited, i.e. owns the working copy
namespace SingleStage.ViewModels.EditorViewModels
{
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
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(EndTime));
                OnPropertyChanged(nameof(TicketPrice));
                OnPropertyChanged(nameof(SoldOut));
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

        public void BeginCreate()
        {
            this.WorkingCopyShow = new Show
            {
                // sensible defaults: start today at 19:00 and 2 hour duration
                StartTime = DateTime.Today.AddHours(19),
                EndTime = DateTime.Today.AddHours(21),
                TicketPrice = 0m,
                SoldOut = false
            };
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
                // note: related collections (ShowAppearances, Tickets) intentionally not copied here.
            };
        }

        public void Cancel()
        {
            this.WorkingCopyShow = null;
        }
    }
}
