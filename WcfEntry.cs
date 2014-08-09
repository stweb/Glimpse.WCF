using System;
using System.Diagnostics;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Wcf
{
    public class WcfEntry : ITimelineMessage
    {
        private IExecutionTimer _timer;
        private Stopwatch _stopWatch;

        public WcfEntry()
        {
            Id = Guid.NewGuid();
            _stopWatch = Stopwatch.StartNew();
            StartTime = DateTime.Now;
        }

        #region ITimelineMessage

        public Guid Id { get; private set; }

        public TimeSpan Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory
        {
            get { return new TimelineCategoryItem("WCF", "#F00000", "#FF0000"); }
            set { }
        }

        public string EventSubText { get; set; }

        #endregion

        // WCF Message data
        public string MessageId;
        public string RequestBody;
        public string ResponseBody;

        public IExecutionTimer Timer
        {
            set
            {
                _timer = value;
                var point = _timer.Point();
                Duration = point.Duration;
                Offset = point.Offset;
                StartTime = point.StartTime;
            }
        }

        public void CalcDuration()
        {
            if (_timer == null)
                return;

            if (_stopWatch == null)
            {
                _stopWatch = Stopwatch.StartNew();
                Duration = TimeSpan.FromMilliseconds(0);
            }
            else if (DateTime.Now - _stopWatch.Elapsed < _timer.RequestStart)
            {
                _stopWatch = Stopwatch.StartNew();
                Duration = TimeSpan.FromMilliseconds(0);
            }
            else
            {
                Duration = _stopWatch.Elapsed;
                _stopWatch = Stopwatch.StartNew();
            }
        }

        public void Cleanup()
        {
            _timer = null;
            _stopWatch = null;
        }
    }
}