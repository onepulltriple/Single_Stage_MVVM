using SingleStage.Entities;

namespace SingleStage.ViewModels.EditorViewModels
{
    public class ArtistPerformanceEditorViewModel : ViewModelBase
    {
        private ArtistPerformance? _workingCopyArtistPerformance;

        public ArtistPerformance? WorkingCopyArtistPerformance
        {
            get => _workingCopyArtistPerformance;
            set
            {
                if (_workingCopyArtistPerformance == value)
                    return;

                _workingCopyArtistPerformance = value;

                OnPropertyChanged(nameof(WorkingCopyArtistPerformance));
                OnPropertyChanged(nameof(ArtistId));
                OnPropertyChanged(nameof(PerformanceId));
                OnPropertyChanged(nameof(RoyaltyUpFront));
                OnPropertyChanged(nameof(RoyaltyAtEnd));
                OnPropertyChanged(nameof(IsEditing));

                Validate();
            }
        }

        public bool IsEditing => WorkingCopyArtistPerformance is not null;

        public int ArtistId
        {
            get => WorkingCopyArtistPerformance?.ArtistId ?? 0;
            set
            {
                if (WorkingCopyArtistPerformance is null)
                    return;

                if (WorkingCopyArtistPerformance.ArtistId == value)
                    return;

                WorkingCopyArtistPerformance.ArtistId = value;

                OnPropertyChanged(nameof(ArtistId));
                Validate();
            }
        }

        public int PerformanceId
        {
            get => WorkingCopyArtistPerformance?.PerformanceId ?? 0;
            set
            {
                if (WorkingCopyArtistPerformance is null)
                    return;

                if (WorkingCopyArtistPerformance.PerformanceId == value)
                    return;

                WorkingCopyArtistPerformance.PerformanceId = value;

                OnPropertyChanged(nameof(PerformanceId));
                Validate();
            }
        }

        public decimal? RoyaltyUpFront
        {
            get => WorkingCopyArtistPerformance?.RoyaltyUpFront;
            set
            {
                if (WorkingCopyArtistPerformance is null)
                    return;

                if (WorkingCopyArtistPerformance.RoyaltyUpFront == value)
                    return;

                WorkingCopyArtistPerformance.RoyaltyUpFront = value;

                OnPropertyChanged(nameof(RoyaltyUpFront));
                Validate();
            }
        }

        public decimal? RoyaltyAtEnd
        {
            get => WorkingCopyArtistPerformance?.RoyaltyAtEnd;
            set
            {
                if (WorkingCopyArtistPerformance is null)
                    return;

                if (WorkingCopyArtistPerformance.RoyaltyAtEnd == value)
                    return;

                WorkingCopyArtistPerformance.RoyaltyAtEnd = value;

                OnPropertyChanged(nameof(RoyaltyAtEnd));
                Validate();
            }
        }

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

        private void Validate()
        {
            ErrorMessage = string.Empty;
            IsValid = false;

            if (WorkingCopyArtistPerformance is null)
            {
                RaiseValidityChanged();
                return;
            }

            if (ArtistId == 0)
            {
                ErrorMessage = "An artist must be selected.";
                RaiseValidityChanged();
                return;
            }

            if (PerformanceId == 0)
            {
                ErrorMessage = "A performance must be selected.";
                RaiseValidityChanged();
                return;
            }

            if (!RoyaltyUpFront.HasValue)
            {
                ErrorMessage = "Up-front royalty must be entered.";
                RaiseValidityChanged();
                return;
            }

            if (RoyaltyUpFront.Value < 0)
            {
                ErrorMessage = "Up-front royalty cannot be negative.";
                RaiseValidityChanged();
                return;
            }

            if (!RoyaltyAtEnd.HasValue)
            {
                ErrorMessage = "End royalty must be entered.";
                RaiseValidityChanged();
                return;
            }

            if (RoyaltyAtEnd.Value < 0)
            {
                ErrorMessage = "End royalty cannot be negative.";
                RaiseValidityChanged();
                return;
            }

            IsValid = true;
            RaiseValidityChanged();
        }

        private void RaiseValidityChanged()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        public void BeginCreate()
        {
            this.WorkingCopyArtistPerformance = new ArtistPerformance
            {
                ArtistId = 0,
                PerformanceId = 0,
                RoyaltyUpFront = null,
                RoyaltyAtEnd = null
            };
        }

        public void BeginCreate(int performanceId)
        {
            this.WorkingCopyArtistPerformance = new ArtistPerformance
            {
                ArtistId = 0,
                PerformanceId = performanceId,
                RoyaltyUpFront = null,
                RoyaltyAtEnd = null
            };
        }

        public void BeginEdit(ArtistPerformance artistPerformance)
        {
            this.WorkingCopyArtistPerformance = new ArtistPerformance
            {
                Id = artistPerformance.Id,
                ArtistId = artistPerformance.ArtistId,
                PerformanceId = artistPerformance.PerformanceId,
                RoyaltyUpFront = artistPerformance.RoyaltyUpFront,
                RoyaltyAtEnd = artistPerformance.RoyaltyAtEnd
            };
        }

        public void Cancel()
        {
            this.WorkingCopyArtistPerformance = null;
            ErrorMessage = string.Empty;
            IsValid = false;
        }
    }
}
