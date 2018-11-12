using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUBS
{
    class Authors
    {
        public Authors(string Id, string Name, string Surname)
        {
            this.Id = Id;
            this.Name = Name;
            this.Surname = Surname;
        }

        public Authors(string Name, string Surname, string Phone, string Address, string City, string State, string Zip)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.Phone = Phone;
            this.Address = Address;
            this.City = City;
            this.State = State;
            this.Zip = Zip;
        }

        public Authors(string Id, string Name, string Surname, string Phone, string Address, string City, string State, string Zip)
        {
            this.Id = Id;
            this.Name = Name;
            this.Surname = Surname;
            this.Phone = Phone;
            this.Address = Address;
            this.City = City;
            this.State = State;
            this.Zip = Zip;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
