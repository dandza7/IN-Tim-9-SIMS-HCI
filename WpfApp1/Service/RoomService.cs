using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class RoomService
    {
        public readonly RoomRepository _roomRepository;
        public readonly DoctorRepository _doctorRepository;
        public readonly InventoryMovingRepository _inventoryMovingRepository;
        public readonly InventoryRepository _inventoryRepository;
        public readonly RenovationRepository _renovationRepository;
        public readonly NotificationRepository _notificationRepository;
        public readonly AppointmentRepository _appointmentRepository;

        public RoomService(RoomRepository roomRepository, DoctorRepository doctorRepository, InventoryMovingRepository inventoryMovingRepository, 
                            InventoryRepository inventoryRepository, RenovationRepository renovationRepository,
                            NotificationRepository notificationRepository, AppointmentRepository appointmentRepository)
        {
            _roomRepository = roomRepository;
            _doctorRepository = doctorRepository;
            _inventoryMovingRepository = inventoryMovingRepository;
            _inventoryRepository = inventoryRepository;
            _renovationRepository = renovationRepository;
            _notificationRepository = notificationRepository;
            _appointmentRepository = appointmentRepository;

        }
        public List<Room> GetAll()
        {
            ExecuteFinishedAdvancedRenovations();
            List<Room> allRooms = _roomRepository.GetAll();
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
            return _roomRepository.Get(id);
        }

        public Room Create(Room room)
        {
            return _roomRepository.Create(room);
        }

        public Room Update(Room room)
        {
            Room oldRoom = this._roomRepository.Get(room.Id);
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
        public int GetIdByNametag(string nametag)
        {
            ExecuteFinishedAdvancedRenovations();
            List<Room> rooms = _roomRepository.GetAll();
            foreach(Room room in rooms)
            {
                if (room.Nametag.Equals(nametag) && room.IsActive)
                {
                    return room.Id;
                }
            }
            return -1;
        }
        public Room GetByNametag(string nametag)
        {
            ExecuteFinishedAdvancedRenovations();
            List<Room> rooms = _roomRepository.GetAll();
            foreach (Room room in rooms)
            {
                if (room.Nametag.Equals(nametag) && room.IsActive)
                {
                    return room;
                }
            }
            return null;
        }
        public List<string> GetEditableNametags()
        {
            ExecuteFinishedAdvancedRenovations();
            List<Room> rooms = _roomRepository.GetAll();
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
            List<Renovation> renovations = _renovationRepository.GetAll();
            foreach(Renovation renovation in renovations)
            {
                if(renovation.Type == "A" && DateTime.Compare(DateTime.Today, DateTime.Parse(renovation.Ending.ToShortDateString())) >= 0)
                {
                    foreach(int id in renovation.RoomsIds)
                    {
                        Room r = _roomRepository.Get(id);
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

    }
}
