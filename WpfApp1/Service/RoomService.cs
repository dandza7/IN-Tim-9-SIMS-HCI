using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.Preview;
using WpfApp1.Repository;
using WpfApp1.Repository.Interface;
using WpfApp1.Repository.Interfaces;

namespace WpfApp1.Service
{
    public class RoomService
    {
        public readonly IRoomRepository _roomRepository;
        public readonly IDoctorRepository _doctorRepository;
        public readonly IInventoryMovingRepository _inventoryMovingRepository;
        public readonly IInventoryRepository _inventoryRepository;
        public readonly IRenovationRepository _renovationRepository;
        public readonly IAppointmentRepository _appointmentRepository;

        public RoomService(IRoomRepository roomRepository, IDoctorRepository doctorRepository, IInventoryMovingRepository inventoryMovingRepository, 
                            IInventoryRepository inventoryRepository, IRenovationRepository renovationRepository, IAppointmentRepository appointmentRepository)
        {
            _roomRepository = roomRepository;
            _doctorRepository = doctorRepository;
            _inventoryMovingRepository = inventoryMovingRepository;
            _inventoryRepository = inventoryRepository;
            _renovationRepository = renovationRepository;
            _appointmentRepository = appointmentRepository;
        }
        public IEnumerable<Room> GetAll()
        {
            ExecuteFinishedAdvancedRenovations();
            List<Room> allRooms = _roomRepository.GetAll().ToList();
            List<Room> activeRooms = new List<Room>();
            foreach (Room room in allRooms)
            {
                if (room.IsActive)
                {
                    activeRooms.Add(room);
                }
            }
            return activeRooms;
        }

        public Room GetById(int id)
        {
            ExecuteFinishedAdvancedRenovations();
            return _roomRepository.GetById(id);
        }

        public Room Create(Room room)
        {
            return _roomRepository.Create(room);
        }

        public Room Update(Room room)
        {
            Room oldRoom = this._roomRepository.GetById(room.Id);
            if (oldRoom.Type.Equals("Office") && !room.Type.Equals(oldRoom.Type))
            {
                MoveDoctorsToMainOffice(room.Id);
            }
            if ((oldRoom.Type.Equals("Storage") || oldRoom.Type.Equals("Operating")) && !(room.Type.Equals("Storage") || room.Type.Equals("Operating")))
            {
                CancelInvenoryMovings(room.Id);
                MoveInventoryToMainStorage(room.Id);
            }
            if ((oldRoom.Type.Equals("Office") || oldRoom.Type.Equals("Operating")) && !(room.Type.Equals("Office") || room.Type.Equals("Operating")))
            {
                CancelAppointments(room.Id);
            }
            return _roomRepository.Update(room);
        }

        public bool Delete(int id)
        {

            CancelAppointments(id);
            CancelRenovations(id);
            CancelInvenoryMovings(id);
            MoveInventoryToMainStorage(id);
            MoveDoctorsToMainOffice(id);


            return _roomRepository.Delete(id);
        }

        public Room GetByNametag(string nametag)
        {
            ExecuteFinishedAdvancedRenovations();
            List<Room> rooms = _roomRepository.GetAll().ToList();
            foreach (Room room in rooms)
            {
                if (room.Nametag.Equals(nametag) && room.IsActive)
                {
                    return room;
                }
            }
            return null;
        }
        public IEnumerable<string> GetEditableNametags()
        {
            ExecuteFinishedAdvancedRenovations();
            List<Room> rooms = _roomRepository.GetAll().ToList();
            List<string> nametags = new List<string>();
            foreach(Room room in rooms)
            {
                if(room.IsActive && room.Id != 1 && room.Id != 2)
                {
                    nametags.Add(room.Nametag);
                }
            }
            return nametags;
        }
        public void ExecuteFinishedAdvancedRenovations()
        {
            List<Renovation> renovations = _renovationRepository.GetAll().ToList();
            foreach(Renovation renovation in renovations)
            {
                if(renovation.Type == "A" && DateTime.Compare(DateTime.Today, DateTime.Parse(renovation.Ending.ToShortDateString())) >= 0)
                {
                    foreach(int id in renovation.RoomsIds)
                    {
                        Room r = _roomRepository.GetById(id);
                        if (!r.IsActive)
                        {
                            r.IsActive = true;
                            _roomRepository.Update(r);
                        } else
                        {
                            _roomRepository.Delete(r.Id);
                        }
                    }
                    _renovationRepository.Delete(renovation.Id);
                }
            }
        }
        private void MoveDoctorsToMainOffice(int roomId)
        {
            List<Doctor> doctors = this._doctorRepository.GetAll().ToList();
            foreach (Doctor doctor in doctors)
            {
                if (doctor.RoomId == roomId)
                {
                    doctor.RoomId = 2;
                    _doctorRepository.Update(doctor);
                }
                    
            }
        }
        private void MoveInventoryToMainStorage(int roomId)
        {
            List<Inventory> inventories = this._inventoryRepository.GetAll().ToList();
            foreach (Inventory inventory in inventories)
            {
                if (inventory.RoomId == roomId)
                {
                    inventory.RoomId = 1;
                    _inventoryRepository.Update(inventory);
                }
            }
            _inventoryRepository.UpdateAll(inventories);
        }
        private void CancelInvenoryMovings(int roomId)
        {
            List<InventoryMoving> inventoryMovings = this._inventoryMovingRepository.GetAll().ToList();
            foreach (InventoryMoving inventoryMoving in inventoryMovings)
            {
                if (inventoryMoving.RoomId == roomId)
                    _inventoryMovingRepository.Delete(inventoryMoving.Id);
            }
        }
        private void CancelAppointments(int roomId)
        {
            List<Appointment> appointments = this._appointmentRepository.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                if (appointment.RoomId == roomId)
                    _appointmentRepository.Delete(appointment.Id);
            }

        }
        private void CancelRenovations(int roomId)
        {
            List<Renovation> renovations = this._renovationRepository.GetAll().ToList();
            foreach (Renovation renovation in renovations)
            {
                if (renovation.RoomsIds.Contains(roomId))
                    _renovationRepository.Delete(renovation.Id);
            }
        }
        //KOD ZA HCI, SIMONA, NE GLEDAJTE OVO :)

        public List<BusynessPreview> GetBusynessPreview()
        {
            List<Appointment> apps = _appointmentRepository.GetAll().ToList();
            List<Renovation> rens = _renovationRepository.GetAll().ToList();
            List<BusynessPreview> retVal = new List<BusynessPreview>();
            foreach (Appointment appointment in apps)
            {
                retVal.Add(new BusynessPreview(_roomRepository.GetById(appointment.RoomId).Nametag, "Appointment", appointment.Beginning, appointment.Ending));
            }
            foreach (Renovation renovation in rens)
            {
                foreach(int id in renovation.RoomsIds)
                {
                    retVal.Add(new BusynessPreview(_roomRepository.GetById(id).Nametag, "Renovation", renovation.Beginning, renovation.Ending));
                }
            }
            return retVal;
        }
    }
}
