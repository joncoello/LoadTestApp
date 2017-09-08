using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTestApp.PerfCounters
{
    public class PerformanceMonitorOperation
    {
        private object m_SyncRoot = new object();

        public PerformanceMonitorOperation(
            string categoryName, string operationName)
        {
            if (String.IsNullOrEmpty(categoryName))
                throw new ArgumentException("categoryName is null or empty.", "categoryName");
            if (String.IsNullOrEmpty(operationName))
                throw new ArgumentException("operationName is null or empty.", "operationName");

            m_CategoryName = categoryName;
            m_OperationName = operationName;
        }

        /// <summary>
        /// Use this method to record the duration for an operation.
        /// </summary>
        /// <param name="duration">Number of ticks for the operation.</param>
        public void RecordOperation(long duration)
        {
            CheckIsInitializedForRuntime();

            OperationCount.Increment();
            OperationsPerSecond.Increment();
            AverageOperationTime.IncrementBy(duration);
            AverageOperationTimeBase.Increment();
        }

        protected const string CounterName_AverageOperationTime = "{0}: Average Operation Time";
        protected const string CounterName_AverageOperationTimeBase = "{0}: Average Operation Time Base";
        protected const string CounterName_OperationCount = "{0}: Operation Count";
        protected const string CounterName_OperationsPerSecond = "{0}: Operations Per Second";

        private bool m_AreCountersInitializedForRuntime = false;

        protected void InitializeCountersForRuntime()
        {
            m_AverageOperationTime =
                GetPerformanceCounterInstance(CounterName_AverageOperationTime);

            m_AverageOperationTimeBase = GetPerformanceCounterInstance(
                CounterName_AverageOperationTimeBase);

            m_OperationCount = GetPerformanceCounterInstance(
                CounterName_OperationCount);

            m_OperationsPerSecond = GetPerformanceCounterInstance(
                CounterName_OperationsPerSecond);

            OnInitializeCountersForRuntime();

            m_AreCountersInitializedForRuntime = true;
        }

        private string m_CategoryName;
        public string CategoryName
        {
            get { return m_CategoryName; }
        }

        private string m_OperationName;
        public string OperationName
        {
            get { return m_OperationName; }
        }

        private PerformanceCounter m_OperationCount;
        protected PerformanceCounter OperationCount
        {
            get { return m_OperationCount; }
        }

        private PerformanceCounter m_OperationsPerSecond;
        protected PerformanceCounter OperationsPerSecond
        {
            get { return m_OperationsPerSecond; }
        }

        private PerformanceCounter m_AverageOperationTime;
        protected PerformanceCounter AverageOperationTime
        {
            get { return m_AverageOperationTime; }
        }

        private PerformanceCounter m_AverageOperationTimeBase;
        protected PerformanceCounter AverageOperationTimeBase
        {
            get { return m_AverageOperationTimeBase; }
        }

        public void RegisterCountersForCreation(CounterCreationDataCollection countersToCreate)
        {
            countersToCreate.Add(CreateCounter(
                String.Format(CounterName_OperationCount, OperationName),
                PerformanceCounterType.NumberOfItems64));

            countersToCreate.Add(CreateCounter(
                String.Format(CounterName_OperationsPerSecond, OperationName),
                PerformanceCounterType.RateOfCountsPerSecond64));

            countersToCreate.Add(CreateCounter(
                String.Format(CounterName_AverageOperationTime, OperationName),
                PerformanceCounterType.AverageTimer32));

            countersToCreate.Add(CreateCounter(
                String.Format(CounterName_AverageOperationTimeBase, OperationName),
                PerformanceCounterType.AverageBase));

            BeforeCreateCounters(countersToCreate);
        }

        protected virtual void BeforeCreateCounters(CounterCreationDataCollection countersToCreate)
        {
            // override this method if you want to create other counters for this category
        }

        private CounterCreationData CreateCounter(
            string counterName,
            PerformanceCounterType counterType)
        {
            return new CounterCreationData(
                counterName, counterName, counterType);
        }

        public void Delete()
        {
            PerformanceCounterCategory.Delete(CategoryName);
        }

        protected virtual void OnInitializeCountersForRuntime()
        {
            // override this method if have created other performance counters that need to 
            // be initialized for runtime 
        }

        private PerformanceCounter GetPerformanceCounterInstance(string counterNameTemplate)
        {
            string counterName = String.Format(counterNameTemplate, OperationName);

            Console.WriteLine("Created counter instance for '{0}'.", counterName);

            return new PerformanceCounter(
                CategoryName, counterName, false);
        }

        private void CheckIsInitializedForRuntime()
        {
            if (m_AreCountersInitializedForRuntime == false)
            {
                lock (m_SyncRoot)
                {
                    if (m_AreCountersInitializedForRuntime == false)
                    {
                        Console.WriteLine("Initializing counter instances for operation '{0}'.", OperationName);
                        InitializeCountersForRuntime();
                    }
                }
            }
        }

        /// <summary>
        /// Record an operation without a duration.  Use this method to
        /// record an exception.
        /// </summary>
        public void RecordOperation()
        {
            RecordOperation(0);
        }
    }
}
