using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataLayer.Database.FunctionalityClasses;

namespace DataLayer.Database
{
    public class PersonMap
    {

        private static Dictionary<int, Person> personMap = new Dictionary<int, Person>();
        private static bool gotAll = false;


        public static void addPerson(Person p)
        {
            personMap.Add(p.Id, p);
        }

        public static void addPersonList(List<Person> ppl)
        {
            foreach(Person p in ppl)
            {
                addPerson(p);
            }
        }

        public static Person getPerson(int id)
        {
            if(!personMap.ContainsKey(id))
                addPerson(PersonTable.SelectOne(id));
            return personMap[id];
        }


        public static List<Person> getAll()
        {
            if(gotAll == false)
            {
                deleteData();
                List<Person> l = PersonTable.Select();
                addPersonList(l);
                gotAll = true;
                return personMap.Values.ToList();
            }
            return personMap.Values.ToList();
        }

        public static void setGotAllFalse()
        {
            gotAll = false;
        }
        public static void deleteData()
        {
            personMap.Clear();
        }

    }
}
