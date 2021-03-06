﻿using System;
using System.Globalization;
using System.Linq;
using Aquarius.TimeSeries.Client.ServiceModels.Publish;

namespace SharpShooterReportsRunner
{
    public class DateTimeParser
    {
        public static DateTimeOffset? Parse(TimeSeriesDescription timeSeriesDescription, string timeText)
        {
            return Parse(timeText, () => timeSeriesDescription.UtcOffsetIsoDuration.ToTimeSpan());
        }

        public static DateTimeOffset? Parse(LocationDataServiceResponse location, string timeText)
        {
            return Parse(timeText, () => TimeSpan.FromHours(location.UtcOffset));
        }

        private static DateTimeOffset? Parse(string timeText, Func<TimeSpan> utcOffsetFunc)
        {
            if (string.IsNullOrWhiteSpace(timeText))
                return null;

            timeText = timeText.Trim();

            // TODO: Support water year

            if (DateTimeOffset.TryParseExact(timeText, SupportedDateFormatsWithZoneInfo, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeOffset))
                return dateTimeOffset;

            if (!DateTime.TryParseExact(timeText, SupportedDateFormatsWithoutZoneInfo, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dateTime))
                throw new ExpectedException($"'{timeText}' is not a supported DateTime value.\n\nSupported DateTime formats are:\n\n    {string.Join("    \n", SupportedDateFormatsWithoutZoneInfo.Concat(SupportedDateFormatsWithZoneInfo))}");

            var utcOffset = utcOffsetFunc();

            dateTimeOffset = new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified), utcOffset);

            return dateTimeOffset;
        }

        private static readonly string[] SupportedDateFormatsWithoutZoneInfo =
        {
            "yyyy",
            "yyyy-MM",
            "yyyy-MM-dd",
            "yyyy-MM-ddTHH:mm",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.fff",
        };

        private static readonly string[] SupportedDateFormatsWithZoneInfo =
        {
            "yyyy-MM-ddTHH:mm:ss.fffzzz",
            "yyyy-MM-ddTHH:mm:ss.fffffffzzz",
        };
    }
}
