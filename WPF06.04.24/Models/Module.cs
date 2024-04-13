using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF06._04._24.Models
{
    public interface IEntity
    {
        int ID { get; set; }
    }
    public class Users : IEntity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
    }
    public class Tickets : IEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int TicketTypeID { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public decimal TicketCost { get; set; }
    }

    public class TicketType : IEntity
    {
        public int ID { get; set; }
        public string TypeName { get; set; }
    }

    public class Equipments : IEntity
    {
        public int ID { get; set; }
        public int EquipmentTypeID { get; set; }
        public string EquipmentName { get; set; }
    }

    public class EquipmentType : IEntity
    {
        public int ID { get; set; }
        public string EquipmentSize { get; set; }
    }

    public class Rental : IEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int EquipmentID { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }

    public class Booking : IEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string Status { get; set; }
    }

    public class Schedule : IEntity
    {
        public int ID { get; set; }
        public int BookingID { get; set; }
        public string EventType { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public DateTime ReservedTime { get; set; }
    }

    public class Pass : IEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }

    public class Qualification : IEntity
    {
        public int ID { get; set; }
        public string QualificationName { get; set; }
        public DateTime DateReceipt { get; set; }
    }

    public class Coaches : IEntity
    {
        public int ID { get; set; }
        public int QualificationID { get; set; }
        public string CoachName { get; set; }
        public string ContactInformation { get; set; }
        public int Experience { get; set; }
    }

    public class Training : IEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CoachID { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

    }

}
