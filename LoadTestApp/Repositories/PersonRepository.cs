using LoadTestApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;

namespace LoadTestApp.Repositories
{
    public class PersonRepository
    {
        private readonly SqlHelper _sqlHelper;

        public PersonRepository()
        {
            _sqlHelper = new SqlHelper("DefaultConnection");
        }

        public List<Person> GetPeople() {

            //Thread.Sleep(2000);

            var startTicks = DateTime.Now.Ticks;

            try
            {
                var people = new List<Person>();

                var data = _sqlHelper.RunScriptReturnDt("select personid, firstname, lastname from person");

                foreach (DataRow row in data.Rows)
                {
                    var person = new Person();
                    person.PersonID = Convert.ToInt32(row["PersonID"]);
                    person.FirstName = Convert.ToString(row["FirstName"]);
                    person.LastName = Convert.ToString(row["LastName"]);
                    people.Add(person);
                }

                return people;
            }
            catch (Exception)
            {
                LoadTestApp.PerfCounters.PerformanceCounterLocator.Instance.PersonRepositoryError.RecordOperation();
                throw;
            }
            finally {
                LoadTestApp.PerfCounters.PerformanceCounterLocator.Instance.GetPeople.RecordOperation(DateTime.Now.Ticks - startTicks);
            }

        }


    }
}