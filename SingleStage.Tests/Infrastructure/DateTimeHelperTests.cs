using FluentAssertions;
using SingleStage.Infrastructure;

namespace SingleStage.Tests.Infrastructure
{
    [TestFixture]
    public class DateTimeHelperTests
    {
        [Test]
        public void TryParseTime_With24HourTime_ReturnsTrue()
        {
            var result = DateTimeHelper.TryParseTime("19:30", out TimeSpan time);

            result.Should().BeTrue();
            time.Should().Be(new TimeSpan(19, 30, 0));
        }

        [Test]
        public void TryParseTime_With12HourTime_ReturnsTrue()
        {
            var result = DateTimeHelper.TryParseTime("7:30 PM", out TimeSpan time);

            result.Should().BeTrue();
            time.Should().Be(new TimeSpan(19, 30, 0));
        }

        [Test]
        public void TryParseTime_WithWhitespace_ReturnsTrue()
        {
            var result = DateTimeHelper.TryParseTime("  19:30  ", out TimeSpan time);

            result.Should().BeTrue();
            time.Should().Be(new TimeSpan(19, 30, 0));
        }

        [Test]
        public void TryParseTime_WithEmptyString_ReturnsFalse()
        {
            var result = DateTimeHelper.TryParseTime(string.Empty, out TimeSpan time);

            result.Should().BeFalse();
        }

        [Test]
        public void TryParseTime_WithWhitespaceOnly_ReturnsFalse()
        {
            var result = DateTimeHelper.TryParseTime("   ", out TimeSpan time);

            result.Should().BeFalse();
        }

        [Test]
        public void TryParseTime_WithInvalidText_ReturnsFalse()
        {
            var result = DateTimeHelper.TryParseTime("not a time", out TimeSpan time);

            result.Should().BeFalse();
        }

        [Test]
        public void TryCombineDateAndTime_WithValidDateAndTime_ReturnsTrue()
        {
            var date = new DateTime(2026, 9, 1);

            var result = DateTimeHelper.TryCombineDateAndTime(
                date,
                "19:30",
                out DateTime combined);

            result.Should().BeTrue();
            combined.Should().Be(new DateTime(2026, 9, 1, 19, 30, 0));
        }

        [Test]
        public void TryCombineDateAndTime_With12HourTime_ReturnsTrue()
        {
            var date = new DateTime(2026, 9, 1);

            var result = DateTimeHelper.TryCombineDateAndTime(
                date,
                "7:30 PM",
                out DateTime combined);

            result.Should().BeTrue();
            combined.Should().Be(new DateTime(2026, 9, 1, 19, 30, 0));
        }

        [Test]
        public void TryCombineDateAndTime_WithNullDate_ReturnsFalse()
        {
            var result = DateTimeHelper.TryCombineDateAndTime(
                null,
                "19:30",
                out DateTime combined);

            result.Should().BeFalse();
        }

        [Test]
        public void TryCombineDateAndTime_WithInvalidTime_ReturnsFalse()
        {
            var date = new DateTime(2026, 9, 1);

            var result = DateTimeHelper.TryCombineDateAndTime(
                date,
                "not a time",
                out DateTime combined);

            result.Should().BeFalse();
        }

        [Test]
        public void TryCombineDateAndTime_IgnoresDateTimePartOfDate()
        {
            var date = new DateTime(2026, 9, 1, 14, 45, 30);

            var result = DateTimeHelper.TryCombineDateAndTime(
                date,
                "19:30",
                out DateTime combined);

            result.Should().BeTrue();
            combined.Should().Be(new DateTime(2026, 9, 1, 19, 30, 0));
        }
    }
}
