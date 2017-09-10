using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTestApp.PerfCounters
{
    public class PerformanceCounterLocator
    {
        private static object m_SyncRoot = new object();

        private const string CategoryName = "Load Test App";

        private PerformanceCounterLocator()
        {
            GetPeople = new PerformanceMonitorOperation(
                CategoryName, "Get People");

            CreatePerson = new PerformanceMonitorOperation(
                CategoryName, "Create Person");

            PersonRepositoryError = new PerformanceMonitorOperation(
                CategoryName, "Person Repository Error");
        }

        public PerformanceMonitorOperation GetPeople { get; private set; }
        public PerformanceMonitorOperation CreatePerson { get; private set; }
        public PerformanceMonitorOperation PersonRepositoryError { get; private set; }

        private static PerformanceCounterLocator m_Instance;
        public static PerformanceCounterLocator Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (m_SyncRoot)
                    {
                        if (m_Instance == null)
                        {
                            m_Instance = new PerformanceCounterLocator();
                        }
                    }
                }

                return m_Instance;
            }
        }

        public void CreateCounters()
        {
            var countersToCreate = new CounterCreationDataCollection();

            GetPeople.RegisterCountersForCreation(countersToCreate);
            CreatePerson.RegisterCountersForCreation(countersToCreate);
            PersonRepositoryError.RegisterCountersForCreation(countersToCreate);

            PerformanceCounterCategory.Create(
                CategoryName,
                CategoryName,
                PerformanceCounterCategoryType.SingleInstance,
                countersToCreate);
        }

        public void DeleteCounters()
        {
            PerformanceCounterCategory.Delete(CategoryName);
        }
    }
}
