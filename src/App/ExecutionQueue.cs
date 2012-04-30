using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Collections.Generic;

namespace App
{
    public static class ExecutionQueue
    {
        public enum Policy
        {
            Immediate,
            Queued
        }

        private static int period = 10;
        private static Timer timer = new Timer(new TimerCallback(Dequeue),
                null,
                TimeSpan.FromSeconds(period),
                TimeSpan.FromSeconds(period));
        private static List<WaitCallback> queue = new List<WaitCallback>();
        
        /// <summary>
        /// Delay between invoking WaitCallbacks passed to Enqueue when the
        /// policy is Queued, measured in seconds.
        /// </summary>
        public static int Period
        {
            get { return period; }
            set { period = value; }
        }
            
        public static void Dequeue(object stateInfo)
        {
            if (queue.Count != 0)
            {
                ThreadPool.QueueUserWorkItem(queue[0]);
                lock (queue)
                {
                    queue.RemoveAt(0);
                }
            }
        }

        public static void Enqueue(WaitCallback callback, Policy policy)
        {
            switch(policy)
            {
                case Policy.Immediate:
                    ThreadPool.QueueUserWorkItem(new WaitCallback(callback));
                    break;
                case Policy.Queued:
                    lock (queue)
                    {
                        queue.Add(callback);
                    }
                    break;
            }
        }

    }
}
