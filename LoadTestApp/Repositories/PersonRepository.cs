using LoadTestApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        public List<Person> GetPeople()
        {

            //Thread.Sleep(2000);

            var startTicks = DateTime.Now.Ticks;

            try
            {
                var people = new List<Person>();

                //var data = _sqlHelper.RunScriptReturnDt("select personid, firstname, lastname from person");

                var script = "select personid, firstname, lastname from person";

                // connection
                var connectionString = @"Data Source=ACCESS-1303SF2\SQL2014;Initial Catalog=LoadTestApp;User ID=sa;PWD=Patrick@1;Max Pool Size=50";
                var connection = new SqlConnection(connectionString);
                connection.Open();

                // command
                var command = new SqlCommand(script, connection);

                // adapter
                var adapter = new SqlDataAdapter(command);

                var data = new DataTable();
                adapter.Fill(data);

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
            finally
            {
                LoadTestApp.PerfCounters.PerformanceCounterLocator.Instance.GetPeople.RecordOperation(DateTime.Now.Ticks - startTicks);
            }

        }

        internal void DeletePerson(int id)
        {
            _sqlHelper.RunScript("delete from person where personid = @PersonID",
               new SqlParameter("@PersonID", id)
               );
        }

        internal Person GetPerson(int id)
        {

            var data = _sqlHelper.RunScriptReturnDt("select personid, firstname, lastname from person where PersonID = @PersonID",
                new SqlParameter("@PersonID", id)
                );

            var row = data.Rows[0];

            var person = new Person();
            person.PersonID = Convert.ToInt32(row["PersonID"]);
            person.FirstName = Convert.ToString(row["FirstName"]);
            person.LastName = Convert.ToString(row["LastName"]);

            return person;

        }

        internal void UpdatePerson(Person person)
        {
            _sqlHelper.RunScript("update person set firstname = @FirstName, lastname = @LastName where PersonID = @PersonID",
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@FirstName", person.FirstName),
                new SqlParameter("@LastName", person.LastName)
                );
        }

        internal void CreatePerson(Person newPerson)
        {

            //Thread.Sleep(1100);

            var startTicks = DateTime.Now.Ticks;

            try
            {

                newPerson.PersonID =
                    Convert.ToInt32(
                        _sqlHelper.RunScriptReturnDt("insert into person values(@FirstName, @LastName) select SCOPE_IDENTITY()",
                        new SqlParameter("@FirstName", newPerson.FirstName),
                        new SqlParameter("@LastName", newPerson.LastName)
                ).Rows[0][0]);

            }
            catch (Exception)
            {
                LoadTestApp.PerfCounters.PerformanceCounterLocator.Instance.PersonRepositoryError.RecordOperation();
                throw;
            }
            finally
            {
                LoadTestApp.PerfCounters.PerformanceCounterLocator.Instance.CreatePerson.RecordOperation(DateTime.Now.Ticks - startTicks);
            }
        }
    }
}