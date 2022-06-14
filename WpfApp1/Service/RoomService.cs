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
        public readonly DoctorService _doctorService;
        public readonly InventoryMovingService _inventoryMovingService;
        public readonly InventoryService _inventoryService;
        public readonly RenovationService _renovationService;
        public readonly AppointmentService _appointmentService;

        public RoomService(IRoomRepository roomRepository, DoctorService doctorService, InventoryMovingService inventoryMovingService,
                            InventoryService inventoryService, RenovationService renovationService, AppointmentService appointmentService)
        {
            _roomRepository = roomRepository;
            _doctorService = doctorService;
            _inventoryMovingService = inventoryMovingService;
            _inventoryService = inventoryService;
            _renovationService = renovationService;
            _appointmentService = appointmentService;
        }
        public IEnumerable<Room> GetAll()
        {
            _renovationService.ExecuteFinishedAdvancedRenovations();
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
            _renovationService.ExecuteFinishedAdvancedRenovations();
            return _roomRepository.GetById(id);
        }
        public IEnumerable<Room> GetAllByType(string type) {
            return _roomRepository.GetAllByType(type);
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
                _doctorService.MoveDoctorsToMainOffice(room.Id);
            }
            if ((oldRoom.Type.Equals("Storage") || oldRoom.Type.Equals("Operating")) && !(room.Type.Equals("Storage") || room.Type.Equals("Operating")))
            {
                _inventoryMovingService.CancelInvenoryMovings(room.Id);
                _inventoryService.MoveInventoryToMainStorage(room.Id);
            }
            if ((oldRoom.Type.Equals("Office") || oldRoom.Type.Equals("Operating")) && !(room.Type.Equals("Office") || room.Type.Equals("Operating")))
            {
                _appointmentService.CancelAppointments(room.Id);
            }
            return _roomRepository.Update(room);
        }

        public bool Delete(int id)
        {

            _appointmentService.CancelAppointments(id);
            _renovationService.CancelRenovations(id);
            _inventoryMovingService.CancelInvenoryMovings(id);
            _inventoryService.MoveInventoryToMainStorage(id);
            _doctorService.MoveDoctorsToMainOffice(id);


            return _roomRepository.Delete(id);
        }

        public Room GetByNametag(string nametag)
        {
            _renovationService.ExecuteFinishedAdvancedRenovations();
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
            _renovationService.ExecuteFinishedAdvancedRenovations();
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
        
    }
}
