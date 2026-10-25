using CampusNova.Domain.Common;
using CampusNova.Domain.Common.Services;
using CampusNova.Domain.Student.Record;
using CampusNova.Domain.User.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Domain.Student.Model
{
    public class Student : BaseEntity
    {
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Email { get; private set; }
        public long? Identification { get; private  set; }
        public string? Role { get; private set; }


        public static Student Create(CreateStudentData model)
        {
            return new Student()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Identification = model.Identification,
                Role = RolUser.Student
            };
        }

        public static Student Update(UpdateStudentRecord model)
        {
            return new Student()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Identification = model.Identification
            };
        }

    }
}
