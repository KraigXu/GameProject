using System;

    public class WorldTimerNode
    {
        public int Id;
        public bool IsActive;

        public bool IsStart;
        public DateTime StartTime;
        public Action StartCallback;

        public DateTime EndTime;
        public Action EndCallback;

        public WorldTimerNode(int id, DateTime startTime, Action startCallback, DateTime endTime, Action endcallback)
        {
            this.Id = id;
            this.StartTime = startTime;
            this.StartCallback = startCallback;
            this.EndTime = endTime;
            this.EndCallback = endcallback;
            this.IsActive = true;
        }

        public void Tick(DateTime time)
        {
            if (time >= StartTime && IsStart == false)
            {
                StartCallback.Invoke();
                IsStart = true;
            }

            if (time >= EndTime)
            {
                EndCallback.Invoke();
                IsActive = false;
                SystemManager.Get<WorldTimeSystem>().RemoveTimer(Id);
            }
        }
    }

