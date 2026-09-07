using FluentAssertions;
using NUnit.Framework;
using SingleStage.Entities;
using SingleStage.Infrastructure;

namespace SingleStage.Tests.Services
{
    [TestFixture]
    public class ShowScheduleValidatorTests
    {
        private ShowScheduleValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new ShowScheduleValidator();
        }

        [Test]
        public void GetConflict_WhenShowsTouchButDoNotOverlap_ReturnsNull()
        {
            var existingShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 21, 0, 0)
            };

            var newShow = new Show
            {
                Id = 0,
                StartTime = new DateTime(2026, 9, 1, 21, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 23, 0, 0)
            };

            _validator.GetConflict(newShow, new[] { existingShow })
                .Should().BeNull();
        }

        [Test]
        public void GetConflict_WhenEditingSameShow_ReturnsNull()
        {
            var existingShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 22, 0, 0)
            };

            var editedShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 22, 0, 0)
            };

            _validator.GetConflict(editedShow, new[] { existingShow })
                .Should().BeNull();
        }

        [Test]
        public void GetConflict_WhenShowsAreSeparate_ReturnsNull()
        {
            var existingShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 21, 0, 0)
            };

            var newShow = new Show
            {
                Id = 0,
                StartTime = new DateTime(2026, 9, 1, 22, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 23, 0, 0)
            };

            _validator.GetConflict(newShow, new[] { existingShow })
                .Should().BeNull();
        }

        [Test]
        public void GetConflict_WhenNewShowCompletelySurroundsExistingShow_ReturnsExistingShow()
        {
            var existingShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 20, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 22, 0, 0)
            };

            var newShow = new Show
            {
                Id = 0,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 23, 0, 0)
            };

            _validator.GetConflict(newShow, new[] { existingShow })
                .Should().BeSameAs(existingShow);
        }

        [Test]
        public void GetConflict_WhenExistingShowCompletelySurroundsNewShow_ReturnsExistingShow()
        {
            var existingShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 23, 0, 0)
            };

            var newShow = new Show
            {
                Id = 0,
                StartTime = new DateTime(2026, 9, 1, 20, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 22, 0, 0)
            };

            _validator.GetConflict(newShow, new[] { existingShow })
                .Should().BeSameAs(existingShow);
        }

        [Test]
        public void GetConflict_WhenShowsHaveSameStartTime_ReturnsExistingShow()
        {
            var existingShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 22, 0, 0)
            };

            var newShow = new Show
            {
                Id = 0,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 20, 0, 0)
            };

            _validator.GetConflict(newShow, new[] { existingShow })
                .Should().BeSameAs(existingShow);
        }

        [Test]
        public void GetConflict_WhenShowsHaveSameEndTime_ReturnsExistingShow()
        {
            var existingShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 22, 0, 0)
            };

            var newShow = new Show
            {
                Id = 0,
                StartTime = new DateTime(2026, 9, 1, 20, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 22, 0, 0)
            };

            _validator.GetConflict(newShow, new[] { existingShow })
                .Should().BeSameAs(existingShow);
        }

        [Test]
        public void GetConflict_WhenShowsAreOnDifferentDates_ReturnsNull()
        {
            var existingShow = new Show
            {
                Id = 1,
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 22, 0, 0)
            };

            var newShow = new Show
            {
                Id = 0,
                StartTime = new DateTime(2026, 9, 2, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 2, 22, 0, 0)
            };

            _validator.GetConflict(newShow, new[] { existingShow })
                .Should().BeNull();
        }

        [Test]
        public void IsWithinOpeningHours_WhenShowStartsAtOpeningTime_ReturnsTrue()
        {
            var show = new Show
            {
                StartTime = new DateTime(2026, 9, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0)
            };

            _validator.IsWithinOpeningHours(show)
                .Should().BeTrue();
        }

        [Test]
        public void IsWithinOpeningHours_WhenShowStartsBeforeOpeningTime_ReturnsFalse()
        {
            var show = new Show
            {
                StartTime = new DateTime(2026, 9, 1, 9, 59, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0)
            };

            _validator.IsWithinOpeningHours(show)
                .Should().BeFalse();
        }

        [Test]
        public void IsWithinOpeningHours_WhenShowEndsAtMidnight_ReturnsTrue()
        {
            var show = new Show
            {
                StartTime = new DateTime(2026, 9, 1, 22, 0, 0),
                EndTime = new DateTime(2026, 9, 2, 0, 0, 0)
            };

            _validator.IsWithinOpeningHours(show)
                .Should().BeTrue();
        }

        [Test]
        public void IsWithinOpeningHours_WhenShowEndsAfterMidnight_ReturnsFalse()
        {
            var show = new Show
            {
                StartTime = new DateTime(2026, 9, 1, 22, 0, 0),
                EndTime = new DateTime(2026, 9, 2, 0, 1, 0)
            };

            _validator.IsWithinOpeningHours(show)
                .Should().BeFalse();
        }

        [Test]
        public void IsWithinOpeningHours_WhenShowStartsBeforeOpeningAndEndsAfterOpening_ReturnsFalse()
        {
            var show = new Show
            {
                StartTime = new DateTime(2026, 9, 1, 9, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 11, 0, 0)
            };

            _validator.IsWithinOpeningHours(show)
                .Should().BeFalse();
        }

        [Test]
        public void IsWithinOpeningHours_WhenShowIsWithinOpeningHours_ReturnsTrue()
        {
            var show = new Show
            {
                StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 23, 0, 0)
            };

            _validator.IsWithinOpeningHours(show)
                .Should().BeTrue();
        }
    }
}
